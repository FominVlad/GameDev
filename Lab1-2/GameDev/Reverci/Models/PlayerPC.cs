using Reversi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class PlayerPC : IPlayer
    {
        public int Id { get; private set; }

        public PlayerType PlayerType { get; private set; }

        public PlayerPC(int id)
        {
            this.Id = id;
            this.PlayerType = PlayerType.PC;
        }
    }
}
