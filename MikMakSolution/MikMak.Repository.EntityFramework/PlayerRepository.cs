using System;
using MikMak.Infrastructure.Ressource;
using MikMak.Repository.Interfaces;

namespace MikMak.Repository.EntityFramework
{
    using System.Linq;

    using MikMak.DomainModel.Entities;
    using System.Data;
    using System.Data.Common;

    public class PlayerRepository : IPlayerRepository
    {
        private Context _Context;

        public PlayerRepository(IDbConnection connection)
        {
            DbConnection connec = connection as DbConnection;
            if (connec == null)
            {
                throw new Exception("Incorrect connection");
            }
            _Context = new Context(connec);
        }

        public Player CreatePlayer(string login, string password)
        {
            if (_Context.Players.Any(g => g.Login == login))
            {
                throw new Exception(Error.LOGIN_ALREADY_EXISTS);
            }
            Player newPlayer = new Player
            {
                Login = login,
                Password = password,
            };
            _Context.Players.Add(newPlayer);
            _Context.SaveChanges();
            return newPlayer;
        }

        public Player Get(string login)
        {
            throw new System.NotImplementedException();
        }

        public Player Get(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public Player LogInPlayer(string login, string password)
        {
            return _Context.Players.FirstOrDefault(o => o.Login == login && o.Password == password);
        }
    }
}
