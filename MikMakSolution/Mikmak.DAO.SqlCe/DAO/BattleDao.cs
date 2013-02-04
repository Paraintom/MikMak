using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;
using MikMak.Configuration;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;
using System.Data.SqlClient;

namespace MikMak.DAO
{
    public class BattleDao : TableCache<int,Battle>
    {
        private const string SqlBattlesAllSelect = "SELECT BattleId, GameType, GameTypeString, CreationTime, LastUpdate "+ 
                                                   "FROM Battles ";

        private const string SqlBattleInsert = "INSERT INTO Battles(GameType, GameTypeString, CreationTime, LastUpdate, BattleId) VALUES(@typ,@tst, GETDATE(), GETDATE(),@bid)";

        private static GridDao gridDao;
        private static LinkBattlePlayersDao linkDao;
        private static BattleDao Instance;
        public static BattleDao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new BattleDao();
            }
            return Instance;
        }

        private BattleDao()
        {
            gridDao = GridDao.GetInstance();
            linkDao = LinkBattlePlayersDao.GetInstance();
        }
        
        protected override void LoadFromDatabase()
        {
            if (MyConfiguration.GetBool("loadAllBattlesFromDatabase", false))
            {
                string query = String.Format("{0};", SqlBattlesAllSelect);
                GetFromQuery(query);
            }
        }

        public void SaveBattle(Battle toSave)
        {
            if (toSave.Players == null || toSave.Players.Count == 0)
            {
                throw new DAOException("No player in the Battle, cannot persist!");
            }
            if (Get(toSave.BattleId) == null)
            {
                InsertIntoDatabase(toSave);
            }
            else
            {
                UpdateGrid(toSave.BattleId,toSave.CurrentState);
            }
        }

        private void InsertIntoDatabase(Battle toSave)
        {
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = SqlBattleInsert;

                    SqlCeCommand ceCmd = cmd as SqlCeCommand;
                    if (ceCmd != null)
                    {
                        ceCmd.Parameters.Add(new SqlCeParameter("@typ", SqlDbType.SmallInt));
                        ceCmd.Parameters.Add(new SqlCeParameter("@tst", SqlDbType.NVarChar));
                        ceCmd.Parameters.Add(new SqlCeParameter("@bid", SqlDbType.Int));

                        ceCmd.Parameters["@tst"].Size = 20;
                        ceCmd.Prepare();

                        ceCmd.Parameters["@typ"].Value = toSave.GameType;
                        ceCmd.Parameters["@tst"].Value = toSave.GameTypeString;
                        ceCmd.Parameters["@bid"].Value = toSave.BattleId;
                    }
                    else
                    {
                        SqlCommand sqlCmd = cmd as SqlCommand;
                        if (sqlCmd != null)
                        {
                            sqlCmd.Parameters.Add("@typ", SqlDbType.SmallInt);
                            sqlCmd.Parameters["@typ"].Value = toSave.GameType;
                            sqlCmd.Parameters.Add("@tst", SqlDbType.NVarChar);
                            sqlCmd.Parameters["@tst"].Value = toSave.GameTypeString;
                            sqlCmd.Parameters["@tst"].Size = 20;
                            sqlCmd.Parameters.Add("@bid", SqlDbType.Int);
                            sqlCmd.Parameters["@bid"].Value = toSave.BattleId;
                            sqlCmd.Prepare();
                        }
                    }
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                }
                gridDao.SaveGrid(new Tuple<int,Grid>(toSave.BattleId,toSave.CurrentState));
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while creating new battle : {0}", e.Message));
            }
        }

        private void UpdateGrid(int battleId,Grid grid)
        {
            gridDao.SaveGrid(new Tuple<int,Grid>(battleId,grid));
        }       
   
        protected override Battle GetFromDatabase(int id)
        {
            Battle toReturn = null;
            string query = String.Format("{0} where BattleId = {1};", SqlBattlesAllSelect, id);
            toReturn = GetFromQuery(query).FirstOrDefault();
            return toReturn;
        }

        #region Helpers
        private Grid GetGrid(int id)
        {
            Grid toReturn = null;
            if (gridDao == null)
            {
                throw new DAOException("BattleDao - the field gridDao is null, no grid can be retrieved!");
            }
            toReturn = gridDao.Get(id);
            return toReturn;
        }

        protected override Battle GetInstanceFromReader(DbDataReader rdr)
        {
            var id = rdr.GetInt32(0);
            var type = rdr.GetInt16(1);
            var gameTypeString = rdr.GetString(2);
            var creationTime = rdr.GetDateTime(3);
            var lastUpdate = rdr.GetDateTime(4);
            System.Diagnostics.Trace.Write(" battleId = " + id +
                " type = " + type);

            var p = new Battle()
            {
                BattleId = id,
                CreationTime = creationTime,
                LastUpdate = lastUpdate,
                CurrentState = GetGrid(id),
                GameType = type,
                GameTypeString = gameTypeString,
                Players = linkDao.Get(id)
            };
            return p;
        }

        #endregion

        #region IDisposable
        public override void Dispose()
        {
            if (gridDao != null)
            {
                gridDao.Dispose();
            }
            base.Dispose();
        } 
        #endregion

        protected override int ExtractKey(Battle toAdd)
        {
            return toAdd.BattleId;
        }
    }

}
