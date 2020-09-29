using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Managers
{
    public class PlayerManagerHuman : IPlayerManager
    {
        private GameService GameService { get; set; }
        public PlayerManagerHuman(GameService gameService)
        {
            this.GameService = gameService;
        }

        public bool DoStep(int playerId, Chip chip)
        {
            throw new NotImplementedException();
        }
    }
}
