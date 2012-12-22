// -----------------------------------------------------------------------
// <copyright file="PlayerInBattleRepository.cs" company="RFLEX Progiciel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MikMak.Repository.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MikMak.Repository.Interfaces;
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerInBattleRepository : IPlayerInBattleRepository
    {
        private Context _context;

        public PlayerInBattleRepository(IDbConnection connection)
        {
            DbConnection connec = connection as DbConnection;
            if (connec == null)
            {
                throw new Exception("Incorrect connection");
            }
            _context = new Context(connec);
        }

        public void CreateLink(string battleId, List<int> players)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Entities.PlayerInBattle> Get(int playerId)
        {
            throw new NotImplementedException();
        }

        public DomainModel.Entities.PlayerInBattle Get(string battleId, int playerId)
        {
            throw new NotImplementedException();
        }

        public void Persist(DomainModel.Entities.PlayerInBattle playersInBattle)
        {
            throw new NotImplementedException();
        }
    }
}
