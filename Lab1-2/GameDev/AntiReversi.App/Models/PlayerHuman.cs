﻿using Reversi.Models.DTO;

namespace Reversi.Models
{
    public class PlayerHuman : IPlayer
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public PlayerType PlayerType { get; private set; }

        public PlayerColour PlayerColour { get; private set; }

        public PlayerHuman(PlayerCreateDTO playerCreateDTO, int playerId)
        {
            this.Id = playerId;
            this.Name = playerCreateDTO.Name;
            this.PlayerColour = playerCreateDTO.PlayerColour;
            this.PlayerType = PlayerType.Human;
        }
    }
}
