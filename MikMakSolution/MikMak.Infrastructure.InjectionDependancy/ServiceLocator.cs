namespace MikMak.Infrastructure.InjectionDependancy
{
    using Microsoft.Practices.Unity;
    using MikMak.Repository.EntityFramework;
    using MikMak.Repository.Interfaces;
    using System.Configuration;
    using System.Data.Common;
    using System.Web;
    using System.Data.SqlServerCe;
    using System.Data;

    public static class ConnectionProvider
    {
        private const string CONNECTION_NAME = "MikMakConnection";
        private const string CACHE_CONNECTION = "connection";

        private static string _connectionString;
        private static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ConnectionString;
                }
                return _connectionString;
            }
        }

        public static IDbConnection GetConnection()
        {
            if (HttpContext.Current.Cache[CACHE_CONNECTION] == null)
            {
                HttpContext.Current.Cache[CACHE_CONNECTION] = new SqlCeConnection(ConnectionString);
            }
            return (DbConnection)HttpContext.Current.Cache[CACHE_CONNECTION];
        }
    }

    public class ServiceLocator
    {
        private static ServiceLocator _service = new ServiceLocator();
        private IUnityContainer _container;

        private ServiceLocator()
        {
            this._container = new UnityContainer();
            this.RegisterDataPersistence();
        }

        public static I GetInstance<I>()
        {
            return _service._container.Resolve<I>();
        }

        private void RegisterDataPersistence()
        {
            var PlayerRepositoryParams = new InjectionConstructor(ConnectionProvider.GetConnection());
            _container.RegisterType<IPlayerRepository, PlayerRepository>(PlayerRepositoryParams);
            _container.RegisterType<IPlayerInBattleRepository, PlayerInBattleRepository>();
        }
    }
}
