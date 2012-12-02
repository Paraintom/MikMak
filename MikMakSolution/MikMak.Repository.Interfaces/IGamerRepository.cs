using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Game.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IGamerRepository
    {
         string CreateGamer(Gamer gamer);
    }
}
