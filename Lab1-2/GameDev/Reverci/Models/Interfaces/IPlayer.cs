using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models.Interfaces
{
    public enum PlayerType
    {
        PC = 0,
        Human = 1
    }

    public interface IPlayer
    {
        public int Id { get; }

        public PlayerType PlayerType { get; }
    }
}
