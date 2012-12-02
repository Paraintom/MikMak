using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        string CreateGamer(Player gamer);
        Player Get(string login);
    }
}
