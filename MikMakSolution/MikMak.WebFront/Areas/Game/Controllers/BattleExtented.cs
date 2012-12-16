using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MikMak.DomainModel.Entities;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class BattleExtented
    {
        public List<Player> InGame { get; set; }
        public Battle Battle { get; set; }
    }
}