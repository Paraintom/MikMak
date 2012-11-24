using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMakCommons
{
    /// <summary>
    /// A move come for a player and has to be forwarded to a <code>IGameManager</code>.
    /// It contains all the action he wants to do.
    /// </summary>
    public class Move
    {
        /// <summary>
        /// The player Id (1 or 2) for Tic tac toe for example
        /// </summary>
        public int PlayerId { get; set; }
        /// <summary>
        /// Position taken by the Pawn played.
        /// For Tic tac toe, this list has length of 1,
        /// for chess the lengh is 2 (initial position, end position),
        /// For the draughts, it can be more than 2...
        /// (This is linked with the prop <code>GridState.MoveNumber</code>)
        /// </summary>
        public List<Pawn> Positions { get; set; }
    }
}
