namespace MikMak.Infrastructure.InjectionDependancy
{
    using Microsoft.Practices.Unity;
    using MikMak.Repository.EntityFramework;
    using MikMak.Repository.Interfaces;

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
            _container.RegisterType<IPlayerRepository, PlayerRepository>();
            _container.RegisterType<IPlayerInBattleRepository, PlayerInBattleRepository>();
        }
    }
}
