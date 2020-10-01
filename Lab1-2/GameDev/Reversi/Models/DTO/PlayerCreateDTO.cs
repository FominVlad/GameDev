using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models.DTO
{
    public class PlayerCreateDTO
    {
        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        public PlayerColour PlayerColour { get; set; }
    }
}
