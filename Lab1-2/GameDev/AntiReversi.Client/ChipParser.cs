using Reversi.Models;
using Reversi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiReversi.Client
{
    class ChipParser
    {
        public Dictionary<string, int> LettersToNumbers { get; private set; }

        public ChipParser()
        {
            InitLettersToNumbersDictionary();
        }

        private void InitLettersToNumbersDictionary()
        {
            LettersToNumbers = new Dictionary<string, int>()
            {
                { "A", 0 },
                { "B", 1 },
                { "C", 2 },
                { "D", 3 },
                { "E", 4 },
                { "F", 5 },
                { "G", 6 },
                { "H", 7 }
            };
        }

        public ChipDoStepDTO ParseStringToChip(string inputStr, int boardSize = 8)
        {
            if (inputStr == null)
                throw new Exception("Inputed string can`t be null.");
            if (inputStr.Length != 2)
                throw new Exception("Inputed string length can`t be not equal 2.");
            
            string inputedPosX = inputStr[0].ToString().ToUpper();

            if (!LettersToNumbers.TryGetValue(inputedPosX, out int posX))
                throw new Exception("Letter is not available.");

            if (Int32.TryParse(inputStr[1].ToString(), out int inputedPosY))
                inputedPosY--;
            else
                throw new Exception("Second char is not number.");

            int posY = inputedPosY >= 0 && inputedPosY < boardSize ? inputedPosY : 
                throw new Exception("Number is not available.");

            return new ChipDoStepDTO() { PosX = posX, PosY = posY };
        }

        public string ParseChipToString(Chip chip, int boardSize = 8)
        {
            if (chip == null)
                throw new Exception("Chip can`t be null.");

            string chipString = string.Empty;

            chipString += LettersToNumbers.Where(obj => obj.Value == chip.PosX)
                .Select(obj => obj.Key).FirstOrDefault() ?? 
                throw new Exception("X position is not available.");
            
            int tmpChipPosY = chip.PosY + 1;

            chipString += tmpChipPosY >= 0 && tmpChipPosY <= boardSize ? tmpChipPosY : 
                throw new Exception("Y position is not available.");

            return chipString;
        }
    }
}
