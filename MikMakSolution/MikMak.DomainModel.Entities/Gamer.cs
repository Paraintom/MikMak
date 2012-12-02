using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMak.Game.Entities
{
    /// <summary>
    /// This class store some data about a player.
    /// </summary>
    public class Gamer
    {
        /// <summary>
        /// Unique id for a player
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// login string
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
    }
}
