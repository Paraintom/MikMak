using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMak.Main.InternalInterfaces
{
    /// <summary>
    /// This interface is used for defining and retrieving the link between a game and the players.
    /// </summary>
    internal interface ILinkPlayersGames
    {
        /// <summary>
        /// Link players to a game.
        /// </summary>
        /// <param name="players">The list of player Ids</param>
        /// <param name="gameId">The game Id</param>
        void LinkedPlayersToGame(List<int> players, string gameId);

        /// <summary>
        /// Get the list of players in a sorted list (player number 1 is in index 0)
        /// </summary>
        /// <param name="gameId">The game Id</param>
        /// <returns>The sorted list</returns>
        List<int> GetPlayersInvolved(string gameId);
    }
}
