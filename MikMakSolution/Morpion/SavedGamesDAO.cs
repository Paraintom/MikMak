using System;
using System.Collections.Generic;
using MikMakCommons;
using MikMakCommons.Interfaces;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Morpion
{
    internal class SavedGamesDAO : IPersistenceManager
    {
        private string path = @".\datas.bin";
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
            catch(Exception e)
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
            catch(Exception e)
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
            if (allGamesE.Count !=0)
            {
                return allGamesE[0];
            }

            var newState = GetNewGameGridState();
            allGames.Add(newState);
            return newState;
        }

        public void SaveState(string gameId, GridState toSave)
        {
            var current = new List<GridState>() { toSave };
            Serialiser(current);
        }

        private static GridState GetNewGameGridState()
        {
            return new GridState()
            {
                CurrentMessage = Message.GetMessage(MorpionMessage.NewGame),
                IsGridShifted = false,
                MoveNumber = 1,
                NextPlayerToPlay = 1,
                NumberColumns = 3,
                NumberLines = 3
            };
        }
    }
}
