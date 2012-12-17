using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MikMak.DomainModel.Entities;
using MikMak.Interfaces;
using MikMak.WebFront.Sessions;
using MikMak.Main;
using MikMak.Main.PlayerManagement;
using MikMak.Main.GamesManagement;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class MikmakController : ApiController
    {
        //TODO : to initialise and USE!!!
        private IPlayerManager playerManager;
        private IGamesManager gameManager;
        private static ISessionManager sessionManager;
        private static string erreurConstructeur;

        public MikmakController()
        {
            try
            {
                playerManager = new PlayerManager();
                gameManager = GameManager.GetInstance();
                if (sessionManager == null)
                {
                    sessionManager = new SessionManager(playerManager, gameManager);
                }
                erreurConstructeur = "No error";
            }
            catch (Exception e)
            {
                erreurConstructeur = e.Message+" inner = "+e.InnerException.Message;
            }
        }

        #region Connection : Connect
        /// <summary>
        /// Initial connection
        /// </summary>
        /// <param name="login">the login</param>
        /// <param name="password">the password</param>
        /// <returns>a sessionId string</returns>
        [AcceptVerbs("GET")]
        public string Connect(string login, string password)
        {
            string toReturn = string.Empty;
            try{
                HackJsonp();

                try
                {
                    var firstSession = sessionManager.GetSession(login, password);
                    toReturn = firstSession.Id;
                }
                catch (Exception ex)
                {
                    toReturn = ex.Message + " erreurConstructeur " + erreurConstructeur;
                }

            }
            catch (Exception ex) { toReturn = ex.Message; }
            return toReturn;
        }

         /// <summary>
        /// Connection to a specific Game
        /// </summary>
        /// <param name="sessionId">A valid session</param>
        /// <param name="gameId">the gameId</param>
        /// <returns>a sessionId string</returns>
        [AcceptVerbs("GET")]
        public string ConnectToGame(string sessionId, string gameId)
        {
            HackJsonp();

            string toReturn = string.Empty;
            try
            {
                var session = sessionManager.GetSession(sessionId,false);
                var newSession = sessionManager.GetSession(session, gameId);
                toReturn = newSession.Id;
            }
            catch (Exception ex)
            {
                toReturn = ex.Message + " erreurConstructeur "+erreurConstructeur;
            }
            return toReturn;
        }
        #endregion

        #region Show list of battles : GetListBattles
        [AcceptVerbs("GET")]
        public List<BattleExtented> GetListBattles(string sessionId)
        {
            HackJsonp();
            List<BattleExtented> toReturn = new List<BattleExtented>();
            try
            {
                var session = sessionManager.GetSession(sessionId, false);
                var allBattle = gameManager.GetAllBattles(session.PlayerInBattle.Player).Select(o => o.Battle).ToList();
                foreach (var battle in allBattle)
                {
                    List<Player> playersInGame = new List<Player>();
                    BattleExtented battleEx = new BattleExtented()
                    {
                        Battle = battle
                    };
                    foreach (int playerId in battle.Players)
                    {
                        var player = playerManager.Get(playerId);
                        playersInGame.Add(new Player()
                        {
                            Login = player.Login,
                            PlayerId = player.PlayerId
                        }
                        );
                    }
                    battleEx.InGame = playersInGame;
                    toReturn.Add(battleEx);
                }
                return toReturn;
            }
            catch(Exception){
                // J'ai pas trouvé mieux POUR l'instant.
                throw;
            }            
        } 
        #endregion

        #region Create a new battle : CreateBattle
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2, string opponent3)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2, opponent3 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2, string opponent3, string opponent4)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2, opponent3, opponent4 });
        }

        public GridExtented CreateBattle(string sessionId, int typeGame, params string[] opponents)
        {
            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId, false);

                // 2-Check valid opponents
                List<Player> allOpponents = new List<Player>();
                foreach(string opp in opponents){
                    var player = playerManager.Get(opp);
                    if(!allOpponents.Contains(player)){
                        allOpponents.Add(player);
                    }
                    else{
                        throw new ArgumentException("You specified a login twice");
                    }
                }

                // 3-Create new game
                var newGame = gameManager.GetNewGame(session.PlayerInBattle.Player, typeGame, allOpponents);
                var newSession = sessionManager.GetSession(session, newGame.Battle.GameId);

                // 4-Return result
                return new GridExtented(){
                    SessionId = newSession.Id,
                    State = newGame.Battle.CurrentState
                };
            }
            catch (Exception e)
            {
                var toReturn = e.Message + " erreurConstructeur " + erreurConstructeur;
                return new GridExtented()
                {
                    SessionId = toReturn,
                    State = new Grid()
                    {
                        NumberColumns = 5,
                        NumberLines = 1
                    }
                };
            }
        } 
        #endregion

        #region Get grid state : GetGrid
        [AcceptVerbs("GET")]
        public GridExtented GetGrid(string sessionId)
        {
            HackJsonp();
            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId, true);

                // 2-Return result
                return new GridExtented()
                {
                    SessionId = session.Id,
                    State = session.PlayerInBattle.Battle.CurrentState
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AcceptVerbs("GET")]
        public GridExtented GetGrid(string sessionId, string gameId)
        {
            HackJsonp();

            var newSession = this.ConnectToGame(sessionId, gameId);
            sessionId = newSession;

            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId, true);

                // 2-Return result
                return new GridExtented()
                {
                    SessionId = session.Id,
                    State = session.PlayerInBattle.Battle.CurrentState
                };
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion

        #region Play a move : Play
        [AcceptVerbs("GET")]
        public GridExtented Play(string sessionId, int x1, int y1)
        {
            var tuple = new Tuple<int, int>(x1, y1);
            return Play(sessionId, new List<Tuple<int, int>>() { tuple });
        }

        [AcceptVerbs("GET")]
        public GridExtented Play(string sessionId, int x1, int y1, int x2, int y2)
        {
            var tuple = new Tuple<int, int>(x1, y1);
            var tupleBis = new Tuple<int, int>(x2, y2);
            return Play(sessionId, new List<Tuple<int, int>>() { tuple, tupleBis });
        }

        private GridExtented Play(string sessionId, List<Tuple<int,int>> allPositions)
        {
            HackJsonp();
            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId, true);

                var positions = new List<Pawn>();
                foreach (var item in allPositions)
	            {
		            positions.Add(new Pawn(){
                            Coord = new Coord(){
                                x = item.Item1,
                                y = item.Item2
                            },
                            Name = "?",
                            Player = session.PlayerInBattle.Player
                    });
	            }


                Move move = new Move()
                {
                    PlayerNumber = session.PlayerInBattle.PlayerNumber,
                    Positions = positions
                };
                var newState = gameManager.Play(session.PlayerInBattle, move);

                // 2-Return result
                return new GridExtented()
                {
                    SessionId = session.Id,
                    State = newState
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion

        private void HackJsonp()
        {
            var negotiator = Configuration.Services.GetContentNegotiator();
            var negotiatorResult = negotiator.Negotiate(typeof(string), Request, Configuration.Formatters);
        } 
    }
}
