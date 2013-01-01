using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Model.Pieces;

namespace Chess.Model
{
    public class Board
    {
        public static Dictionary<char, int> Columns = new Dictionary<char, int>() {
                { 'A', 0 }, { 'B', 1 }, { 'C', 2 }, { 'D', 3 }, { 'E', 4 }, { 'F', 5 }, { 'G', 6 }, { 'H', 7 }
        };
        public static Char[] Columns_Inv = new Char[] {'P', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

        public Tuple<char, int> EnPassant
        {
            get;
            set;
        }

        // piece position status on board
        private Piece[,] _pieces;

        public Piece[,] AllPieces()
        {
            return _pieces;
        }

        public Board()
            : this( false )
        {
        }

        public Board( bool startGame )
        {
            Initialize();
            if( startGame )
                InitializeForStartGame();
        }

        // board factory
        public static Board GetNewBoard()
        {
            var board = new Board();

            return board;
        }

        private void Initialize()
        {
            _pieces = new Piece[8, 8];
        }

        public void InitializeForStartGame()
        {
            // set pawns
            foreach( var c in Columns.Keys )
            {
                SetPiece<Pawn>( ChessColor.White, c, 2 );
                SetPiece<Pawn>( ChessColor.Black, c, 7 );
            }

            // set rocks
            SetPiece<Rook>( ChessColor.White, 'A', 1 );
            SetPiece<Rook>( ChessColor.White, 'H', 1 );
            SetPiece<Rook>( ChessColor.Black, 'A', 8 );
            SetPiece<Rook>( ChessColor.Black, 'H', 8 );

            // set knights
            SetPiece<Knight>( ChessColor.White, 'B', 1 );
            SetPiece<Knight>( ChessColor.White, 'G', 1 );
            SetPiece<Knight>( ChessColor.Black, 'B', 8 );
            SetPiece<Knight>( ChessColor.Black, 'G', 8 );

            // set bishops
            SetPiece<Bishop>( ChessColor.White, 'C', 1 );
            SetPiece<Bishop>( ChessColor.White, 'F', 1 );
            SetPiece<Bishop>( ChessColor.Black, 'C', 8 );
            SetPiece<Bishop>( ChessColor.Black, 'F', 8 );

            // set queens
            SetPiece<Queen>( ChessColor.White, 'D', 1 );
            SetPiece<Queen>( ChessColor.Black, 'D', 8 );

            // set kings
            SetPiece<King>( ChessColor.White, 'E', 1 );
            SetPiece<King>( ChessColor.Black, 'E', 8 );
        }

        private Piece TryGetPiece(int column, int row)
        {
            if (column < 1 || column > 8 || row < 1 || row > 8)
            {
                return null;
            }
            else
            {
                return GetPiece(column, row);
            }
        }
        public Piece GetPiece(int column, int row)
        {
            return GetPiece(Columns_Inv[column], row);
        }
        public Piece GetPiece(char column, int row)
        {
            return _pieces[row - 1, Columns[column]];
        }
        public Tuple<int, int> GetFirstPosition(Piece p)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece currentPiece = _pieces[i, j];
                    if (_pieces[i, j] != null && currentPiece.GetType() == p.GetType() && currentPiece.ChessColor == p.ChessColor)
                    {
                        return new Tuple<int, int>(i + 1, j + 1);
                    }
                }
            }

            return null;
        }


        public List<Tuple<int, int>> GetAllPieces(ChessColor color)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece currentPiece = _pieces[i, j];
                    if (_pieces[i, j] != null && currentPiece.ChessColor == color)
                    {
                        result.Add(new Tuple<int, int>(i + 1, j + 1));
                    }
                }
            }
            return result;
        }

        // piece factory
        public void SetPiece<T>( ChessColor color, char column, int row ) where T : Piece 
        {
            var piece = Activator.CreateInstance( typeof( T ), color ) as Piece;
            PutPiece( piece, column, row );
        }

        // set piece at position
        public void PutPiece( Piece piece, char column, int row )
        {
            _pieces[row - 1, Columns[column]] = piece;
        }

        // clear piece at position
        private void clearBoardPosition( char column, int row )
        {
            _pieces[row - 1, Columns[column]] = null;
        }
        #region Move Piece

        // try to do a movement of piece
        public MovementResult MovePiece(int column, int row, int targetColumn, int targetRow)
        {
            return MovePiece(column, row, targetColumn, targetRow, false);
        }

        public MovementResult MovePiece(char column, int row, char targetColumn, int targetRow)
        {
            return MovePiece(column, row, targetColumn, targetRow, false);
        }

        public MovementResult MovePiece(int column, int row, int targetColumn, int targetRow, bool forTest)
        {
            return MovePiece(Columns_Inv[column], row, Columns_Inv[targetColumn], targetRow, forTest);
        }

        private MovementResult MovePiece(char column, int row, char targetColumn, int targetRow, bool forTest)
        {
            MovementResult result = new MovementResult();

            var selectPiece = GetPiece(column, row);

            // check if there is a piece at start position
            if (selectPiece == null)
            {
                result.IsSuccess = false;
                result.Description = String.Format("No piece is present at position {0}{1}", column, row.ToString());

                return result;
            }

            Piece targetPiece = null;
            var isEnPassant = EnPassant != null && selectPiece is Pawn && targetColumn == EnPassant.Item1 && targetRow == EnPassant.Item2;
            if (!isEnPassant)
            {
                targetPiece = GetPiece(targetColumn, targetRow);
            }
            else
            {
                targetPiece = GetPiece(targetColumn, row);
            }



            // check it is a valid movement for piece (rules piece validator)
            if (!selectPiece.IsValidMovement(
                (targetPiece != null && !selectPiece.ChessColor.Equals(targetPiece.ChessColor)),
                row - 1, Columns[column], targetRow - 1, Columns[targetColumn]))
            {
                result.IsSuccess = false;
                result.Description =
                    String.Format("The {0} {1} at position {2}{3} cannot move to {4}{5}",
                    selectPiece.ChessColor.ToString(), selectPiece.GetType().Name,
                    column, row.ToString(), targetColumn, targetRow.ToString());

                return result;
            }

            // check if the path is free if piece is not a knight
            if (!(selectPiece is Knight) && !checkIfPathIsFree(column, row, targetColumn, targetRow))
            {
                result.IsSuccess = false;
                result.Description =
                    String.Format("The path from {0}{1} to {2}{3} for {4}{5} is not free.",
                     column, row.ToString(), targetColumn, targetRow.ToString(),
                     selectPiece.ChessColor.ToString(), selectPiece.GetType().Name);

                return result;
            }

            // check if target position there is already present a piece with same color
            if (targetPiece != null && selectPiece.ChessColor.Equals(targetPiece.ChessColor))
            {
                result.IsSuccess = false;
                result.Description =
                    String.Format("There is already present a {0} piece at position {1}{2}",
                    selectPiece.ChessColor.ToString(), targetColumn, targetRow);

                return result;
            }

            //En passant done
            if (isEnPassant && !forTest)
            {
                clearBoardPosition(targetColumn, row);
            }

            // set result information after ate
            result.Ate = (targetPiece != null && !selectPiece.ChessColor.Equals(targetPiece.ChessColor));
            if (result.Ate)
            {
                result.AtePiece = targetPiece;
            }


            // set EnPassant Information
            EnPassant = null;
            if ((selectPiece is Pawn) && (Math.Abs(row - targetRow) == 2))
            {
                //En passant possibilities
                var pieceLeft = TryGetPiece(Columns[targetColumn], targetRow);
                var pieceRigth = TryGetPiece(Columns[targetColumn] + 2, targetRow);
                if ((pieceLeft != null && pieceLeft.ChessColor != selectPiece.ChessColor && pieceLeft is Pawn)
                    ||
                    (pieceRigth != null && pieceRigth.ChessColor != selectPiece.ChessColor && pieceRigth is Pawn))
                {
                    EnPassant = new Tuple<char, int>(targetColumn, (targetRow + row) / 2);
                }
            }

            //Castling rules
            bool isCastling = false;
            if ((selectPiece is King) && Math.Abs(Columns[column] - Columns[targetColumn]) == 2)
            {
                //1-The king should not have moved yet
                var king = (King)selectPiece;
                if (king.HasAlreadyMoved || !(column == 'E' && (row == 1 || row == 8)))
                {
                    result.IsSuccess = false;
                    result.Description =
                        String.Format("You cannot do the Castling because your king has already moved.");
                    return result;
                }
                //2-The Rook should not have moved yet either
                var rookColumn = (targetColumn == 'G') ? 'H' : 'A';
                var pieceFound = GetPiece(rookColumn, targetRow);
                if (pieceFound == null || pieceFound.ChessColor != king.ChessColor || !(pieceFound is Rook))
                {
                    result.IsSuccess = false;
                    result.Description =
                        String.Format("You cannot do the Castling because there is no Rook at position {0}{1}.",rookColumn,targetRow);
                    return result;
                }

                var goodRook = (Rook)pieceFound;

                if (goodRook.HasAlreadyMoved)
                {
                    result.IsSuccess = false;
                    result.Description =
                        String.Format("You cannot do the Castling as your rook has already moved at position {0}{1}.", rookColumn, targetRow);
                    return result;
                }
                //3-The King should not be chess or passing on a echec position
                var canMoveOnTheKing = CheckKingEchec(king.ChessColor);
                if (canMoveOnTheKing != null)
                {
                    result.IsSuccess = false;
                    result.Description =
                        String.Format("You cannot do the Castling because you are echec {0}{1}.", canMoveOnTheKing.Item1, canMoveOnTheKing.Item2);
                    return result;
                }

                //4-Can do one step :
                var oneStepColumn = (targetColumn == 'G') ? 'F' : 'D';
                var canDoOne = this.MovePiece(column, row, oneStepColumn, targetRow);
                if (!canDoOne.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Description =
                        String.Format("You cannot do the Castling because you would be echec at the position {0}{1}.", oneStepColumn, targetRow);
                    return result;
                }
                else
                {
                    //One step is posible, so rollback, and test two step :
                    PutPiece(king, column, row);
                    clearBoardPosition(oneStepColumn, row);
                    king.HasAlreadyMoved = false;
                    isCastling = true;
                }     
            }

            // change position of piece
            if (!forTest)
            {
                PutPiece(selectPiece, targetColumn, targetRow);
                clearBoardPosition(column, row);
                // check if the move does not make our king in chess
                Tuple<char, int> pieceWitchMakeChess = CheckKingEchec(selectPiece.ChessColor);
                if (pieceWitchMakeChess != null)
                {
                    //RollBack 
                    //supprimer le if suivant qui est inutile; ce test est fait 5 lignes plus haut...
                    if (!forTest)
                    {
                        PutPiece(selectPiece, column, row);
                        clearBoardPosition(targetColumn, targetRow);
                    }

                    result.IsSuccess = false;
                    result.Description =
                        String.Format("Your king is in echec from the piece {0} ({1}{2}).",
                        selectPiece.GetType().Name, pieceWitchMakeChess.Item1, pieceWitchMakeChess.Item2);

                    return result;
                }

                if (selectPiece is IHasAlreadyMoved)
                {
                    ((IHasAlreadyMoved)selectPiece).HasAlreadyMoved = true;
                    //TODO : Move the rook in case of castling
                    if (isCastling)
                    {
                        var rookColumnOrigin = (targetColumn == 'G') ? 'H' : 'A';
                        var rookColumnTarget = (targetColumn == 'G') ? 'F' : 'D';
                        var pieceFound = GetPiece(rookColumnOrigin, row);
                        PutPiece(pieceFound, rookColumnTarget, row);
                        clearBoardPosition(rookColumnOrigin, row);
                    }
                }
            }
            return result;
        }

        #endregion

        private Tuple<char, int> CheckKingEchec(ChessColor chessColor)
        {
            Tuple<int, int> currentPosition = GetFirstPosition(new King(chessColor));
            if (currentPosition == null)
            {
                return null;
            }
            int currentRow = currentPosition.Item1;
            int currentColumn = currentPosition.Item2;

            var otherPieces = GetAllPieces(chessColor == ChessColor.White ? ChessColor.Black : ChessColor.White);
            var result_int = otherPieces.Where(o=> this.MovePiece(o.Item2, o.Item1, currentColumn,currentRow, true).IsSuccess).FirstOrDefault();

            return result_int == null ? null :new Tuple<char, int>(Columns_Inv[result_int.Item1], result_int.Item2);
        }

        // check if the path for select piece is free
        private bool checkIfPathIsFree( char column, int row, char targetColumn, int targetRow )
        {
            bool result = true;

            int stepx = Columns[targetColumn].CompareTo( Columns[column] );

            int stepy = targetRow.CompareTo( row );

            // start position
            int c = Columns[column];
            int r = row - 1;

            // next position
            c = c + stepx;
            r = r + stepy;

            while( !(c == Columns[targetColumn] && r == (targetRow -1)) )
            {
                var p = _pieces[r, c];

                if( p != null )
                {
                    result = false;
                    break;
                }

                // next position
                c = c + stepx;
                r = r + stepy;
            }

            return result;
        }

    }
}
