using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Interfaces;
using MikMak.DomainModel.Entities;
using System.ComponentModel;

namespace ConnectFour
{
    public class ConnectFourManager : IGameServices
    {
        private const int maxPawnInColumns = 6;

        public int GetGameType()
        {
            return 3;
        }

        public Grid GetNewGame()
        {
            Grid toReturn = new Grid()
            {
                CurrentMessage = Message.GetMessage(ConnectFourMessage.NewGame),
                MoveNumber = 1,
                NextPlayerToPlay = 1,
                NumberColumns = 7,
                NumberLines = maxPawnInColumns
            };
            return toReturn;
        }

        public Grid Play(Grid currentState, Move move)
        {
            //0-Game already finished, no more move
            if (currentState.CurrentMessage.Id == (int)ClassicMessage.GameFinished)
            {
                return currentState;
            }
            //1-Not Your turn
            if (move.PlayerNumber != currentState.NextPlayerToPlay)
            {
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.NotYourTurn);
                return currentState;
            }

            var choosenX = move.Positions[0].Coord.x;
            var indice = currentState.PawnLocations.Where(o => o.Coord.x == choosenX).Count();
            
            //2-Column full
            if (indice == maxPawnInColumns)
            {
                currentState.CurrentMessage = Message.GetMessage(ConnectFourMessage.ColumnFull);
                return currentState;
            }

            //3-Case Ok, 
            var PawnToAdd = new Pawn(move.PlayerNumber == 1 ? "R" : "Y", choosenX, indice +1);
            currentState.PawnLocations.Add(PawnToAdd);
            if (IsFinished(currentState, PawnToAdd))
            {
                //3-1 Case Ok + finished
                currentState.NextPlayerToPlay = 0;
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.GameFinished);
            }
            else
            {
                //3-1 Case Ok + Not finished
                currentState.NextPlayerToPlay = (currentState.NextPlayerToPlay == 1) ? 2 : 1;
                currentState.CurrentMessage = Message.GetMessage((currentState.NextPlayerToPlay == 2) ? ConnectFourMessage.J2 : ConnectFourMessage.J1);
            }
            return currentState;
        }

        private bool IsFinished(Grid currentState, Pawn pawnToAdd)
        {
            var allPawns = currentState.PawnLocations;
            
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        //We have to do a move at least
                        continue;
                    }

                    int newx, newy;
                    for (int i = 1; i < 4; i++)
                    {
                        newx = pawnToAdd.Coord.x + i*x;
                        newy = pawnToAdd.Coord.y + i*y;
                        if (!isLocationCorrect(newx, newy))
                        {
                            break;
                        }
                        var currentPawn = allPawns.Where(o => o.Coord.x == newx && o.Coord.y == newy).FirstOrDefault();
                        if (currentPawn == null || currentPawn.Name != pawnToAdd.Name) { break; }
                        if (i == 3)
                        {
                            return true;
                        }
                    }
                }                
            }
            return false;
        }

        private bool isLocationCorrect(int x, int y)
        {
            return x > 0 && x <= 8 && y > 0 && y < maxPawnInColumns;
        }

        public enum ConnectFourMessage
        {
            [Description("New game, joueur 1 turn")]
            NewGame = 300,
            [Description("Joueur 1 turn")]
            J1 = 301,
            [Description("Joueur 2 turn")]
            J2 = 302,
            [Description("Column Full")]
            ColumnFull = 303,
        }
    }
}
