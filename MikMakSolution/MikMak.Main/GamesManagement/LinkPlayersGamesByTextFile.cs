using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MikMak.Main.InternalInterfaces;
using MikMak.Configuration;

namespace MikMak.Main.GamesManagement
{
    /// <summary>
    /// Class that manage the creation and the retrieval of the links between a player and a game.
    /// </summary>
    internal class LinkPlayersGamesByTextFile : ILinkPlayersGames
    {
        /// <summary>
        /// The file where are stocked the infos about the link between players and games.
        /// </summary>
        private string file = MyConfiguration.GetString("linkPlayersGamesFile", "./data/linkPlayersGames.txt");

        /// <summary>
        /// Internal representation of the data
        /// </summary>
        private Dictionary<string, List<int>> data = new Dictionary<string, List<int>>();

        public LinkPlayersGamesByTextFile()
        {
            Initialize();
        }

        /// <summary>
        /// See Interface
        /// </summary>
        /// <param name="players">See Interface</param>
        /// <param name="gameId">See Interface</param>
        public void LinkedPlayersToGame(List<int> players, string gameId)
        {
            List<string> toWrite = new List<string>();
            for (int i = 0; i < players.Count; i++)
            {
                string line = String.Format("{0} {1} {2}", gameId, players[i], i + 1);
                toWrite.Add(line);
            }
            File.AppendAllLines(file, toWrite);
        }

        /// <summary>
        /// See Interface
        /// </summary>
        /// <param name="gameId">See Interface</param>
        /// <returns>See Interface</returns>
        public List<int> GetPlayersInvolved(string gameId)
        {
            List<int> result = null;
            data.TryGetValue(gameId, out result);
            return result;
        }

        private void Initialize()
        {
            if (!File.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            else
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        AddFileLine(line.Trim());
                    }
                }
            }
        }

        /// <summary>
        /// We expect a line be like this : "gameId playerId playerNumber"
        /// Example : "gameId5003 244 1"
        /// </summary>
        /// <param name="line">The line to parse</param>
        private void AddFileLine(string line)
        {
            string[] allParts = line.Split(' ');
            if (allParts.Length != 3)
            {
                string exceptionString = String.Format("The line [{0}] has incorectect number of part, should be 3, is {1}", line, allParts.Length);
                throw new FormatException(exceptionString);
            }
            string gameId = allParts[0];
            int playerId = Int32.Parse(allParts[1]);
            int playerNumber = Int32.Parse(allParts[2]);

            /// log line read
            if (data.ContainsKey(gameId))
            {
                data[gameId].Insert(playerNumber, playerId);
            }
        }
    }
}
