using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;
using MikMak.Configuration;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace MikMak.DAO
{
    class GridDao : TableCache<int, GridExtented>
    {
        private const string SqlGridsAllSelect = "SELECT BattleId, NumberLines, NumberColumns, IsGridShifted, MoveNumber, PawnLocations, NextPlayerToPlay, CurrentMessage, LastMove FROM Grids";
        private const string SqlGridInsert = "INSERT INTO Grids (BattleId, NumberLines, NumberColumns, IsGridShifted, MoveNumber, PawnLocations, NextPlayerToPlay, CurrentMessage, LastMove)" +
                                            "VALUES        (@BattleId,@NumberLines,@NumberColumns,@IsGridShifted,@MoveNumber,@PawnLocations,@NextPlayerToPlay,@CurrentMessage,@LastMove);";
        private const string SqlGridUpdate = "UPDATE Grids SET PawnLocations = @PawnLocations, NextPlayerToPlay = @NextPlayerToPlay, CurrentMessage = @CurrentMessage, LastMove = @LastMove ";

        private static GridDao Instance;
        public static GridDao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new GridDao();
            }
            return Instance;
        }

        private GridDao() { }

        protected override void LoadFromDatabase()
        {
            if (MyConfiguration.GetBool("loadAllGridsFromDatabase", false))
            {
                string query = String.Format("{0};", SqlGridsAllSelect);
                GetFromQuery(query);
            }
        }

        public void SaveGrid(Tuple<int,Grid> toSave)
        {
            if (GetFromDatabase(toSave.Item1) == null)
            {
                InsertIntoDatabase(toSave);
            }
            else
            {
                UpdateGrid(toSave);
            }
        }

        private void InsertIntoDatabase(Tuple<int, Grid> toSave)
        {
            //BattleId, NumberLines, NumberColumns, IsGridShifted, MoveNumber, 
            //PawnLocations, NextPlayerToPlay, CurrentMessage, LastMove
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = SqlGridInsert;
                    Grid grid = toSave.Item2;
                    SqlCommand sqlCmd = cmd as SqlCommand;
                    if (sqlCmd != null)
                    {
                        sqlCmd.Parameters.Add("@BattleId", SqlDbType.Int);
                        sqlCmd.Parameters.Add("@NumberLines", SqlDbType.SmallInt);
                        sqlCmd.Parameters.Add("@NumberColumns", SqlDbType.SmallInt);
                        sqlCmd.Parameters.Add("@IsGridShifted", SqlDbType.NChar);
                        sqlCmd.Parameters.Add("@MoveNumber", SqlDbType.SmallInt);

                        sqlCmd.Parameters.Add("@PawnLocations", SqlDbType.NVarChar);
                        sqlCmd.Parameters.Add("@NextPlayerToPlay", SqlDbType.SmallInt);
                        sqlCmd.Parameters.Add("@CurrentMessage", SqlDbType.NVarChar);
                        sqlCmd.Parameters.Add("@LastMove", SqlDbType.NVarChar);

                        sqlCmd.Parameters["@IsGridShifted"].Size = 5;
                        sqlCmd.Parameters["@PawnLocations"].Size = 500;
                        sqlCmd.Parameters["@CurrentMessage"].Size = 100;
                        sqlCmd.Parameters["@LastMove"].Size = 100;

                        sqlCmd.Parameters["@BattleId"].Value = toSave.Item1;
                        sqlCmd.Parameters["@NumberLines"].Value = grid.NumberLines;
                        sqlCmd.Parameters["@NumberColumns"].Value = grid.NumberColumns;
                        sqlCmd.Parameters["@IsGridShifted"].Value = grid.IsGridShiftedToDb();
                        sqlCmd.Parameters["@MoveNumber"].Value = grid.MoveNumber;
                        sqlCmd.Parameters["@PawnLocations"].Value = grid.PawnLocations.PawnLocationsToDb();
                        sqlCmd.Parameters["@NextPlayerToPlay"].Value = grid.NextPlayerToPlay;
                        sqlCmd.Parameters["@CurrentMessage"].Value = grid.CurrentMessage.CurrentMessageToDb();
                        sqlCmd.Parameters["@LastMove"].Value = grid.LastMove.LastMoveToDb();

                        sqlCmd.Prepare();
                    }

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while creating new battle : {0}", e.Message));
            }
        }

        private void UpdateGrid(Tuple<int,Grid> toSave)
        {
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = String.Format("{0} where BattleID={1};",SqlGridUpdate,toSave.Item1);
                    //PawnLocations = ?, NextPlayerToPlay = ?, CurrentMessage = ?, LastMove = ?

                    Grid grid = toSave.Item2;

                    SqlCommand sqlCmd = cmd as SqlCommand;
                    if (sqlCmd != null)
                    {
                        sqlCmd.Parameters.Add("@PawnLocations", SqlDbType.NVarChar);
                        sqlCmd.Parameters.Add("@NextPlayerToPlay", SqlDbType.SmallInt);
                        sqlCmd.Parameters.Add("@CurrentMessage", SqlDbType.NVarChar);
                        sqlCmd.Parameters.Add("@LastMove", SqlDbType.NVarChar);

                        sqlCmd.Parameters["@PawnLocations"].Size = 500;
                        sqlCmd.Parameters["@CurrentMessage"].Size = 100;
                        sqlCmd.Parameters["@LastMove"].Size = 100;
                        sqlCmd.Prepare();

                        sqlCmd.Parameters["@PawnLocations"].Value = grid.PawnLocations.PawnLocationsToDb();
                        sqlCmd.Parameters["@NextPlayerToPlay"].Value = grid.NextPlayerToPlay;
                        sqlCmd.Parameters["@CurrentMessage"].Value = grid.CurrentMessage.CurrentMessageToDb(); ;
                        sqlCmd.Parameters["@LastMove"].Value = grid.LastMove.LastMoveToDb();
                    }

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while updating battle : {0}", e.Message));
            }
        }       
   
        protected override GridExtented GetFromDatabase(int id)
        {
            GridExtented toReturn = null;
            string query = String.Format("{0} where BattleId = {1};", SqlGridsAllSelect, id);
            toReturn = GetFromQuery(query).FirstOrDefault();
            return toReturn;
        }
        
        #region Helpers

        protected override GridExtented GetInstanceFromReader(DbDataReader rdr)
        {            
            //BattleId, NumberLines, NumberColumns, IsGridShifted, MoveNumber, 
            //PawnLocations, NextPlayerToPlay, CurrentMessage, LastMove ;

            var id = rdr.GetInt32(0);
            var lines = rdr.GetInt16(1);
            var columns = rdr.GetInt16(2);
            var isShifted = rdr.GetString(3).IsGridShiftedFromDb();
            var moveNumber = rdr.GetInt16(4);

            var pawnLocation = rdr.GetString(5).PawnLocationsFromDb();
            var nextPlayer = rdr.GetInt16(6);
            var currentMessage = rdr.GetString(7);
            var lastMove = rdr.GetString(8).LastMoveFromDb();

            System.Diagnostics.Trace.Write(" battleId = " + id +
                " currentMessage = " + currentMessage);

            var grid = new GridExtented()
            {
                BattleId = id,
                CurrentMessage = currentMessage.CurrentMessageFromDb(),
                IsGridShifted = isShifted,
                LastMove = lastMove,
                MoveNumber = moveNumber,
                NextPlayerToPlay = nextPlayer,
                NumberColumns = columns,
                NumberLines = lines,
                PawnLocations = pawnLocation
            };

            return grid;
        }

        #endregion
        
        protected override int ExtractKey(GridExtented toAdd)
        {
            return toAdd.BattleId;
        }
    }

    public static class ExtentionMethodForDb
    {
        public static string IsGridShiftedToDb(this Grid g)
        {
            return g.IsGridShifted ? "YES" : "NO";
        }
        public static bool IsGridShiftedFromDb(this string g)
        {
            return g.Equals("YES") ? true : false;
        }

        private static Regex regexPawn = new Regex(@"(?<name>\w{1,10}):(?<playerId>\d{1,10}):(?<x>\d{1,10}),(?<y>\d{1,20})");
        public static string PawnLocationsToDb(this List<Pawn> g)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pawn in g)
            {
                sb.Append(String.Format("{0}:{1}:{2},{3};", pawn.Name, pawn.Player != null ? pawn.Player.PlayerId : 0, pawn.Coord.x, pawn.Coord.y));
            }
            return sb.ToString();
        }
        public static List<Pawn> PawnLocationsFromDb(this string g)
        {
            List<Pawn> result = new List<Pawn>();
            if (!String.IsNullOrEmpty(g))
            {
                var tab = g.Split(';');
                foreach (string stringPawn in tab)
                {
                    Match match = regexPawn.Match(stringPawn);
                    if (match.Success)
                    {
                        Pawn p = new Pawn()
                        {
                            Name = match.Groups["name"].Value,
                            Coord = new Coord()
                            {
                                x = Int32.Parse(match.Groups["x"].Value),
                                y = Int32.Parse(match.Groups["y"].Value)
                            },
                            Player = new Player()
                            {
                                PlayerId = Int32.Parse(match.Groups["playerId"].Value)
                            }
                        };
                        result.Add(p);
                    }
                }
            }
            return result;
        }

        public static string LastMoveToDb(this Move g)
        {
            if (g == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(g.PlayerNumber);
            sb.Append("-");
            sb.Append(PawnLocationsToDb(g.Positions));
            return sb.ToString();
        }
        public static Move LastMoveFromDb(this string g)
        {
            Move result = null;
            if (!String.IsNullOrEmpty(g))
            {
                var tab = g.Split('-');
                if (tab.Length == 2)
                {
                    int playerNumber = Int32.Parse(tab[0]);
                    List<Pawn> allMove = PawnLocationsFromDb(tab[1]);
                    result = new Move()
                    {
                        PlayerNumber = playerNumber,
                        Positions = allMove
                    };
                }
            }
            return result;
        }
        
        public static string CurrentMessageToDb(this Message g)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(g.Id);
            sb.Append("_");
            sb.Append(g.Information);
            return sb.ToString();
        }
        public static Message CurrentMessageFromDb(this string g)
        {
            Message toReturn = Message.GetMessage(ClassicMessage.Default);
            if (!String.IsNullOrWhiteSpace(g))
            {
                var tab = g.Split('-');
                if (tab.Length == 2)
                {
                    int id = 0;
                    Int32.TryParse(tab[0], out id);
                    toReturn.Id = id;
                    toReturn.Information = tab[1];
                }
            }
            return toReturn;
        }
    }
}
