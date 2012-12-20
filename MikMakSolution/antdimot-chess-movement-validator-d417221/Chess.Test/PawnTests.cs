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
    public class PawnTests
    {
        [TestMethod]
        public void MovePiece_TwoStepFromStartPosition_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'A', 2 );

            var result = board.MovePiece( 'A', 2, 'A', 4 );

            Assert.IsTrue( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_OneStepFromStartPosition_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'A', 2 );

            var result = board.MovePiece( 'A', 2, 'A', 3 );

            Assert.IsTrue( result.IsSuccess, result.Description );

            var oldPosition = board.GetPiece( 'A', 2 );

            Assert.IsNull( oldPosition, "There is still a piece at old position" );

            var newPosition = board.GetPiece( 'A', 3 );

            Assert.IsNotNull( newPosition, "There is not a piece at new position" );

            Assert.IsInstanceOfType( newPosition, typeof( Pawn ), "The piece at new position is different." );

            Assert.IsTrue( newPosition.ChessColor == ChessColor.White, "The piece at new position is different." );
        }

        [TestMethod]
        public void MovePiece_WihEatFromAnyPosition_Success()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'B', 4 );

            board.SetPiece<Pawn>( ChessColor.Black, 'C', 5 );

            var result = board.MovePiece( 'B', 4, 'C', 5 );

            Assert.IsTrue( result.IsSuccess, result.Description );

            Assert.IsTrue( result.Ate, "Ate property was not set correctly" );

            Assert.IsNotNull( result.AtePiece, "AtePiece property was not set correctly" );

            Assert.IsInstanceOfType( result.AtePiece, typeof(Pawn), "AtePiece property was not set correctly" );
        }

        [TestMethod]
        public void MovePiece_WithEatFromAnyPosition_Error()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'B', 4 );

            board.SetPiece<Pawn>( ChessColor.White, 'C', 5 );

            var result = board.MovePiece( 'B', 4, 'C', 5 );

            Assert.IsFalse( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_ThreeStepFromStartPosition_Error()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'A', 2 );

            var result = board.MovePiece( 'A', 2, 'A', 5 );

            Assert.IsFalse( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_OneStepBackFromAnyPosition_Error()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'A', 5 );

            var result = board.MovePiece( 'A', 5, 'A', 4 );

            Assert.IsFalse( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_WihoutEatSetEnPassant_Success()
        {
            var board = Board.GetNewBoard();
            Assert.IsNull(board.EnPassant, "Initialisation should not start with en passant position");

            board.SetPiece<Pawn>(ChessColor.White, 'A', 2);
            board.SetPiece<Pawn>(ChessColor.Black, 'B', 4);

            var result = board.MovePiece('A', 2, 'A', 4);
            Assert.IsNotNull(board.EnPassant, "Should detect en passant posibility");
            Assert.AreEqual('A', board.EnPassant.Item1);
            Assert.AreEqual(3, board.EnPassant.Item2);

            result = board.MovePiece('B', 4, 'B', 5);
            Assert.IsNotNull(board.EnPassant, "Bad move should not erase the en passant info");

            result = board.MovePiece('B', 4, 'B', 3);
            Assert.IsNull(board.EnPassant, "En passant position finished");
            Assert.IsTrue(result.IsSuccess, result.Description);
        }

        [TestMethod]
        public void MovePiece_WihEatEnPassant_Success()
        {
            var board = Board.GetNewBoard();
            Assert.IsNull(board.EnPassant, "Initialisation should not start with en passant position");

            board.EnPassant = new Tuple<char, int>('A', 3);
            board.SetPiece<Pawn>(ChessColor.White, 'A', 4);
            board.SetPiece<Pawn>(ChessColor.Black, 'B', 4);

            var result = board.MovePiece('B', 4, 'A', 3);
            Assert.IsTrue(result.IsSuccess, result.Description);
            Assert.IsTrue(result.Ate, "has eaten en passant!");

            Assert.IsNotNull(result.AtePiece, "AtePiece property was not set correctly");
            Assert.IsInstanceOfType(result.AtePiece, typeof(Pawn), "AtePiece property was not set correctly");
            Assert.IsNull(board.EnPassant, "Good move should  erase the en passant info");
        }
    }
}
