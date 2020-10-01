namespace Reversi.Models.DTO
{
    public class PlayerCreateDTO
    {
        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        public PlayerColour PlayerColour { get; set; }
    }
}
