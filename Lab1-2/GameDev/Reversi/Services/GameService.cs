using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Services
{
    public class GameService
    {
        public Board Board { get; private set; }
        public List<Player> Players { get; private set; }

        public GameService()
        {
            Players = new List<Player>();
        }

        public void AddPlayers(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player == null)
                    throw new Exception("Player in player list can`t be null.");

                Players.Add(player);
            }
        }

        public void CreateBoard(Board board)
        {
            if (board == null)
                throw new Exception("Board can`t be null.");

            this.Board = board;
        }
    }
}
