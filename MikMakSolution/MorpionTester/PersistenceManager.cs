using System;
using System.Collections.Generic;
using MikMak.Commons;
using MikMak.Interfaces;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MorpionTester
{
    internal class PersistenceManager : IPersistenceManager
    {
        public int CreateAccout(string login, string password)
        {
            throw new NotImplementedException();
        }

        public AccountOverview GetAccountOverview(string login)
        {
            return new AccountOverview()
            {
                Login = login,
                Password = login,
                PlayerId = GetPlayerId(login)
            };
        }

        public static int GetPlayerId(string login)
        {
            return login.Length + (int)login[1];
        }

        public void CreateGame(string gameId, GridState initialState, List<int> playerInvolved)
        {
            throw new NotImplementedException();
        }

        public List<GameOverview> GetAllGames(int playerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateState(string gameId, GridState newState)
        {
            throw new NotImplementedException();
        }

        public GridState GetState(string gameId)
        {
            throw new NotImplementedException();
        }

        public GameOverview GetGameOverview(string gameId)
        {
            if (gameId != "test")
            {
                throw new Exception("Game not Found");
            }
            else
            {
                return new GameOverview()
                {
                    CreationTime = DateTime.Now.AddDays(-1),
                    GameId = "Morpion009",
                    GameType = 1,
                    GameTypeString = "Morpion",
                    LastUpdate = DateTime.Now.AddDays(-1),
                    Players = new List<int>(){
                    GetPlayerId("tom"),
                    GetPlayerId("oliv")
                }
                };
            }
        }
    }
}
