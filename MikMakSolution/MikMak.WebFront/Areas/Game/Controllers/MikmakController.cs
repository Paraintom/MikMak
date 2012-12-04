using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MikMak.DomainModel.Entities;
using MikMak.Interfaces;
using MikMak.WebFront.Sessions;
using MikMak.Main;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class MikmakController : ApiController
    {
        //TODO : to initialise and USE!!!
        private IPlayerManager playerManager;
        private IGamesManager gameManager;
        private ISessionManager sessionManager;

        public MikmakController(){
        }

        #region Connection : Connect
        /// <summary>
        /// Initial connection
        /// </summary>
        /// <param name="login">the login</param>
        /// <param name="password">the password</param>
        /// <returns>a sessionId string</returns>
        [AcceptVerbs("GET")]
        public string Connect(string login, string password)
        {
            HackJsonp();

            string toReturn = string.Empty;
            try
            {
                var firstSession = sessionManager.GetSession(login, password);
            }
            catch (Exception ex)
            {
                toReturn = ex.Message;
            }
            return toReturn;
        }

        private void HackJsonp()
        {
            var negotiator = Configuration.Services.GetContentNegotiator();
            var negotiatorResult = negotiator.Negotiate(typeof(string), Request, Configuration.Formatters);
        } 
        #endregion

        #region Show list of battles : GetListBattles
        [AcceptVerbs("GET")]
        public List<Battle> GetListBattles(string sessionId)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);
                return gameManager.GetAllBattles(session.PlayerInBattle.Player).Select(o => o.Battle).ToList();
            }
            catch(Exception){
                // J'ai pas trouvé mieux POUR l'instant.
                throw;
            }            
        } 
        #endregion

        #region Create a new battle : CreateBattle
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2, string opponent3)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2, opponent3 });
        }
        [AcceptVerbs("GET")]
        public GridExtented CreateBattle(string sessionId, int typeGame, string opponent1, string opponent2, string opponent3, string opponent4)
        {
            return CreateBattle(sessionId, typeGame, new string[] { opponent1, opponent2, opponent3, opponent4 });
        }

        public GridExtented CreateBattle(string sessionId, int typeGame, params string[] opponents)
        {
            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId);

                // 2-Check valid opponents
                List<Player> allOpponents = new List<Player>();
                foreach(string opp in opponents){
                    var player = playerManager.Get(opp);
                    if(!allOpponents.Contains(player)){
                        allOpponents.Add(player);
                    }
                    else{
                        throw new ArgumentException("You specified a login twice");
                    }
                }

                // 3-Create new game
                var newGame = gameManager.GetNewGame(session.PlayerInBattle.Player, typeGame, allOpponents);
                var newSession = sessionManager.GetSession(session, newGame.Battle.GameId);

                // 4-Return result
                return new GridExtented(){
                    sessionId = newSession.Id,
                    state = newGame.Battle.CurrentState
                };
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion

        #region Get grid state : GetGrid
        [AcceptVerbs("GET")]
        public GridExtented GetGrid(string sessionId)
        {

            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId);

                // 2-Return result
                return new GridExtented(){
                    sessionId = session.Id,
                    state = session.PlayerInBattle.Battle.CurrentState
                };
            }
            catch(Exception){
                throw;
            }
        } 
        #endregion

        #region Play a move : Play
        [AcceptVerbs("GET")]
        public GridExtented Play(string sessionId, int x, int y)
        {
            try
            {
                // 1-Check valid session
                var session = sessionManager.GetSession(sessionId);
                Move move = new Move()
                {
                    PlayerNumber = session.PlayerInBattle.PlayerNumber,
                    Positions = new List<Pawn>(){
                        new Pawn(){
                            Coord = new Coord(){
                                x = x,
                                y = y
                            },
                            Name = '?',
                            Player = session.PlayerInBattle.Player
                        }                        
                    }
                };
                var newState = gameManager.Play(session.PlayerInBattle, move);

                // 2-Return result
                return new GridExtented()
                {
                    sessionId = session.Id,
                    state = newState
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion

    }
}
