using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reversi.Services;

namespace Reversi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameService GameService { get; set; }

        public GameController(GameService gameService)
        {
            this.GameService = gameService;
        }

        /*[HttpPatch]
        public IActionResult DoStep(int playerId, )
        {

        }*/
    }
}
