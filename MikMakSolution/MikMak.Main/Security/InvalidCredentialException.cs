using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Commons;
using System.ComponentModel;

namespace MikMak.Main.Security
{
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialEnum  InvalidCredentialNumber { get; set; }
        
        public InvalidCredentialException(InvalidCredentialEnum raison) : base(raison.Description()) 
        {
            InvalidCredentialNumber = raison;
        }
    }

    public enum InvalidCredentialEnum
    {
        [Description("No login found")]
        NoLoginFound,
        [Description("Bad Password")]
        BadPassword,
        [Description("Impossible to check the credential due to a technical problem")]
        TechnicalException,
        [Description("Unknow session")]
        UnknownSession,
        [Description("Invalid game id")]
        PlayerNotInvolvedInGame,
        [Description("Unknow raison")]
        UnknowRaison
    }
}
