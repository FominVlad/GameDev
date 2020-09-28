using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reversi.Controllers
{
    public class BoardController : ControllerBase
    {
        private BoardService BoardService { get; set; }

        public BoardController(BoardService boardService)
        {
            this.BoardService = boardService;
        }
    }
}
