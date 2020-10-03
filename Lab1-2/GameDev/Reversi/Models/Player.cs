using Reversi.Managers;
using Reversi.Models.DTO;
using Reversi.Services;

namespace Reversi.Models
{
    public enum PlayerType {
        PC = 0,
        Human = 1
    }

    public enum PlayerColour
    {
        Black = 0,
        White = 1
    }

    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public PlayerType PlayerType { get; private set; }

        public PlayerColour PlayerColour { get; private set; }

        public IPlayerManager PlayerManager { get; private set; }

        public Player (PlayerCreateDTO playerCreateDTO, int playerId, 
            BoardService boardService, PlayerService playerService)
        {
            this.Id = playerId;
            this.Name = playerCreateDTO.Name;
            this.PlayerType = playerCreateDTO.PlayerType;
            this.PlayerColour = playerCreateDTO.PlayerColour;

            if (playerCreateDTO.PlayerType == PlayerType.PC)
                PlayerManager = new PlayerManagerPC(boardService, playerService);
            else
                PlayerManager = new PlayerManagerHuman(boardService, playerService);
        }
    }
}
