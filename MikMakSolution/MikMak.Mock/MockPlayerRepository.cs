using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;

namespace MikMak.Mock
{
    public class MockPlayerRepository : IPlayerRepository
    {
        public Player CreatePlayer(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Player Get(string login)
        {
            return new Player()
            {
                Login = login,
                Password = login,
                PlayerId = GetPlayerId(login)
            };
        }

        public static int GetPlayerId(string login)
        {
            return login.Length + (int)login[1];
        }
    }
}
