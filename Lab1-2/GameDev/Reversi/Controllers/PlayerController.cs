using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi.Models;
using Reversi.Models.Interfaces;
using Reversi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private PlayerService PlayerService { get; set; }

        public PlayerController(PlayerService playerService)
        {
            this.PlayerService = playerService;
        }

        [HttpPost]
        public IActionResult AddPlayers(List<IPlayer> players)
        {
            PlayerService.FirstPlayer = players[0];
            PlayerService.SecondPlayer = players[1];

            return Ok();
        }
    }
}
