namespace MikMak.WebFront.Sessions
{
    using MikMak.DomainModel.Entities;
    public interface ISessionManager
    {
        /// <summary>
        /// For the initial connection, return a Session.
        /// (used for browsing games at the start)
        /// </summary>
        /// <param name="login">the player login</param>
        /// <param name="password">the player password</param>
        /// <returns>a temporary session</returns>
        Session GetSession(string login, string password);

        /// <summary>
        /// Return a session from a sessionId.
        /// </summary>
        /// <param name="sessionId">The sessionId</param>
        /// <returns>The corresponding session</returns>
        Session GetSession(string sessionId);

        /// <summary>
        /// Return a session for a gameId.
        /// </summary>
        /// <param name="otherSession">other validSession</param>
        /// <param name="gameId">GameId that we want to connect</param>
        /// <returns></returns>
        Session GetSession(Session otherSession, string gameId);

        //Session GetSession(Session otherSession, string gameId, int gameType, int playerNumber);
    }
}
