using Reversi.Models;
using Reversi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Services
{
    public class PlayerService
    {
        public IPlayer FirstPlayer 
        { 
            get
            {
                return FirstPlayer;
            }
            set
            {
                if (value.PlayerType == PlayerType.Human)
                    FirstPlayer = new PlayerHuman(value.Id);
                else
                    FirstPlayer = new PlayerPC(value.Id);
            }
        }

        public IPlayer SecondPlayer { get; set; }
    }
}
