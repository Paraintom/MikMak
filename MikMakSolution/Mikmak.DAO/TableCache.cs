using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace MikMak.DAO
{
    public abstract class TableCache<K, V> : IDisposable
    {
        protected DbConnection MyConnection { get; private set; }
        protected static Dictionary<K, V> allData = new Dictionary<K, V>();
        protected object internalLock = new object();

        public TableCache()
        : this(MyConfiguration.GetString("MikMakDbConnection", @"Data Source=|DataDirectory|\LocalDb.sdf"))            
        {
        }
        
        private TableCache(string connectionString)
        {
            try
            {
                    SqlConnectionStringBuilder connString1Builder = new SqlConnectionStringBuilder(connectionString);
                    connString1Builder.Encrypt = true;
                    connString1Builder.TrustServerCertificate = false;
                    MyConnection = new SqlConnection(connString1Builder.ToString());
                MyConnection.Open();
                LoadFromDatabase();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("TableCache, Ex : "+e.Message);
            }
        }

        protected abstract void LoadFromDatabase();

        protected abstract V GetFromDatabase(K key);

        public V Get(K key)
        {
            V toReturn = default(V);
            if (allData.Keys.Contains(key))
            {
                //Use of cache policy
                toReturn = allData[key];
            }
            else
            {
                lock (internalLock)
                {
                    if (allData.Keys.Contains(key))
                    {
                        //Use of cache policy
                        toReturn = allData[key];
                        return toReturn;
                    }
                    toReturn = GetFromDatabase(key);
                }

            }
            return toReturn;
        }

        protected List<V> GetFromQuery(string query)
        {
            List<V> toReturn = new List<V>();
            try
            {
                using (DbCommand cmd = MyConnection.CreateCommand())
                {
                    cmd.CommandText = query;

                    DbDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        V instance = GetInstanceFromReader(rdr);
                        toReturn.Add(instance);
                    }
                    foreach (V toAdd in toReturn)
                    {
                        AddToCache(toAdd);
                    }
                }
            }
            catch (Exception e)
            {
                throw new DAOException(String.Format("Error while reading database query=({0}) : {1}", query, e.Message));
            }
            return toReturn;
        }

        protected abstract V GetInstanceFromReader(DbDataReader rdr);

        private void AddToCache(V toAdd)
        {
            if(toAdd != null)
                allData[ExtractKey(toAdd)] = toAdd;
        }
        protected abstract K ExtractKey(V toAdd);

        public virtual void Dispose()
        {
            lock (internalLock)
            {
                if (MyConnection != null)
                {
                    MyConnection.Close();
                }
            }
        }
        
        public static void Dispose(IDisposable toDispose)
        {
            if (toDispose != null)
                toDispose.Dispose();
        }

        ~TableCache()
        {
            Dispose();
        }
        //Very usefull!
        //protected static void ShowErrors(SqlCeException e)
        //{
        //    SqlCeErrorCollection errorCollection = e.Errors;

        //    StringBuilder bld = new StringBuilder();
        //    Exception inner = e.InnerException;

        //    foreach (SqlCeError err in errorCollection)
        //    {
        //        bld.Append("\n Error Code: " + err.HResult.ToString("X"));
        //        bld.Append("\n Message   : " + err.Message);
        //        bld.Append("\n Minor Err.: " + err.NativeError);
        //        bld.Append("\n Source    : " + err.Source);

        //        foreach (int numPar in err.NumericErrorParameters)
        //        {
        //            if (0 != numPar) bld.Append("\n Num. Par. : " + numPar);
        //        }

        //        foreach (string errPar in err.ErrorParameters)
        //        {
        //            if (String.Empty != errPar) bld.Append("\n Err. Par. : " + errPar);
        //        }

        //        MessageBox.Show(bld.ToString());
        //        bld.Remove(0, bld.Length);
        //    }
        //}
    }
}
