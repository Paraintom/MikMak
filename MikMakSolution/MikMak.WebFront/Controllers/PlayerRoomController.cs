using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MikMak.WebFront.Common;

namespace MikMak.WebFront.Controllers
{
    [Authorize]
    public class PlayerRoomController : Controller
    {
        //
        // GET: /PlayerRoom/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Games()
        {
            return View();
        }

    }
}
