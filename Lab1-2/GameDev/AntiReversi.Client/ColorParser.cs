using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntiReversi.Client
{
    class ColorParser
    {
        private Dictionary<string, PlayerColour> StringsToPlayerColours { get; set; }

        public ColorParser()
        {
            InitStringsToPlayerColoursDictionary();
        }

        private void InitStringsToPlayerColoursDictionary()
        {
            StringsToPlayerColours = new Dictionary<string, PlayerColour>()
            {
                { "black", PlayerColour.Black },
                { "white", PlayerColour.White }
            };
        }

        public PlayerColour ParseStringToPlayerColor(string inputStr)
        {
            if (inputStr == null)
                throw new Exception("Inputed string can`t be null.");

            if (!StringsToPlayerColours.TryGetValue(inputStr.ToLower(), out PlayerColour playerColour))
                throw new Exception("Inputed colour is not available.");

            return playerColour;
        }
    }
}
