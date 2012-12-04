using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Interfaces
{
    /// <summary>
    /// Interface For Player managment
    /// </summary>
    public interface IPlayerManager
    {
        /// <summary>
        /// Method for creating a new player
        /// </summary>
        /// <param name="login">The login</param>
        /// <param name="password">The password</param>
        /// <returns>A new player!</returns>
        Player GetNewPlayer(string login, string password);

        /// <summary>
        /// Get a player from his login
        /// </summary>
        /// <param name="login">The login</param>
        /// <returns>The player</returns>
        Player Get(string login);
    }
}
