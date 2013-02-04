using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;
using MikMak.Configuration;
using System.Data.SqlServerCe;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MikMak.DAO
{
    //TODO
    public class PlayerInBattleDao : TableCache<Tuple<int, int>, PlayerInBattle>, IPlayerInBattleRepository
    {
        private const string SqlPlayerInBattleSelect = "SELECT BattleId, PlayerId, PlayerNumber FROM PlayerInBattles";
        private const string SqlPlayerInBattleInsert = "INSERT INTO PlayerInBattles (PlayerId, BattleId, PlayerNumber) VALUES (@pla, @bat, @num)";

        private PlayerDao playerDao;
        private BattleDao battleDao;

        private static PlayerInBattleDao Instance;
        public static PlayerInBattleDao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PlayerInBattleDao();
            }
            return Instance;
        }

        private PlayerInBattleDao()
        {
            playerDao = PlayerDao.GetInstance();
            battleDao = BattleDao.GetInstance();
        }

        public void Persist(PlayerInBattle playersInBattle)
        {
            PlayerInBattle currentState = Get(new Tuple<int, int>(playersInBattle.Player.PlayerId, playersInBattle.Battle.BattleId));

            if (currentState != null)
            {
                //Already exist
                Update(playersInBattle);
            }
            else
            {
                Create(playersInBattle);
            }
        }

        private void Update(PlayerInBattle playersInBattle)
        {
            battleDao.SaveBattle(playersInBattle.Battle);
        }

        public void Create(PlayerInBattle playersInBattle)
        {
            battleDao.SaveBattle(playersInBattle.Battle);
            int battleId = playersInBattle.Battle.BattleId;
            int playerNumber = 1;
            foreach (int playerId in playersInBattle.Battle.Players)
            {
                //"INSERT INTO PlayerInBattles (PlayerId, BattleId, PlayerNumber) VALUES (...)";
                try
                {
                    using (DbCommand cmd = MyConnection.CreateCommand())
                    {
                        cmd.CommandText = SqlPlayerInBattleInsert;


                        SqlCeCommand ceCmd = cmd as SqlCeCommand;
                        if (ceCmd != null)
                        {
                            ceCmd.Parameters.Add(new SqlCeParameter("@pla", SqlDbType.Int));
                            ceCmd.Parameters.Add(new SqlCeParameter("@bat", SqlDbType.Int));
                            ceCmd.Parameters.Add(new SqlCeParameter("@num", SqlDbType.SmallInt));
                            ceCmd.Prepare();

                            ceCmd.Parameters["@pla"].Value = playerId;
                            ceCmd.Parameters["@bat"].Value = battleId;
                            ceCmd.Parameters["@num"].Value = playerNumber;
                        }
                        else
                        {
                            SqlCommand sqlCmd = cmd as SqlCommand;
                            if (sqlCmd != null)
                            {
                                sqlCmd.Parameters.Add("@pla", SqlDbType.Int);
                                sqlCmd.Parameters["@pla"].Value = playerId;
                                sqlCmd.Parameters.Add("@bat", SqlDbType.Int);
                                sqlCmd.Parameters["@bat"].Value = battleId;
                                sqlCmd.Parameters.Add("@num", SqlDbType.SmallInt);
                                sqlCmd.Parameters["@num"].Value = playerNumber;
                                sqlCmd.Prepare();
                            }
                        }
                        cmd.ExecuteNonQuery();

                        cmd.Parameters.Clear();
                    }
                    playerNumber++;
                }
                catch (Exception e)
                {
                    throw new DAOException(String.Format("Error while creating new PlayerInBattle : {0}", e.Message));
                }
            }
        }

        //TODO
        public List<PlayerInBattle> Get(int playerId)
        {
            List<PlayerInBattle> toReturn = null;
            string query = String.Format("{0} where Playerid = {1} ;", SqlPlayerInBattleSelect, playerId);
            toReturn = GetFromQuery(query);
            return toReturn;
        }

        protected override void LoadFromDatabase()
        {
            if (MyConfiguration.GetBool("loadAllPlayerInBattlesFromDatabase", false))
            {
                string query = String.Format("{0};", SqlPlayerInBattleSelect);
                GetFromQuery(query);
            }
        }

        protected override PlayerInBattle GetFromDatabase(Tuple<int, int> key)
        {
            PlayerInBattle toReturn = null;
            string query = String.Format("{0} where Playerid = {1} and BattleId = {2};", SqlPlayerInBattleSelect, key.Item1, key.Item2);
            toReturn = GetFromQuery(query).FirstOrDefault();
            return toReturn;
        }

        protected override Tuple<int, int> ExtractKey(PlayerInBattle toAdd)
        {
            return new Tuple<int, int>(toAdd.Player.PlayerId, toAdd.Battle.BattleId);
        }
        public PlayerInBattle Get(int battleId, int playerId)
        {
            return Get(new Tuple<int, int>(playerId,battleId));
        }

        protected override PlayerInBattle GetInstanceFromReader(DbDataReader rdr)
        {
            //"SELECT BattleId, PlayerId, PlayerNumber FROM PlayerInBattles"
            var battleId = rdr.GetInt32(0);
            var playerId = rdr.GetInt32(1);
            var playerNumber = rdr.GetInt16(2);

            System.Diagnostics.Trace.Write(" battleId = " + battleId +
                " playerId = " + playerId);

            var result = new PlayerInBattle()
            {
                Battle = battleDao.Get(battleId),
                Player = playerDao.Get(playerId),
                PlayerNumber = playerNumber
            };
            return result;
        }

        public override void Dispose()
        {
            Dispose(playerDao);
            Dispose(battleDao);
        }
    }
}
