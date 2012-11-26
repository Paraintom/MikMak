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
        Dictionary<int, IGame> GetAllMappings();
        IGame GetGame(int typeGame);
    }
}
