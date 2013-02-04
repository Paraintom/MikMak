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

namespace MikMak.DAO
{
    //TODO
    public class LinkBattlePlayersDao : TableCache<int, List<int>>
    {
        private const string SqlPlayerInBattleSelect = "SELECT BattleId, PlayerId FROM PlayerInBattles";       

        private PlayerDao playerDao;

        private static LinkBattlePlayersDao Instance;
        public static LinkBattlePlayersDao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new LinkBattlePlayersDao();
            }
            return Instance;
        }

        private LinkBattlePlayersDao()
        {
            playerDao = PlayerDao.GetInstance();
        }
        
        public new List<int> Get(int battleId)
        {
            List<int> toReturn = new List<int>();
            if (allData.Keys.Contains(battleId))
            {
                //Use of cache policy
                toReturn = allData[battleId];
            }
            else
            {
                lock (internalLock)
                {
                    if (allData.Keys.Contains(battleId))
                    {
                        //Use of cache policy
                        toReturn = allData[battleId];
                        return toReturn;
                    }
                    toReturn = GetFromQueryInternal(battleId); ;
                }
            }
            return toReturn;
        }

        private List<int> GetFromQueryInternal(int battleId)
        {
            string query = String.Format("{0} where BattleId = {1} ;", SqlPlayerInBattleSelect, battleId);
            List<int> toReturn = new List<int>();
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = query;

                    DbDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        int instance = GetPlayerFromReader(rdr);
                        toReturn.Add(instance);
                    }
                    if (toReturn.Count != 0)
                    {
                        allData[battleId] = toReturn;
                    }
                }
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while reading database query=({0}) : {1}", query, e.Message));
            }
            return toReturn;
        }

        protected override void LoadFromDatabase()
        {
            //Do Nothing;
        }

        public override void Dispose()
        {
            Dispose(playerDao);
        }

        protected override List<int> GetFromDatabase(int key)
        {
            //Should not be used
            throw new NotImplementedException();
        }

        protected override List<int> GetInstanceFromReader(DbDataReader rdr)
        {
            //Should not be used
            throw new NotImplementedException();
        }

        private int GetPlayerFromReader(DbDataReader rdr)
        {
            var id = rdr.GetInt32(1);
            return id;
        }

        protected override int ExtractKey(List<int> toAdd)
        {
            //Should not be used
            throw new NotImplementedException();
        }
    }
}
