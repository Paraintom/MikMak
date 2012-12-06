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
        public void Persist(PlayerInBattle playersInBattle)
        {
            //Done :-)
        }

        public List<PlayerInBattle> Get(int playerId)
        {
            return new List<PlayerInBattle>(){
                new PlayerInBattle(){
                    Battle = new Battle(){
                        GameId = "Id1",
                        GameTypeString = "MockForTest"
                    },
                    Player = new Player(){
                        PlayerId= playerId
                    }                    
                },
                new PlayerInBattle(){
                    Battle = new Battle(){
                        GameId = "Id2",
                        GameTypeString = "MockForTest"
                    },
                    Player = new Player(){
                        PlayerId= playerId
                    }                    
                }
            };
        }

        public PlayerInBattle Get(string battleId, int playerId)
        {
            return new PlayerInBattle()
            {
                Battle = new Battle()
                {
                    GameId = battleId,
                    GameType = 1,
                    GameTypeString = "MockTest",
                    Players = new List<int>(){
                        10,
                        20
                    },
                    CurrentState = new Grid()
                    {
                        CurrentMessage = Message.GetMessage(ClassicMessage.YourTurn),
                        IsGridShifted = false,
                        MoveNumber = 1,
                        NumberColumns = 3,
                        NumberLines = 3,
                        NextPlayerToPlay = 1,
                        PawnLocations = new List<Pawn>()
                        {
                            new Pawn('B', 1,1),
                            new Pawn('W', 2,2),
                            new Pawn('B', 3,3),
                            new Pawn('B', 2,3),
                            new Pawn('W', 3,1),
                        }
                    }
                },
                Player = new Player()
                {
                    PlayerId = playerId,
                    Login = "tom",
                    Password = "tom"
                },
                PlayerNumber = playerId ==20 ? 2 : 1
            };
        }
    }
}
