using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMakCommons.Interfaces
{
    /// <summary>
    /// Very Important interface that each .dll game should implement only Once.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Return the State of the game witch id is gameId 
        /// </summary>
        /// <param name="gameId">the gameId</param>
        /// <returns>The current state</returns>
        GridState GetState(string gameId);
        /// <summary>
        /// Play a move for a gameID
        /// </summary>
        /// <param name="gameId">the gameId</param>
        /// <param name="move">The move played</param>
        /// <returns></returns>
        GridState Play(string gameId, Move move);
    }
}
