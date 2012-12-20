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
    public class BaseMovementTests
    {
        [TestMethod]
        public void MovePiece_ToBusyPostionWithSameColor_Error()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<Pawn>( ChessColor.White, 'A', 5 );
            board.SetPiece<Pawn>( ChessColor.White, 'A', 6 );

            var result = board.MovePiece( 'A', 5, 'A', 6 );

            Assert.IsFalse( result.IsSuccess, result.Description );
        }

        [TestMethod]
        public void MovePiece_MakeKingChess_Error()
        {
            var board = Board.GetNewBoard();

            board.SetPiece<King>(ChessColor.White, 'A', 1);
            board.SetPiece<Pawn>(ChessColor.White, 'B', 2);
            board.SetPiece<Queen>(ChessColor.Black, 'D', 4);

            var result = board.MovePiece('B', 2, 'B', 3);

            Assert.IsFalse(result.IsSuccess, result.Description);
        }
    }
}
