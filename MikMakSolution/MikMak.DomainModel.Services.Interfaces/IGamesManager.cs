using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Interfaces
{
    /// <summary>
    /// Interface Between the Client and The system.
    /// </summary>
    public interface IGamesManager
    {
        /// <summary>
        /// Create the battle and assign to each player a place in the game.
        /// </summary>
        /// <param name="initialSession">The player who create the game</param>
        /// <param name="gameType">the type of game</param>
        /// <param name="opponents">the list of all opponents</param>
        /// <returns>Return the information about the link between a player and the new game created</returns>
        PlayerInBattle GetNewGame(Player firstPlayer, int gameType, List<Player> opponents);

        /// <summary>
        /// Return the State of the battle 
        /// </summary>
        /// <param name="battle">the battle</param>
        /// <returns>The current game state</returns>
        Grid GetState(Battle battle);       

        /// <summary>
        /// Play a move durring a session
        /// </summary>
        /// <param name="player">the player and the battle</param>
        /// <param name="move">The move played</param>
        /// <returns>The new game state</returns>
        Grid Play(PlayerInBattle player, Move move);
    }
}
