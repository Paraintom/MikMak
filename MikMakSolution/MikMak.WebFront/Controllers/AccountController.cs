using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MikMak.Infrastructure.InjectionDependancy;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;

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

            IPlayerRepository playerRepo = ServiceLocator.GetInstance<IPlayerRepository>();
            Player player = playerRepo.LogInPlayer(username, password);
            if (player == null)
            {
                return View();
            }
            this.AutentificateUser(player);
           

            return RedirectToAction("Index","PlayerRoom",null);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index","Home",null);
        }

        public ActionResult NewAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewAccount(string username, string password)
        {
            IPlayerRepository playerRepo = ServiceLocator.GetInstance<IPlayerRepository>();
            Player player = playerRepo.CreatePlayer(username, password);
            this.AutentificateUser(player);
            return RedirectToAction("Index", "PlayerRoom", null);
        }


        private void AutentificateUser(Player player)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(player.PlayerId.ToString(), false, 30);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

    }
}
