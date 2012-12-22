using System.Collections.Generic;
namespace MikMak.DomainModel.Entities
{

    /// <summary>
    /// This class store some data about a player.
    /// </summary>
    public class Player
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

        public virtual ICollection<Battle> Battles { get; set; }
    }
}
