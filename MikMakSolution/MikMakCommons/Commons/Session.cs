using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMak.Commons
{
    /// <summary>
    /// Class that help to get a rid of authentification.
    /// A session exist in the system only for a certain amount of time and expire.
    /// Then the player has to reconnect.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// The Session Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Player number in the game, by convention start at 1.
        /// 0 = not defined
        /// </summary>
        public int PlayerNumber { get; set; }

        /// <summary>
        /// Player Id.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// The game type can be Morpion, Go or Chess.
        /// 0 = not defined
        /// </summary>
        public int GameType { get; set; }

        /// <summary>
        /// The unique id of the game
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// Date when the session expire.
        /// </summary>
        public DateTime MaxValidity { get; set; }
    }
}
