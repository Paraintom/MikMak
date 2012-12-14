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
            return Get(login);
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
            if (login == "tom")
            {
                return 10;
            }
            if (login == "oliv")
            {
                return 20;
            }
            int h = 0;
            for (int i = 0; i < login.Length; i++)
                h += login[i] * 31 ^ login.Length - (i + 1);
            return h;
        }
    }

}
