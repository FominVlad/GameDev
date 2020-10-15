using Reversi.Models;
using Reversi.Models.DTO;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiReversi.Client
{
    class Program
    {
        private static ChipParser chipParser;
        private static ColorParser colorParser;
        private static BoardService boardService;
        private static PlayerService playerService;
        private static int CurrentPlayerId;
        private static int OpponentPlayerId;
        private static int? LastPlayerId;

        static void Main(string[] args)
        {
            try
            {
                InitClient();

                string blackHole = Console.ReadLine();
                ChipDoStepDTO chipBlackHole = chipParser.ParseStringToChip(blackHole);

                string currentPlayerColor = Console.ReadLine();
                InitPlayers(colorParser.ParseStringToPlayerColor(currentPlayerColor));
                InitBoard(chipBlackHole);

                while (boardService.Board.WinnerPlayerIdList.Count == 0)
                {
                    LastPlayerId = playerService.NextStepPlayerId;
                    if (playerService.NextStepPlayerId == CurrentPlayerId)
                    {
                        Chip movedChip = playerService.DoStep(CurrentPlayerId, null);

                        if (movedChip != null)
                            Console.WriteLine(chipParser.ParseChipToString(movedChip));

                        if (LastPlayerId == playerService.NextStepPlayerId && playerService.NextStepPlayerId != null)
                        {
                            string chipDoStep = Console.ReadLine();
                            if (chipDoStep != "pass")
                                throw new Exception("Opponent must pass!");
                        }
                    }
                    else
                    {
                        string chipDoStep = Console.ReadLine();
                        if (chipDoStep == "pass")
                            break;

                        playerService.DoStep(OpponentPlayerId, chipParser.ParseStringToChip(chipDoStep));

                        if (LastPlayerId == playerService.NextStepPlayerId && playerService.NextStepPlayerId != null)
                            Console.WriteLine("pass");
                    }
                }
            }
            catch
            {
            }
        }

        private static void InitClient()
        {
            chipParser = new ChipParser();
            colorParser = new ColorParser();
            boardService = new BoardService();
            playerService = new PlayerService(boardService);
        }

        private static void InitPlayers(PlayerColour currentPlayerColor)
        {
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
            CurrentPlayerId = playerService.Players
                .Where(player => player.PlayerType == PlayerType.PC)
                .Select(player => player.Id).FirstOrDefault();
            OpponentPlayerId = playerService.Players
                .Where(player => player.PlayerType == PlayerType.Human)
                .Select(player => player.Id).FirstOrDefault();
        }

        private static void InitBoard(ChipDoStepDTO chipBlackHole)
        {
            boardService.InitBoard(8, playerService.Players, chipBlackHole);
        }
    }
}
