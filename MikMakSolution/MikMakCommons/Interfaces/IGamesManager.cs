using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Commons;

namespace MikMak.Interfaces
{
    /// <summary>
    /// Interface Between the Client and The system.
    /// </summary>
    public interface IGamesManager
    {
        /// <summary>
        /// Return the sessionId of a new game
        /// </summary>
        /// <param name="initialSession">a valid session</param>
        /// <param name="gameType">the type of game</param>
        /// <param name="opponent">the name of the opponent</param>
        /// <returns>a session id of the new Game</returns>
        string GetNewGame(Session initialSession, int gameType, string opponent);

        /// <summary>
        /// Return the State of the game link with the session 
        /// </summary>
        /// <param name="sessionId">the sessionId</param>
        /// <returns>The current game state</returns>
        GridState GetState(Session sessionId);       

        /// <summary>
        /// Play a move durring a session
        /// </summary>
        /// <param name="sessionId">the sessionId</param>
        /// <param name="move">The move played</param>
        /// <returns>The next game state</returns>
        GridState Play(Session sessionId, Move move);
    }
}
