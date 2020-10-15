using Reversi.Models;
using Reversi.Models.DTO;
using Reversi.Services;
using System.Collections.Generic;
using System.Linq;

namespace AntiReversi.Client
{
    public class Initializer
    {
        private ColorParser colorParser;

        public Initializer()
        {
            colorParser = new ColorParser();
        }

        public PlayerService InitPlayers(PlayerColour currentPlayerColor, BoardService boardService,
            out int currentPlayerId, out int opponentPlayerId)
        {
            PlayerService playerService = new PlayerService(boardService);

            PlayerColour opponentPlayerColor = colorParser.StringsToPlayerColours
                .Where(obj => obj.Value != currentPlayerColor)
                .Select(obj => obj.Value).FirstOrDefault();

            List<PlayerCreateDTO> playerCreates = new List<PlayerCreateDTO>()
            {
                new PlayerCreateDTO()
                {
                    Name = "Current Player",
                    PlayerColour = currentPlayerColor,
                    PlayerType = PlayerType.PC
                },
                new PlayerCreateDTO()
                {
                    Name = "Opponent Player",
                    PlayerColour = opponentPlayerColor,
                    PlayerType = PlayerType.Human
                }
            };

            playerService.InitPlayers(playerCreates);
            currentPlayerId = playerService.Players
                .Where(player => player.PlayerType == PlayerType.PC)
                .Select(player => player.Id).FirstOrDefault();
            opponentPlayerId = playerService.Players
                .Where(player => player.PlayerType == PlayerType.Human)
                .Select(player => player.Id).FirstOrDefault();

            return playerService;
        }

        public BoardService InitBoardService()
        {
            return new BoardService();
        }

        public void InitBoard(ChipDoStepDTO chipBlackHole, 
            PlayerService playerService, BoardService boardService)
        {
            boardService.InitBoard(8, playerService.Players, chipBlackHole);
        }
    }
}
