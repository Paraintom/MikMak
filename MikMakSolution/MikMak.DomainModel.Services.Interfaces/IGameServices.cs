namespace MikMak.Interfaces
{
    using System.Collections.Generic;
    using MikMak.DomainModel.Entities;

    /// <summary>
    /// Very Important interface that each game library should implement only once.
    /// </summary>
    public interface IGameServices
    {
        /// <summary>
        /// Return the type id of the game
        /// </summary>
        /// <returns>The type id of the game</returns>
        int GetGameType();

        /// <summary>
        /// Return the gameId of a new game
        /// </summary>
        /// <returns>A gameId of a new Game</returns>
        string GetNewGame();

        /// <summary>
        /// Return the State of the game witch id is gameId 
        /// </summary>
        /// <param name="gameId">The gameId</param>
        /// <returns>The current state</returns>
        Grid GetState(string gameId);       

        /// <summary>
        /// Play a move for a gameID
        /// </summary>
        /// <param name="gameId">The gameId</param>
        /// <param name="move">The move played</param>
        /// <returns>The next game state</returns>
        Grid Play(string gameId, Move move);

        ///???
        List<Battle> GetAllGames(Player player);
    }
}
