using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMakCommons
{
    /// <summary>
    /// That class represent the state of a game.    
    /// </summary>
    public class GridState
    {
        /// <summary>
        /// Number of lines in the grid
        /// </summary>
        public int NumberLines { get; set; }
        /// <summary>
        /// Number of column in the grid
        /// </summary>
        public int NumberColumns { get; set; }
        /// <summary>
        /// is true for Go game for example, when you play On the line, not in a square.
        /// </summary>
        public bool IsGridShifted { get; set; }
        /// <summary>
        /// Give the number of move to register before sending back to Service (Chess : 2, Tic tac toe : 1)
        /// </summary>
        public int MoveNumber { get; set; }

        /// <summary>
        /// Give the List of pawn in the game.  
        /// </summary>
        public List<Pawn> PawnLocations { get; set; }
        /// <summary>
        /// Give the number of player to play.  
        /// </summary>        
        public int NextPlayerToPlay { get; set; }
        /// <summary>
        /// Give a message.  
        /// </summary>        
        public Message CurrentMessage { get; set; }

        public GridState() {
            PawnLocations = new List<Pawn>();
            CurrentMessage = Message.GetMessage(ClassicMessage.Default);
        }
    }
}
