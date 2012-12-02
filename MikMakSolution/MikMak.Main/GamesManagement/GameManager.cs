using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;
using MikMak.Main.InternalInterfaces;
using MikMak.Interfaces;

namespace MikMak.Main.GamesManagement
{
    public class GameManager : IGamesManager
    {
        private ITypeGameMapping typeMapping;
        private Random ran;

        public GameManager()
        {
            // TODO Logs. (UNITY)
            typeMapping = new TypeGameMappingByReflection();
            ran = new Random();
        }

        public Grid Play(PlayerInBattle playerInBattle, Move move)
        {
            var game = typeMapping.GetGame(playerInBattle.Battle.GameType);
            return game.Play(playerInBattle.Battle.GameId, move);
        }

        public PlayerInBattle GetNewGame(Player firstPlayer, int gameType, List<Player> opponents)
        {
            // 1-Create The game
            var game = typeMapping.GetGame(gameType);
            string gameId = game.GetNewGame();

            // 2-Preparing listOfPlayers
            var listPlayers = opponents.Select(p=>p.PlayerId).ToList();
            listPlayers.Insert(0, firstPlayer.PlayerId);
            
            // 2-Link the players Id with player numbers
            PlayerInBattle toReturn = new PlayerInBattle()
            {
                Battle = new Battle
                {
                    CreationTime = DateTime.Now,
                    GameId = String.Format("{0}_{1}_{2}_{3}", game.GetGameType(), firstPlayer.PlayerId, GetElapsedSecondsSinceLastNewYear(), ran.Next(99)),
                    GameType = gameType,
                    GameTypeString = game.ToString(),
                    LastUpdate = DateTime.Now,
                    Players = listPlayers
                },
                Player = firstPlayer,
                PlayerNumber = 1
            };
            return toReturn;
        }

        private string GetElapsedSecondsSinceLastNewYear()
        {
            DateTime centuryBegin = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentDate = DateTime.Now;

            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            return elapsedSpan.TotalSeconds.ToString();
        }

        public Grid GetState(Battle battle)
        {
            var game = typeMapping.GetGame(battle.GameType);
            return game.GetState(battle.GameId);
        }
    }
}
