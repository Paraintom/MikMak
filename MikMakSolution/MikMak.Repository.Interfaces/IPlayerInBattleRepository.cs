using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;

namespace MikMak.Repository.Interfaces
{
    public interface IPlayerInBattleRepository
    {
        //void CreateLink(string battleId, List<int> players);
        void Persist(PlayerInBattle playersInBattle);

        List<PlayerInBattle> Get(int playerId);

        PlayerInBattle Get(string battleId, int playerId);
    }
}
