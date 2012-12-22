using System;
using MikMak.Infrastructure.Ressource;
using MikMak.Repository.Interfaces;

namespace MikMak.Repository.EntityFramework
{
    using System.Linq;

    using MikMak.DomainModel.Entities;
    using System.Data;
    using System.Data.Common;

    public class BattleRepository //: IBattleRepository
    {
        private Context _context;

        public BattleRepository(IDbConnection connection)
        {
            DbConnection connec = connection as DbConnection;
            if (connec == null)
            {
                throw new Exception("Incorrect connection");
            }
            _context = new Context(connec);
        }

        public string CreateBattle(Battle battle)
        {
           Battle Newbattle = _context.Battles.Add(battle);
           return string.Empty;
        }


        public Battle Get(int battleId)
        {
            return _context.Battles.FirstOrDefault(o => o.BattleId == battleId);
        }

        public void Update(Battle battle)
        {
            _context.Battles.Attach(battle);
            _context.SaveChanges();
        }
    }
}
