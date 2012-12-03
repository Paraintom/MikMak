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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerInBattleRepository : IPlayerInBattleRepository
    {
        public void CreateLink(string battleId, List<int> players)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Entities.PlayerInBattle> Get(string battleId)
        {
            throw new NotImplementedException();
        }

        public DomainModel.Entities.PlayerInBattle Get(string battleId, int playerId)
        {
            throw new NotImplementedException();
        }
    }
}
