using System;
using System.Collections.Generic;
using System.Linq;
using MikMak.Interfaces;

namespace MikMak.Main.InternalInterfaces
{
    /// <summary>
    /// Class doing mapping between a id type and the game provider.
    /// </summary>
    internal interface ITypeGameMapping
    {
        /// <summary>
        /// Retrieve all the mappings between a game type and a game.
        /// </summary>
        /// <returns>All values</returns>
        Dictionary<int, IGame> GetAllMappings();

        /// <summary>
        /// Retrieve a game based on his game type
        /// </summary>
        /// <param name="typeGame">The game's type</param>
        /// <returns>The game</returns>
        IGame GetGame(int typeGame);
    }
}
