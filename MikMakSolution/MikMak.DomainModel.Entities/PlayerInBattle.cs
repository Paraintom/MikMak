namespace MikMak.DomainModel.Entities
{
    using System;

    /// <summary>
    /// Class that help to get a rid of authentification.
    /// A session exist in the system only for a certain amount of time and expire.
    /// Then the player has to reconnect.
    /// </summary>
    public class PlayerInBattle
    {
        public int BattleId { get; set; }
        public int PlayerId { get; set; }



        /// <summary>
        /// Gets or sets the player in battle id.
        /// </summary>
        /// <value>
        /// The player in battle id.
        /// </value>
        public int PlayerInBattleId { get; set; }

        /// <summary>
        /// Player number in the game, by convention start at 1.
        /// 0 = not defined
        /// </summary>
        public int PlayerNumber { get; set; }

        /// <summary>
        /// Player Id.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The game type can be Morpion, Go or Chess.
        /// 0 = not defined
        /// </summary>
        public Battle Battle { get; set; }
    }
}
