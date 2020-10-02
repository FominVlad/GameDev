using Reversi.Models.DTO;

namespace Reversi.Models
{
    public class Chip
    {
        public int OwnerId { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public Chip(ChipDoStepDTO chipDoStepDTO, int playerId) 
        {
            this.OwnerId = playerId;
            this.PosX = chipDoStepDTO.PosX;
            this.PosY = chipDoStepDTO.PosY;
        }

        public Chip(int ownerId, int posX, int posY)
        {
            this.OwnerId = ownerId;
            this.PosX = posX;
            this.PosY = posY;
        }

        public override bool Equals(object obj)
        {
            Chip chip = obj as Chip;

            if (chip == null ||
                chip.OwnerId != this.OwnerId ||
                chip.PosX != this.PosX ||
                chip.PosY != this.PosY)
                return false;

            return true;
        }
    }
}
