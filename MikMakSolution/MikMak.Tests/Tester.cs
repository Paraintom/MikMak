using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DAO;
using MikMak.Repository.Interfaces;
using System.Diagnostics;
using MikMak.DomainModel.Entities;

namespace MikMak.Tests
{
    class Tester : IDisposable
    {
        IPlayerRepository playerDao;
        IPlayerInBattleRepository playerInBattleDao;

        public Tester()
        {
            playerDao = PlayerDao.GetInstance();
            playerInBattleDao = PlayerInBattleDao.GetInstance();
        }

        //Integration test for DAO
        public static void Main()
        {
            //PlayerDao.ConnectionString = MyConfiguration.GetString("MikMakConnection", @"Data Source=|DataDirectory|\LocalDb.sdf");
            Console.WriteLine("Test starting...");
            try
            {
                using (Tester test = new Tester())
                {
                    //Player Tests
                    string login1 = DateTime.Now.ToString("HH:mm:ss");
                    string login2 = DateTime.Now.ToString("HH:mm:ss") + "bis";

                    Debug.Assert(test.playerDao.Get(91233) ==null);
                    Debug.Assert(test.playerDao.Get(login1 + "ee") == null);
                    Debug.Assert(test.playerDao.Get(login1 )== null);

                    test.playerDao.CreatePlayer(login1, login1);
                    Debug.Assert(test.playerDao.Get(login1) != null);
                    Debug.Assert(test.playerDao.Get(login2) == null);
                    test.playerDao.CreatePlayer(login2, login2);
                    var player1 = test.playerDao.Get(login1);
                    var player2 = test.playerDao.Get(login2);
                    Debug.Assert(player2 != null);
                    Debug.Assert(test.playerDao.Get(login2).Login == login2);
                    Debug.Assert(test.playerDao.Get(login2).Password == login2);
                    Debug.Assert(test.playerDao.Get(login2).PlayerId != test.playerDao.Get(login1).PlayerId);

                    //For fun, tom should exist!
                    if (test.playerDao.Get("tom") == null)
                    {
                        test.playerDao.CreatePlayer("tom", "tom");
                    }

                    //No battles yet
                    Debug.Assert(test.playerInBattleDao.Get(test.playerDao.Get(login1).PlayerId).Count == 0);
                    Debug.Assert(test.playerInBattleDao.Get(test.playerDao.Get(login2).PlayerId).Count == 0);

                    //CreationBattle
                    PlayerInBattle newBattle = new PlayerInBattle()
                    {
                        Player = player1,
                        PlayerNumber = 1,
                        Battle = new Battle()
                        {
                            BattleId = Math.Abs((int)DateTime.Now.Ticks),
                            CurrentState = new Grid()
                            {
                                CurrentMessage = Message.GetMessage(ClassicMessage.Default),
                                IsGridShifted = true,
                                MoveNumber = 5,
                                NextPlayerToPlay = 1,
                                NumberColumns = 2,
                                NumberLines = 4,
                                PawnLocations = new List<Pawn>()
                                {
                                    new Pawn("p1",1,2){
                                        Player = player1
                                    },
                                    new Pawn("p2",4,2){
                                        Player = player2
                                    }
                                }
                            },
                            GameType = 9,
                            GameTypeString = "testToTrash",
                            Players = new List<int>()
                            {
                                player1.PlayerId,
                                player2.PlayerId
                            }
                        }
                    };
                    test.playerInBattleDao.Persist(newBattle);

                    //Both players should see the battle
                    Debug.Assert(test.playerInBattleDao.Get(test.playerDao.Get(login1).PlayerId).Count == 1);
                    Debug.Assert(test.playerInBattleDao.Get(test.playerDao.Get(login2).PlayerId).Count == 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e);
            }
            Console.WriteLine("End of the test");
        }

        public void Dispose()
        {
            Dispose((PlayerDao)playerDao);
            Dispose((PlayerInBattleDao)playerInBattleDao);
        }

        private static void Dispose(IDisposable toDispose)
        {
            if (toDispose != null)
                toDispose.Dispose();
        }
    }
}
