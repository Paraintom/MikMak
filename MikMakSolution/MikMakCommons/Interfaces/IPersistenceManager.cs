using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Commons;

namespace MikMak.Interfaces
{
    public interface IPersistenceManager
    {
        GridState GetState(string gameId);
        void SaveState(string gameId, GridState toSave);
    }
}
