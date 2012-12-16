using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MikMak.WebFront.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogIn()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            if (username != password)
            {
                return View();
            }
            // Open session for 30 min!
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket("Tom",false,30);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            return RedirectToAction("Index","PlayerRoom",null);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index","Home",null);
        }

    }
}
