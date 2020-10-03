using Reversi.Models;
using System.Collections.Generic;

namespace Reversi.Managers
{
    public interface IPlayerManager
    {
        public List<Chip> DoStep(int playerId, Chip chip);
    }
}
