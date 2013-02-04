using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Repository.Interfaces;
using MikMak.DomainModel.Entities;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;
using MikMak.Configuration;
using System.Data.SqlClient;

namespace MikMak.DAO
{
    public class PlayerDao : TableCache<int, Player>, IPlayerRepository
    {
        private const string SqlPlayerSelect = "SELECT PlayerId, Login, Password, CreationTime, LastUpdate FROM Players";
        private const string SqlPlayerInsert = "INSERT INTO Players (Login, Password, CreationTime, LastUpdate) VALUES (@log, @pas, GETDATE(), GETDATE())";

        private static PlayerDao Instance;
        public static PlayerDao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PlayerDao();
            }
            return Instance;
        }

        private PlayerDao() { }       

        protected override void LoadFromDatabase()
        {
            if (MyConfiguration.GetBool("loadAllPlayersFromDatabase", false))
            {
                string query = String.Format("{0};", SqlPlayerSelect);
                GetFromQuery(query);
            }
        }

        public Player CreatePlayer(string login, string password)
        {
            if (Get(login) != null)
            {
                throw new DAOException(String.Format("Player with login:{0} already exist, cannot create player", login));
            }
            InsertIntoDatabase(login, password);
            return Get(login);
        }

        private void InsertIntoDatabase(string login, string password)
        {
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = SqlPlayerInsert;

                    SqlCeCommand ceCmd = cmd as SqlCeCommand;
                    if (ceCmd != null)
                    {
                        ceCmd.Parameters.Add(new SqlCeParameter("@log", SqlDbType.NVarChar));
                        ceCmd.Parameters.Add(new SqlCeParameter("@pas", SqlDbType.NVarChar));

                        ceCmd.Parameters["@log"].Size = 15;
                        ceCmd.Parameters["@pas"].Size = 15;
                        ceCmd.Prepare();

                        ceCmd.Parameters["@log"].Value = login;
                        ceCmd.Parameters["@pas"].Value = password;
                    }
                    else
                    {
                        SqlCommand sqlCmd = cmd as SqlCommand;
                        if (sqlCmd != null)
                        {
                            sqlCmd.Parameters.Add("@log", SqlDbType.NVarChar);
                            sqlCmd.Parameters["@log"].Value = login;
                            sqlCmd.Parameters.Add("@pas", SqlDbType.NVarChar);
                            sqlCmd.Parameters["@pas"].Value = login;
                            sqlCmd.Parameters["@log"].Size = 15;
                            sqlCmd.Parameters["@pas"].Size = 15;
                            sqlCmd.Prepare();
                        }
                    }
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while creating new player ({0}) : {1}", login, e.Message));
            }
        }

        public Player Get(string login)
        {
            Player toReturn = null;
            if (InternalCacheContains(login))
            {
                //Use of cache policy
                toReturn = allData.First(o => o.Value.Login == login).Value;
            }
            else
            {
                lock (internalLock)
                {
                    if (InternalCacheContains(login))
                    {
                        //Use of cache policy
                        toReturn = allData.First(o => o.Value.Login == login).Value;
                        return toReturn;
                    }
                    toReturn = GetFromDatabase(login);
                }

            }
            return toReturn;
        }

        private Player GetFromDatabase(string login)
        {
            Player toReturn = null;
            string query = String.Format("{0} where Login = '{1}';", SqlPlayerSelect, login);
            toReturn = GetFromQuery(query).FirstOrDefault();
            return toReturn;
        }

        protected override Player GetFromDatabase(int id)
        {
            Player toReturn = null;
            string query = String.Format("{0} where PlayerId = {1};", SqlPlayerSelect, id);
            toReturn = GetFromQuery(query).FirstOrDefault();
            return toReturn;
        }

        //private Player GetFromQuery(string query)
        //{
        //    Player toReturn = null;
        //    try
        //    {
        //        using (SqlCeCommand cmd = MyConnection.CreateCommand())
        //        {
        //            cmd.CommandText = query;

        //            SqlCeDataReader rdr = cmd.ExecuteReader();

        //            while (rdr.Read())
        //            {
        //                toReturn = GetPlayerFromReader(rdr);
        //                break;
        //            }
        //            if (toReturn != null)
        //            {
        //                allData.Add(toReturn.PlayerId, toReturn);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new DAOException(String.Format("Error while reading database query=({0}) : {1}", query, e.Message));
        //    }
        //    return toReturn;
        //}

        #region Helpers

        private static bool InternalCacheContains(string login)
        {
            return allData.Any(o => o.Value.Login == login);
        }
        protected override Player GetInstanceFromReader(DbDataReader rdr)
        {
            var id = rdr.GetInt32(0);
            var login = rdr.GetString(1);
            var pwd = rdr.GetString(2);
            System.Diagnostics.Trace.Write(" playerId = " + id +
                " Login = " + login +
                " Password = " + pwd);

            var p = new Player()
            {
                Login = login,
                Password = pwd,
                PlayerId = id
            };
            return p;
        }

        #endregion

        protected override int ExtractKey(Player toAdd)
        {
            return toAdd.PlayerId;
        }
    }

}
