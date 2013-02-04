using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMak.DAO
{
    public class DAOException : Exception
    {
        public DAOException(string error) : base(error) { }
    }
}
