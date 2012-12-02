using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MikMak.DomainModel.Entities;
using MikMak.Repository.Interfaces;
using MikMak.Configuration;

namespace MikMak.WebFront.Sessions
{
    /// <summary>
    /// Class for creating session and managing life cycle.
    /// Can be configured via App.config with keys "sessionTimeOut" and "sessionCleaning"
    /// </summary>
    public class SessionManager : ISessionManager
    {
        private readonly int numberOfHourBeforeTimeOut = MyConfiguration.GetInt("sessionTimeOut", 1);
        private readonly int numberOfMillisecondsBetweenCleanup = MyConfiguration.GetInt("sessionCleaning", 1000 * 60 * 60);
        private object internalLock = new object();
        private Dictionary<string, Session> allSessions = new Dictionary<string, Session>();
        private Timer cleanUpTimer;
        private IPlayerRepository daoPlayer;
        private IPlayerInBattleRepository daoPlayerInBattle;

        public SessionManager(IPlayerRepository daoPlayer, IPlayerInBattleRepository daoPlayerInBattle)
        {
            this.daoPlayer = daoPlayer;
            this.daoPlayerInBattle = daoPlayerInBattle;
            cleanUpTimer = new Timer(numberOfMillisecondsBetweenCleanup);
            cleanUpTimer.Elapsed += CleanUpTimer_Elapsed;
            cleanUpTimer.AutoReset = true;
            cleanUpTimer.Start();
        }

        public Session GetSession(string login, string password)
        {
            Player playerOverview = null;
            try
            {
                playerOverview = daoPlayer.Get(login);
            }
            catch (Exception e)
            {                
                // Exception caugth from persistenceManager, technical raison, not buiness
                var toRethrow = new InvalidCredentialException(e.Message);
                throw toRethrow;
            }

            // No result found
            if (playerOverview == null)
            {
                throw new InvalidCredentialException(InvalidCredentialEnum.NoLoginFound);
            }

            // Invalid Password
            if (playerOverview.Password !=password)
            {
                throw new InvalidCredentialException(InvalidCredentialEnum.BadPassword);
            }

            var session = new Session
            {
                PlayerInBattle = new PlayerInBattle
                {
                    Battle = null,
                    Player = playerOverview
                },
                Id = Guid.NewGuid().ToString(),
                MaxValidity = DateTime.UtcNow.AddHours(numberOfHourBeforeTimeOut)
            };

            Add(session);
            return session;
        }

        public Session GetSession(string sessionId)
        {
            lock (internalLock)
            {
                Session result;
                //// Check session exist and not expired
                if (this.allSessions.TryGetValue(sessionId, out result) && result.MaxValidity > DateTime.UtcNow)
                {
                    return result;
                }
                else
                {
                    throw new InvalidCredentialException(InvalidCredentialEnum.UnknownSession);
                }
            }
        }

        public Session GetSession(Session otherSession, string gameId)
        {
            var gameOverview = this.daoPlayerInBattle.Get(gameId, otherSession.PlayerInBattle.Player.PlayerId);
            if (gameOverview == null)
            {
                throw new InvalidCredentialException(InvalidCredentialEnum.PlayerNotInvolvedInGame);
            }

            var session = new Session
            {
                PlayerInBattle = gameOverview,
                Id = Guid.NewGuid().ToString(),
                MaxValidity = DateTime.UtcNow.AddHours(numberOfHourBeforeTimeOut)
            };
            Add(session);
            return session;
        }

        //public Session GetSession(Session otherSession, string gameId, int gameType, int playerNumber)
        //{
        //    var session = new Session
        //    {
        //        GameId = gameId,
        //        GameType = gameType,
        //        Id = Guid.NewGuid().ToString(),
        //        MaxValidity = DateTime.UtcNow.AddHours(numberOfHourBeforeTimeOut),
        //        PlayerId = otherSession.PlayerId,
        //        PlayerNumber = playerNumber
        //    };
        //    Add(session);
        //    return session;
        //}

        private void Add(Session session)
        {
            lock (internalLock)
            {
                this.allSessions.Add(session.Id, session);
            }
        }

        private void CleanUpTimer_Elapsed(object sender, ElapsedEventArgs args)
        {
            lock (internalLock)
            {
                List<string> keyOutOfDate = new List<string>();
                //// Retrieving out of date sessions
                foreach (var keyValuePair in allSessions)
                {
                    if (keyValuePair.Value != null && keyValuePair.Value.MaxValidity < DateTime.UtcNow)
                    {
                        keyOutOfDate.Add(keyValuePair.Key);
                    }
                }

                //// Removing outOfDate sessions
                foreach (string key in keyOutOfDate)
                {
                    allSessions.Remove(key);
                }
            }
        }
    }
}
