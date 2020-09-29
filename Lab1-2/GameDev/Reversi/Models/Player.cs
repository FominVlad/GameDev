using Reversi.Managers;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public enum PlayerType {
        PC = 0,
        Human = 1
    }

    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public PlayerType PlayerType { private get; set; }

        public IPlayerManager PlayerManager { get; private set; }

        public Player(int id, PlayerType playerType, string name, GameService gameService)
        {
            this.Id = id;
            this.PlayerType = playerType;
            this.Name = name;

            if (playerType == PlayerType.PC)
            {
                PlayerManager = new PlayerManagerPC(gameService);
            }
            else
            {
                PlayerManager = new PlayerManagerHuman(gameService);
            }
        }
    }
}
