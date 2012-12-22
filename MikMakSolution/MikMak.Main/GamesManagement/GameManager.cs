using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.DomainModel.Entities;
using MikMak.Main.InternalInterfaces;
using MikMak.Interfaces;
using MikMak.Repository.Interfaces;
using MikMak.Mock;

namespace MikMak.Main.GamesManagement
{
    public class GameManager : IGamesManager
    {
        private ITypeGameMapping typeMapping;
        private IPlayerInBattleRepository repoPlayerInBattle;
        private IPlayerRepository repoPlayer;
        private IBattleRepository repoBattle;
        private Random ran;
        private static GameManager instance = new GameManager();

        private GameManager()
        {
            // TODO Logs. (UNITY)
            repoPlayer = new MockPlayerRepository();
            repoPlayerInBattle = new MockPlayerInBattleRepository();
            typeMapping = new TypeGameMappingByReflection();
            ran = new Random();
        }

        public static GameManager GetInstance()
        {
            return instance;
        }

        public Grid Play(PlayerInBattle playerInBattle, Move move)
        {
            // 1- We play the move
            var currentBattle = playerInBattle.Battle;
            var game = typeMapping.GetGame(currentBattle.GameType);
            var newState = game.Play(currentBattle.CurrentState, move);

            // 2- If the grid has change, we persit the new state
            var oldState = currentBattle.CurrentState;
            currentBattle.CurrentState = newState;
            if (newState.DeservePersistence(oldState))
            {
                //See what we want to do....
                //repoBattle.Update(playerInBattle.Battle);
            }

            // 3- We return the new state to the client
            return newState;
        }

        public PlayerInBattle GetNewGame(Player firstPlayer, int gameType, List<Player> opponents)
        {
            // 1-Create The game
            var game = typeMapping.GetGame(gameType);
            Grid currentState = game.GetNewGame();

            // 2-Preparing listOfPlayers
            var listPlayers = opponents; //.Select(p=>p.PlayerId).ToList();
            listPlayers.Insert(0, firstPlayer);
            
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
                    Players = listPlayers,
                    CurrentState = currentState
                },
                Player = firstPlayer,
                PlayerNumber = 1
            };

            repoPlayerInBattle.Persist(toReturn);
            return toReturn;
        }

        public List<PlayerInBattle> GetAllBattles(Player player)
        {
            return repoPlayerInBattle.Get(player.PlayerId);
        }

        public PlayerInBattle GetParticipation(Player player, string gameId)
        {
            return repoPlayerInBattle.Get(gameId, player.PlayerId);
        }

        private string GetElapsedSecondsSinceLastNewYear()
        {
            DateTime centuryBegin = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentDate = DateTime.Now;

            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            return elapsedSpan.TotalSeconds.ToString();
        }
    }
}
