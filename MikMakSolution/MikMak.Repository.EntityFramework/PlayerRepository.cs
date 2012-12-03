using MikMak.Infrastructure.Ressource;
using MikMak.Repository.Interfaces;

namespace MikMak.Repository.EntityFramework
{
    using System.Linq;

    using MikMak.DomainModel.Entities;

    public class PlayerRepository : IPlayerRepository
    {
        private Context _Context;

        public PlayerRepository(Context context)
        {
            _Context = context;
        }

        public string CreatePlayer(Player player)
        {
            string errorMsg = string.Empty;
            if (_Context.Players.Any(g => g.Login == player.Login))
            {
                errorMsg = Error.LOGIN_ALREADY_EXISTS;
            }
            else
            {
                _Context.Players.Add(player);
                _Context.SaveChanges();
            }
            return errorMsg;
        }

        public Player Get(string login)
        {
            throw new System.NotImplementedException();
        }
    }
}
