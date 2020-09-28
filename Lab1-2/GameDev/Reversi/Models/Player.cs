using Reversi.Managers;
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
        public int Id { get; set; }
        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        public IPlayerManager PlayerManager { get; private set; }

        public Player(int id, PlayerType playerType, string name)
        {
            this.Id = id;
            this.PlayerType = playerType;
            this.Name = name;

            if (playerType == PlayerType.PC)
            {
                PlayerManager = new PlayerManagerPC();
            }
            else
            {
                PlayerManager = new PlayerManagerHuman();
            }
        }

        public Player()
        {

        }
    }
}
