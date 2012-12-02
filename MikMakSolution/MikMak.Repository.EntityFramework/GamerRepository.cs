using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.Game.Entities;
using MikMak.Infrastructure.Ressource;

namespace MikMak.Repository.EntityFramework
{
    class GamerRepository : IGamerRepository
    {
        private Context _Context;

        public GamerRepository(Context context)
        {
            _Context = context;
        }


        /// <summary>
        /// Creates the gamer.
        /// </summary>
        /// <param name="gamer">The gamer.</param>
        /// <returns>errror message, if login already exists</returns>
        public string CreateGamer(Gamer gamer)
        {
            string errorMsg = string.Empty;
            if (_Context.Gamers.Any(g => g.Login == gamer.Login))
            {
                errorMsg = Error.LOGIN_ALREADY_EXISTS;
            }
            else
            {
                _Context.Gamers.Add(gamer);
                _Context.SaveChanges();
            }
            return errorMsg;
        }
    }
}
