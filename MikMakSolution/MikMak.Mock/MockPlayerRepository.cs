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
        private static Dictionary<int, Player> allPlayers = new Dictionary<int, Player>();

        private static MockPlayerRepository Instance;
        public static MockPlayerRepository GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MockPlayerRepository();
            }
            return Instance;
        }

        public Player CreatePlayer(string login, string password)
        {
            return Get(login);
        }

        public Player Get(string login)
        {
            var p = new Player()
            {
                Login = login,
                Password = login,
                PlayerId = GetPlayerId(login)
            };
            if (!allPlayers.ContainsKey(p.PlayerId))
            {
                allPlayers.Add(p.PlayerId, p);
            }
            return p;
        }

        public Player Get(int id)
        { 
            Player p;
            if (!allPlayers.TryGetValue(id, out p))
            {
                throw new Exception("player unknown!! id = "+id+" list size "+allPlayers.Count);
            }
            return p;
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

        #region IPlayerRepository Membres


        public Player LogInPlayer(string login, string password)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
