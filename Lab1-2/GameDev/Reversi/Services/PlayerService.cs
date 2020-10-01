using Reversi.Models;
using Reversi.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Services
{
    public class PlayerService
    {
        private BoardService BoardService { get; set; }
        public List<Player> Players { get; private set; }

        public PlayerService(BoardService boardService)
        {
            this.BoardService = boardService;
        }

        public void InitPlayers(List<PlayerCreateDTO> players)
        {
            Players = players.Select((playerDTO, index) => 
                new Player(playerDTO, index, BoardService, this)).ToList();
        }

        public List<Chip> DoStep(int playerId, ChipDoStepDTO chipDoStepDTO)
        {
            Chip chipForStep = new Chip(chipDoStepDTO, playerId);

            return Players.Where(player => player.Id == playerId)
                .FirstOrDefault().PlayerManager.DoStep(playerId, chipForStep);
        }
    }
}
