namespace MikMak.Repository.EntityFramework
{
    using System.Data.Common;
    using System.Data.Entity;
    using MikMak.DomainModel.Entities;

    public class Context: DbContext
    {

        public DbSet<Player> Players { get; set; }
        public DbSet<Battle> Battles { get; set; }

        public Context(DbConnection connection) : base(connection, false) { }
    }
}
