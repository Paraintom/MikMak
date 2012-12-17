using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MikMak.Interfaces;
using MikMak.DomainModel.Entities;
using Chess.Model;

namespace ChessService
{
    public class ChessManager : IGameServices
    {
        public int GetGameType()
        {
            return 2;
        }

        public Grid GetNewGame()
        {            
            Board b = new Board(true);
            Grid toReturn = GetGridFromBoard(b);
            return toReturn;
        }


        public Grid Play(Grid currentState, Move move)
        {
            //0-Game already finished, no more move
            if (currentState.CurrentMessage.Id == (int)ClassicMessage.GameFinished)
            {
                return currentState;
            }   

            //0-Not Your turn
            if (move.PlayerNumber != currentState.NextPlayerToPlay)
            {
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.NotYourTurn);
                return currentState;
            }

            if (move.Positions.Count != 2)
            {
                currentState.CurrentMessage = Message.GetMessage(ChessMessage.NotEnoughMove);
                return currentState;
            }

            int x_init = move.Positions[0].Coord.x;
            int y_init = move.Positions[0].Coord.y;
            int x_end = move.Positions[1].Coord.x;
            int y_end = move.Positions[1].Coord.y;
            Board b = GenerateBoardFromGrid(currentState);
            var result = b.MovePiece(x_init, y_init, x_end, y_end);
            var newState = GetGridFromBoard(b);
            if (result.IsSuccess)
            {
                newState.CurrentMessage = new Message()
                {
                    Id = 30,
                    Information = result.Description
                };
            }
            else
            {
                newState.CurrentMessage = Message.GetMessage(ChessMessage.InvalidMove);
            }
            return newState;
        }



        #region Helpers

        private Pawn GetPawn(Piece p, int i, int j)
        {
            return new Pawn(((int)p.ChessColor) + p.GetType().Name, i + 1, j + 1);
        }
        private Piece GetPiece(Pawn pawn)
        {
            ChessColor color = pawn.Name[0] == '0' ? ChessColor.White : ChessColor.Black;
            string piece = pawn.Name.Substring(1);
            return PieceFactory.GetType(piece,color);
        }
        private Board GenerateBoardFromGrid(Grid currentState)
        {
            Board b = new Board(false);
            foreach (Pawn pawn in currentState.PawnLocations)
            {
                var p = GetPiece(pawn);
                b.putPiece(p, Board.Columns_Inv[pawn.Coord.x], pawn.Coord.y);
            }
            return b;
        }
        private Grid GetGridFromBoard(Board b)
        {
            Grid toReturn = new Grid()
            {
                CurrentMessage = Message.GetMessage(ChessMessage.NewGame),
                IsGridShifted = false,
                MoveNumber = 2,
                NextPlayerToPlay = 1,
                NumberColumns = 8,
                NumberLines = 8
            };

            toReturn.PawnLocations = GetAllPieces(b);
            return toReturn;
        }
        private List<Pawn> GetAllPieces(Board b)
        {
            List<Pawn> toReturn = new List<Pawn>();
            Piece[,] board = b.AllPieces();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                    {
                        Piece p = board[i, j];
                        Pawn toAdd = GetPawn(p, j, i);
                        toReturn.Add(toAdd);
                    }
                }
            }
            return toReturn;
        }
        #endregion
        
        public enum ChessMessage
        {
            [Description("New game, joueur 1 turn")]
            NewGame = 200,
            [Description("Joueur 1 turn")]
            J1 = 201,
            [Description("Joueur 2 turn")]
            J2 = 202,
            [Description("Not enough moves (2 required)")]
            NotEnoughMove = 203,
            [Description("Invalid Move")]
            InvalidMove = 204,
        }
    }
}
