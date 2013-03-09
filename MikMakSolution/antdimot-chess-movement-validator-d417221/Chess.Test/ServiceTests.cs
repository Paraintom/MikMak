using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.Model;
using Chess.Model.Pieces;
using ChessService;
using MikMak.DomainModel.Entities;
using Pawn = MikMak.DomainModel.Entities.Pawn;

namespace Chess.Test
{
    [TestClass]
    public class ServiceTests
    {
        ChessManager LocalChessManager;
        Grid CurrentGid;

        [TestMethod]
        public void PawnPromoted_GoodWorkflow_Success()
        {
            LocalChessManager = new ChessManager();
            CurrentGid = LocalChessManager.GetNewGame();
            int nextPlayer;
            nextPlayer = Play(1, 1, 2, 1, 3).NextPlayerToPlay;
            nextPlayer = Play(2, 1, 7, 1, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 2, 1, 3, 3).NextPlayerToPlay;
            nextPlayer = Play(2, 1, 5, 1, 4).NextPlayerToPlay;
            nextPlayer = Play(1, 3, 3, 1, 4).NextPlayerToPlay;
            nextPlayer = Play(2, 1, 8, 1, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 1, 4, 3, 3).NextPlayerToPlay;
            nextPlayer = Play(2, 1, 5, 2, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 1, 3, 1, 4).NextPlayerToPlay;
            nextPlayer = Play(2, 2, 5, 3, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 1, 4, 1, 5).NextPlayerToPlay;
            nextPlayer = Play(2, 3, 5, 2, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 1, 5, 1, 6).NextPlayerToPlay;
            nextPlayer = Play(2, 2, 5, 3, 5).NextPlayerToPlay;
            nextPlayer = Play(1, 1, 6, 1, 7).NextPlayerToPlay;
            nextPlayer = Play(2, 3, 5, 2, 5).NextPlayerToPlay;
            var grid = Play(1, 1, 7, 1, 8);
            Assert.AreEqual(9, grid.NumberLines);
            var extraPawn = grid.PawnLocations.Where(p => p.Coord.y == 9);
            Assert.AreEqual(4, extraPawn.Count());
            Assert.AreEqual(1, grid.MoveNumber);
            Assert.AreEqual(1, grid.NextPlayerToPlay);
            //Choice of taking ... A queen!
            Grid ToReturn = LocalChessManager.Play(CurrentGid, new Move() { PlayerNumber = 1, Positions = new List<Pawn> { new Pawn("", 1, 9)} });
            CurrentGid = ToReturn;

            nextPlayer = Play(2, 2, 5, 3, 5).NextPlayerToPlay;
            Assert.AreEqual(1, grid.NextPlayerToPlay);
            grid = Play(1, 1, 8, 1, 2);
            Assert.AreEqual(2, grid.NextPlayerToPlay);
            var newQueen = grid.PawnLocations.Where(p => p.Coord.x == 1 && p.Coord.y == 2).FirstOrDefault();
            Assert.IsNotNull(newQueen);

        }

        public Grid Play(int playerNumber, int x, int y, int x2, int y2)
        {
            Grid ToReturn = LocalChessManager.Play(CurrentGid, new Move() { PlayerNumber = playerNumber, Positions = new List<Pawn> { new Pawn("", x, y), new Pawn("", x2, y2) } });
            CurrentGid = ToReturn;
            return ToReturn;
        }
    }
}
