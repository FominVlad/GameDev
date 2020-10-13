using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models.DTO
{
    public class PlayerGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        public PlayerColour PlayerColour { get; set; }

        public PlayerGetDTO(IPlayer player)
        {
            this.Id = player.Id;
            this.Name = player.Name;
            this.PlayerType = player.PlayerType;
            this.PlayerColour = player.PlayerColour;
        }
    }
}
