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
        private Context _context;

        public PlayerRepository(IDbConnection connection)
        {
            DbConnection connec = connection as DbConnection;
            if (connec == null)
            {
                throw new Exception("Incorrect connection");
            }
            _context = new Context(connec);
        }

        public Player CreatePlayer(string login, string password)
        {
            if (_context.Players.Any(g => g.Login == login))
            {
                throw new Exception(Error.LOGIN_ALREADY_EXISTS);
            }
            Player newPlayer = new Player
            {
                Login = login,
                Password = password,
            };
            _context.Players.Add(newPlayer);
            _context.SaveChanges();
            return newPlayer;
        }

        public Player Get(string login)
        {
            throw new System.NotImplementedException();
        }

        public Player Get(int playerId)
        {
            return _context.Players.FirstOrDefault(o => o.PlayerId == playerId);
        }

        //public Player LogInPlayer(string login, string password)
        //{           
        //    return _context.Players.FirstOrDefault(o => o.Login == login && o.Password == password);
        //}



    }
}
