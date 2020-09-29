using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models.DTO
{
    public class PlayerAddDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        //public static explicit operator Player(PlayerAddDTO playerAddDTO)
        //{
        //    return new Player(playerAddDTO.Id, playerAddDTO.PlayerType, playerAddDTO.Name);
        //}
    }
}
