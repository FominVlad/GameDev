using Reversi.Models;
using Reversi.Models.DTO;
using Reversi.Services;
using System;

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

                while (boardService.Board.WinnerPlayerIdList.Count == 0)
                {
                    LastPlayerId = playerService.NextStepPlayerId;
                    if (playerService.NextStepPlayerId == CurrentPlayerId)
                    {
                        Chip movedChip = playerService.DoStep(CurrentPlayerId, null);

                        Console.WriteLine(chipParser.ParseChipToString(movedChip));

                        if (LastPlayerId == playerService.NextStepPlayerId && playerService.NextStepPlayerId != null)
                        {
                            string chipDoStep = Console.ReadLine();
                            if (chipDoStep != "pass")
                                throw new Exception("Command is undefined.");
                        }
                    }
                    else
                    {
                        string chipDoStep = Console.ReadLine();
                        if (chipDoStep == "pass")
                            break;

                        playerService.DoStep(OpponentPlayerId, chipParser.ParseStringToChip(chipDoStep));

                        if (LastPlayerId == playerService.NextStepPlayerId && 
                            playerService.NextStepPlayerId != null)
                            Console.WriteLine("pass");
                    }
                }
            }
            catch { }
        }

        private static void InitClient()
        {
            Initializer initializer = new Initializer();
            colorParser = new ColorParser();
            chipParser = new ChipParser();

            string blackHole = Console.ReadLine();
            ChipDoStepDTO chipBlackHole = chipParser.ParseStringToChip(blackHole);

            string currentPlayerColor = Console.ReadLine();
            boardService = initializer.InitBoardService();
            playerService = initializer.InitPlayers(colorParser.ParseStringToPlayerColor(currentPlayerColor),
                boardService, out CurrentPlayerId, out OpponentPlayerId);
            initializer.InitBoard(chipBlackHole, playerService, boardService);
        }
    }
}
