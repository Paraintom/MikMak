using System;
using MikMak.Infrastructure.Ressource;
using MikMak.Repository.Interfaces;

namespace MikMak.Repository.EntityFramework
{
    using System.Linq;

    using MikMak.DomainModel.Entities;

    public class PlayerRepository : IPlayerRepository
    {
        private Context _Context;
        private object internalLock = new object();

        public PlayerRepository(Context context)
        {
            _Context = context;
        }

        public Player CreatePlayer(string login, string password)
        {
            if (_Context.Players.Any(g => g.Login == login))
            {
                throw new Exception(Error.LOGIN_ALREADY_EXISTS);
            }
            else
            {
                lock (internalLock)
                {
                    Player newPlayer = new Player
                    {
                        Login = login,
                        Password = password,
                        // Auto Increment managment, Carefull, need the lock!
                        PlayerId = _Context.Players.Count() + 1
                    };
                    _Context.Players.Add(newPlayer);
                    _Context.SaveChanges();
                    return newPlayer;
                }
            }
        }

        public Player Get(string login)
        {
            throw new System.NotImplementedException();
        }
    }
}
