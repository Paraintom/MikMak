using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MikMak.DomainModel.Entities;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class GridExtented
    {
        public string sessionId { get; set; }
        public Grid state { get; set; }
    }
}