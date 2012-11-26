using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Interfaces;
using MikMak.Commons;

namespace MikMak.Main.GamesManagement
{
    public class GameManager : IGamesManager
    {
        public string GetNewGame(Session initialSession, int gameType, string opponent)
        {
            throw new NotImplementedException();
        }

        public GridState GetState(Session sessionId)
        {
            throw new NotImplementedException();
        }

        public GridState Play(Session sessionId, Move move)
        {
            throw new NotImplementedException();
        }
    }
}
