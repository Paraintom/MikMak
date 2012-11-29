using System;
using System.Collections.Generic;
using MikMak.Commons;
using MikMak.Interfaces;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Morpion
{
    internal class SavedGamesDAO : IPersistenceManager
    {
        //private string path = @".\datas.bin";
        private List<GridState> allGames;

        public SavedGamesDAO()
        {
            try
            {
                allGames = Deserialiser();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception : " + e);
                allGames = new List<GridState>();
            }
        }

        private List<GridState> Deserialiser()
        {
            List<GridState> toReturn = new List<GridState>();
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(List<GridState>));
                using (var reader = XmlReader.Create("TestData.xml"))
                {
                    toReturn = (List<GridState>)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception : " + e);
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
            return toReturn;
        }

        private void Serialiser(List<GridState> toReturn)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(toReturn.GetType());
                using (var writer = XmlWriter.Create("TestData.xml"))
                {
                    serializer.Serialize(writer, toReturn);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception : " + e);
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }


        public GridState GetState(string gameId)
        {
            var allGamesE = Deserialiser();
            if (allGamesE.Count != 0)
            {
                return allGamesE[0];
            }
            throw new Exception("Game does not exist!");
        }

        private void SaveState(string gameId, GridState toSave)
        {
            var current = new List<GridState>() { toSave };
            Serialiser(current);
        }

        public int CreateAccout(string login, string password)
        {
            throw new NotImplementedException();
        }

        public AccountOverview GetAccountOverview(string login)
        {
            throw new NotImplementedException();
        }

        public void CreateGame(string gameId, GridState initialState, List<int> playerInvolved)
        {
            SaveState(gameId, initialState);
        }

        public List<GameOverview> GetAllGames(int playerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateState(string gameId, GridState newState)
        {
            SaveState(gameId, newState);
        }

        public GameOverview GetGameOverview(string gameId)
        {
            throw new NotImplementedException();
        }
    }
}
