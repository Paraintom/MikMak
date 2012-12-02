namespace MikMak.Repository.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;
    using MikMak.Game.Entities;
    using System.Data.Common;

    public class Context: DbContext
    {

        public DbSet<Gamer> Gamers { get; set; }
        public DbSet<Game> Games { get; set; }

        public Context(DbConnection connection) : base(connection, false) { }
    }
}
