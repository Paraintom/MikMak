using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMak.Commons
{
    /// <summary>
    /// This class store some meta-data about a game.
    /// </summary>
    public class GameOverview
    {
        /// <summary>
        /// Unique id for a game
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// GameType unique id, can be used for statistiques
        /// </summary>
        public int GameType { get; set; }

        /// <summary>
        /// English description of game type
        /// </summary>
        public string GameTypeString { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// List of all other players id involved
        /// </summary>
        public List<int> Players { get; set; }
    }
}
