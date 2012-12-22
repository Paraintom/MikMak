namespace MikMak.WebFront.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
using MikMak.DomainModel.Entities;
    using MikMak.Repository.Interfaces;
    using MikMak.Infrastructure.InjectionDependancy;

    public static class Context
    {      
        public static Player User
        {
            get
            {
                Player playerContext;
                if (HttpContext.Current.Cache["user"] == null)
                {
                    IPlayerRepository playerRepo = ServiceLocator.GetInstance<IPlayerRepository>();
                    playerContext = playerRepo.Get(UserContextID);
                    HttpContext.Current.Cache["user"] = playerContext;
                }
                else
                {
                    playerContext = (Player)HttpContext.Current.Cache["user"];
                }

                return playerContext;
            }
        }

        private static int UserContextID
        {
            get
            {
                return int.Parse(HttpContext.Current.User.Identity.Name);
            }
        }

    }
}