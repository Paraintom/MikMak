namespace MikMak.Repository.EntityFramework
{
    using System.Data.Common;
    using System.Data.Entity;
    using MikMak.DomainModel.Entities;
    using System.Data;
    using System;

    public class Context: DbContext
    {

        public DbSet<Player> Players { get; set; }
        public DbSet<Battle> Battles { get; set; }

        public Context(DbConnection connection): base(connection,false){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Battle>()
            //                    .HasMany(p=> p.Players)
            //                    .WithMany(t =>t.Battles)
            //                    .Map(m =>
            //                        {
            //                            m.ToTable("PlayerInBattle");
            //                            m.MapLeftKey("BattleId");
            //                            m.MapRightKey("PlayerId");
            //                        });
        }

    }
}
