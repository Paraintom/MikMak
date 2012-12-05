using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MikMak.DomainModel.Entities;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class GridExtented
    {
        public string SessionId { get; set; }
        public Grid State { get; set; }
    }
}