namespace MikMak.DomainModel.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class store some meta-data about a game.
    /// </summary>
    public class Battle
    {

        public int BattleId { get; set; }

        /// <summary>
        /// Unique id for a game
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// GameType id, can be used for statistiques
        /// </summary>
        public int GameType { get; set; }

        /// <summary>
        /// English description of game type
        /// </summary>
        public string GameTypeString { get; set; }

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// The last update time
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// List of all other players id involved
        /// </summary>
        public virtual ICollection<Player> Players { get; set; }

        /// <summary>
        /// The current grid
        /// </summary>
        public Grid CurrentState { get; set; }
    }
}
