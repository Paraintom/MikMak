using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Player CreatePlayer(string login, string password);

        Player Get(string login);

        Player Get(int playerId);

        //Player LogInPlayer(string login, string password);
    }
}
