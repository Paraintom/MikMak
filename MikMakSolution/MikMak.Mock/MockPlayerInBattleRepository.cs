using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;

namespace MikMak.Mock
{
    public class MockPlayerInBattleRepository : IPlayerInBattleRepository
    {
        private static Dictionary<Tuple<int, string>, PlayerInBattle> datas = new Dictionary<Tuple<int, string>, PlayerInBattle>();

        public void Persist(PlayerInBattle playersInBattle)
        {
            int i = 1;
            foreach (int playerId in playersInBattle.Battle.Players)
            {
                playersInBattle = new PlayerInBattle()
                {
                    Battle = playersInBattle.Battle,
                    Player = MockPlayerRepository.GetInstance().Get(playerId),
                    PlayerNumber = i
                };
                
                Tuple<int, string> toPersist = new Tuple<int, string>(playerId, playersInBattle.Battle.GameId);
                i++;
                datas[toPersist] = playersInBattle;                
            }
        }

        public List<PlayerInBattle> Get(int playerId)
        {
            List<PlayerInBattle> toReturn = new List<PlayerInBattle>();
            foreach (var item in datas)
            {
                if (item.Key.Item1 == playerId)
                {
                    toReturn.Add(item.Value);
                }
            }
            return toReturn;
        }

        public PlayerInBattle Get(string battleId, int playerId)
        {
            foreach (var item in datas)
            {
                if (item.Key.Item1 == playerId && item.Key.Item2 == battleId)
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
}
