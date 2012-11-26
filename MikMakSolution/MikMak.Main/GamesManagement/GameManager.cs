using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Interfaces;
using MikMak.Commons;
using MikMak.Main.InternalInterfaces;

namespace MikMak.Main.GamesManagement
{
    public class GameManager : IGamesManager
    {
        private int defaultPlayerNumber = 1;
        private ITypeGameMapping typeMapping;
        private ISessionManager sessionManager;

        public GameManager()
        {
            // TODO Initialize the field typeMapping and sessionManager.
        }

        public string GetNewGame(Session initialSession, int gameType, string opponent)
        {
            var game = typeMapping.GetGame(gameType);
            string gameId = game.GetNewGame();
            Session newSession = sessionManager.CreateSession(initialSession, gameId, gameType, defaultPlayerNumber);
            return newSession.Id;
        }

        public GridState GetState(Session session)
        {
            var game = typeMapping.GetGame(session.GameType);
            return game.GetState(session.GameId);
        }

        public GridState Play(Session session, Move move)
        {
            var game = typeMapping.GetGame(session.GameType);
            return game.Play(session.GameId, move);
        }
    }
}
