using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Model.Pieces
{
    public class King : Piece, IHasAlreadyMoved
    {
        public King( ChessColor color ) : base( color )
        {
        }

        public override void InitializeRules()
        {
            Rules.Add( new Rule(
                        m => m.EndX == m.StartX + 1,
                        m => m.EndY == m.StartY + 1
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX - 1,
                        m => m.EndY == m.StartY - 1
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX - 1,
                        m => m.EndY == m.StartY + 1
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX + 1,
                        m => m.EndY == m.StartY - 1
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX + 1,
                        m => m.EndY == m.StartY
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX,
                        m => m.EndY == m.StartY + 1
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX - 1,
                        m => m.EndY == m.StartY
                        ) );

            Rules.Add( new Rule(
                        m => m.EndX == m.StartX,
                        m => m.EndY == m.StartY - 1
                        ) );
            //Castling rules
            Rules.Add(new Rule(
                        m => m.EndX == m.StartX - 2,
                        m => m.EndY == m.StartY
                        ));

            Rules.Add(new Rule(
                        m => m.EndX == m.StartX + 2,
                        m => m.EndY == m.StartY
                        ));
        }

        public bool HasAlreadyMoved
        {
            get;
            set;
        }
    }
}
