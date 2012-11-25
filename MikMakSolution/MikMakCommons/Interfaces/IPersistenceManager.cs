using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikMakCommons.Interfaces
{
    public interface IPersistenceManager
    {
        GridState GetState(string gameId);
        void SaveState(string gameId, GridState toSave);
    }
}
