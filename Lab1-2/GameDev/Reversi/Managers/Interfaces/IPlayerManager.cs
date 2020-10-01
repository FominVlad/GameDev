using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Managers
{
    public interface IPlayerManager
    {
        public List<Chip> DoStep(int playerId, Chip chip);
    }
}
