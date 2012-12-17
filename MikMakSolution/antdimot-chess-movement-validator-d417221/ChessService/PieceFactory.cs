using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Model;
using Chess.Model.Pieces;

namespace ChessService
{
    class PieceFactory
    {
        public static Piece GetType(string s, ChessColor color)
        {
            Piece p;
            switch (s)
            {
                case "Bishop":
                    p = new Bishop(color);
                    break;
                case "King":
                    p = new King(color);
                    break;
                case "Knight":
                    p = new Knight(color);
                    break;
                case "Pawn":
                    p = new Pawn(color);
                    break;
                case "Queen":
                    p = new Queen(color);
                    break;
                case "Rook":
                    p = new Rook(color);
                    break;
                default:
                    throw new Exception("Piece unknown : " + s);
            }
            return p;
        }
    }
}
