using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Interfaces;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;
using MikMak.Mock;

namespace MikMak.Main.PlayerManagement
{
    public class PlayerManager : IPlayerManager
    {
        private IPlayerRepository repoPlayer;

        public PlayerManager()
        {
            //Todo injecter repo!!
            repoPlayer = new MockPlayerRepository();
        }

        public Player GetNewPlayer(string login, string password)
        {
            var player = repoPlayer.CreatePlayer(login, password);
            return player;
        }

        public Player Get(string login)
        {
            var player = repoPlayer.Get(login);
            return player;
        }

        public Player Get(int playerId)
        {
            var player = repoPlayer.Get(playerId);
            return player;
        }
    }
}
