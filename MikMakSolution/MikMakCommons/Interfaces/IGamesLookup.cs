using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Commons;

namespace MikMak.Interfaces
{
    /// <summary>
    /// Interface for retrieving all the current game of a player.
    /// </summary>
    public interface IGamesLookup
    {
        List<GameOverview> GetAllGames(Session sessionId);
    }
}
