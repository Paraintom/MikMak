using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Interfaces
{
    /// <summary>
    /// Link Between Application and Persisted Data
    /// </summary>
    public interface IPersistenceManager
    {
        /// <summary>
        /// No Comment
        /// </summary>
        /// <param name="login">The login</param>
        /// <param name="password">The Password</param>
        /// <returns>The player id</returns>
        int CreateAccout(string login, string password);

        /// <summary>
        /// Get the credentials persisted from a login, throw Exceptions if something wrong
        /// </summary>
        /// <param name="login">the login</param>
        /// <returns>All player Infos</returns>
        Player GetAccountOverview(string login);

        /// <summary>
        /// Persist a Game with the link with players involved
        /// </summary>
        /// <param name="gameId">the game id</param>
        /// <param name="initialState">the initial state</param>
        /// <param name="playerInvolved">The list of player involved (better to persist starting at 1 instead of 0)</param>
        void CreateGame(string gameId, Grid initialState, List<int> playerInvolved);

        /// <summary>
        /// Return all the active game of a player
        /// </summary>
        /// <param name="playerId">The player id</param>
        /// <returns>All the games</returns>
        List<Battle> GetAllGames(int playerId);

        /// <summary>
        /// No Comment, update also the last update of a game
        /// </summary>
        /// <param name="GameId">The game Id</param>
        /// <param name="newState">The new state</param>
        void UpdateState(string gameId, Grid newState);

        /// <summary>
        /// No comment
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <returns>The current state</returns>
        Grid GetState(string gameId);

        /// <summary>
        /// No comment
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <returns>The game overview</returns>
        Battle GetGameOverview(string gameId);
    }
}
