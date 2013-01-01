using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.Model;
using Chess.Model.Pieces;

namespace Chess.Test
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void MovePiece_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>( ChessColor.White, 'A', 1 );

            var result = board.MovePiece( 'A', 1, 'A', 2 );

            Assert.IsTrue( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_SmallRock_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'E', 1);
            board.SetPiece<Rook>(ChessColor.White, 'H', 1);

            board.SetPiece<Pawn>(ChessColor.White, 'E', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'F', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'G', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'H', 2);

            Assert.IsNull(board.GetPiece('F', 1), "Nothing Here!");
            board.SetPiece<Bishop>(ChessColor.Black, 'A', 6);

            var result = board.MovePiece('E', 1, 'G', 1);
            Assert.IsTrue(result.IsSuccess, "it is a small rock");
            Assert.IsNull(board.GetPiece('H', 1), "The rook has to be moved!");
            Assert.IsNotNull(board.GetPiece('F', 1), "Here!");
        }

        [TestMethod]
        public void MovePiece_BigRock_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'E', 1);
            board.SetPiece<Rook>(ChessColor.White, 'A', 1);

            board.SetPiece<Pawn>(ChessColor.White, 'E', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'F', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'G', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'H', 2);
            Assert.IsNull(board.GetPiece('D', 1), "Nothing Here!");

            board.SetPiece<Bishop>(ChessColor.Black, 'A', 6);

            var result = board.MovePiece('E', 1, 'C', 1);
            Assert.IsTrue(result.IsSuccess, "it is a big rock");
            Assert.IsNull(board.GetPiece('A', 1), "The rook has to be moved!");
            Assert.IsNotNull(board.GetPiece('D', 1), "Here!");
        }

        [TestMethod]
        public void MovePiece_KingAlreadyMoved_Failed()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'E', 1);
            board.SetPiece<Rook>(ChessColor.White, 'A', 1);

            board.SetPiece<Pawn>(ChessColor.White, 'E', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'F', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'G', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'H', 2);

            board.SetPiece<Bishop>(ChessColor.Black, 'A', 6);

            var result = board.MovePiece('E', 1, 'F', 1);
            result = board.MovePiece('F', 1, 'E', 1);
            result = board.MovePiece('E', 1, 'C', 1);
            Assert.IsFalse(result.IsSuccess, "The king has moved before");
        }

        [TestMethod]
        public void MovePiece_RookAlreadyMoved_Failed()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'E', 1);
            board.SetPiece<Rook>(ChessColor.White, 'A', 1);

            board.SetPiece<Pawn>(ChessColor.White, 'E', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'F', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'G', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'H', 2);

            board.SetPiece<Bishop>(ChessColor.Black, 'A', 6);

            var result = board.MovePiece('A', 1, 'A', 2);
            result = board.MovePiece('A', 2, 'A', 1);
            result = board.MovePiece('E', 1, 'C', 1);
            Assert.IsFalse(result.IsSuccess, "The rook has moved before");
        }

        [TestMethod]
        public void MovePiece_KingChess_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'E', 1);
            board.SetPiece<Rook>(ChessColor.White, 'H', 1);

            board.SetPiece<Pawn>(ChessColor.White, 'F', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'G', 2);
            board.SetPiece<Pawn>(ChessColor.White, 'H', 2);

            board.SetPiece<Bishop>(ChessColor.Black, 'A', 6);

            var result = board.MovePiece('E', 1, 'G', 1);
            Assert.IsFalse(result.IsSuccess, "The king pass by a case in chess");
        }
    }
}
