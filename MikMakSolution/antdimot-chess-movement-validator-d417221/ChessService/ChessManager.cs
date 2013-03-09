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
        public const string EnPassantSpecialName = "ep";
        public const string HaveAlreadyMovedSpecialName = "am";

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
            
            //Not enough information for doing a move!
            if (move.Positions.Count < 1)
            {
                currentState.CurrentMessage = Message.GetMessage(ChessMessage.NotEnoughMove);
                return currentState;
            }

            int x_init = move.Positions[0].Coord.x;
            int y_init = move.Positions[0].Coord.y;
            Board b = GenerateBoardFromGrid(currentState);

            //Choice for promoted pawn is done
            if (currentState.NumberLines == 9 && move.Positions.Count == 1)
            {
                b.SetPawnPromoted(x_init);
                var finalState = GetGridFromBoard(b);
                finalState.NextPlayerToPlay = (currentState.NextPlayerToPlay == 1) ? 2 : 1;
                finalState.LastMove = move;
                return finalState;                
            }

            //Not enough information for doing a move!
            if (move.Positions.Count != 2)
            {
                currentState.CurrentMessage = Message.GetMessage(ChessMessage.NotEnoughMove);
                return currentState;
            }

            int x_end = move.Positions[1].Coord.x;
            int y_end = move.Positions[1].Coord.y;
            Piece p = b.GetPiece(x_init, y_init);

            if (p == null)
            {
                currentState.CurrentMessage = Message.GetMessage(ChessMessage.NoPawnHere);
                return currentState;
            }

            if (!IsGoodColor(p.ChessColor, move.PlayerNumber))
            {
                currentState.CurrentMessage = Message.GetMessage(ChessMessage.NotYourPawn);
                return currentState;
            }


            var result = b.MovePiece(x_init, y_init, x_end, y_end);
            var newState = GetGridFromBoard(b);
            newState.NextPlayerToPlay = currentState.NextPlayerToPlay;
            if (result.IsSuccess)
            {
                if (newState.NumberLines == 8)
                {
                    newState.NextPlayerToPlay = (currentState.NextPlayerToPlay == 1) ? 2 : 1;
                    newState.LastMove = move;
                    newState.CurrentMessage = Message.GetMessage((newState.NextPlayerToPlay == 2) ? ChessMessage.J2 : ChessMessage.J1);
                }
                else
                {
                    //If a pawn is at his last position, we can change it by what we want.
                    //The 9th column is the choose.
                    newState.NextPlayerToPlay = currentState.NextPlayerToPlay;
                    newState.LastMove = move;
                    newState.CurrentMessage = currentState.CurrentMessage;
                }
            }
            else
            {
                newState.CurrentMessage = Message.GetMessage(ChessMessage.InvalidMove);
                newState.CurrentMessage.Information = result.Description;
            }
            return newState;
        }

        private bool IsGoodColor(ChessColor chessColor, int p)
        {
            switch (chessColor)
            {
                case ChessColor.White:
                    return (p == 1);
                case ChessColor.Black:
                    return (p == 2);
                default:
                    return false;
            }
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
            List<Tuple<char, int>> HaveAlreadyMoved = new List<Tuple<char, int>>();
            //0 Special Pawn
            foreach (Pawn pawn in currentState.PawnLocations)
            {
                if(pawn.Coord.x < 1 || pawn.Coord.y < 1){
                    //Special Pawn!
                    if (pawn.Name == EnPassantSpecialName)
                    {
                        b.EnPassant = new Tuple<char, int>(Board.Columns_Inv[Math.Abs(pawn.Coord.x)], Math.Abs(pawn.Coord.y));
                    }
                    else
                    {
                        if (pawn.Name == HaveAlreadyMovedSpecialName)
                        {
                            var hasAlreadyMoved = new Tuple<char, int>(Board.Columns_Inv[Math.Abs(pawn.Coord.x)], Math.Abs(pawn.Coord.y));
                            HaveAlreadyMoved.Add(hasAlreadyMoved);
                        }
                        else
                        {
                            throw new Exception(String.Format("Piece Unknown ({0}) or badly located: [{1},{2}]", pawn.Name, pawn.Coord.x, pawn.Coord.y));
                        }
                    }
                }
                else{
                    if (pawn.Coord.x < 9 && pawn.Coord.y < 9)
                    {
                        var p = GetPiece(pawn);
                        b.PutPiece(p, Board.Columns_Inv[pawn.Coord.x], pawn.Coord.y);
                    }
                }
            }
            foreach (var hasAlreadyMoved in HaveAlreadyMoved)
            {
                var piece = b.GetPiece(hasAlreadyMoved.Item1, hasAlreadyMoved.Item2);
                ((IHasAlreadyMoved)piece).HasAlreadyMoved = true;
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

            if (b.PawnPromoted)
            {
                //We list the propositions :
                toReturn.NumberLines = 9;
                toReturn.MoveNumber = 1;
            }

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
                        if (p is IHasAlreadyMoved && ((IHasAlreadyMoved)p).HasAlreadyMoved)
                        {
                            Pawn infoHasAlreadyMoved = GetPawn(p, j, i);
                            infoHasAlreadyMoved.Coord.x = -infoHasAlreadyMoved.Coord.x;
                            infoHasAlreadyMoved.Coord.y = -infoHasAlreadyMoved.Coord.y;
                            infoHasAlreadyMoved.Name = HaveAlreadyMovedSpecialName;
                            toReturn.Add(infoHasAlreadyMoved);
                        }
                    }
                }
            }
            if (b.EnPassant != null)
            {
                Pawn enPassant = new Pawn(EnPassantSpecialName, -(Board.Columns[b.EnPassant.Item1] + 1), -(b.EnPassant.Item2));
                toReturn.Add(enPassant);
            }

            if (b.PawnPromoted)
            {
                //We list the propositions :
                int currentColumn = 0;
                foreach (Piece p in b.PawnPromotedChoice())
                {
                    Pawn PawnChoice = GetPawn(p, currentColumn, 8);
                    toReturn.Add(PawnChoice);
                    currentColumn++;
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
            [Description("No pawn here")]
            NoPawnHere = 205,
            [Description("Not your pawn")]
            NotYourPawn = 206,
        }
    }
}
