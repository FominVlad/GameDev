namespace Reversi.Models
{
    public enum PlayerType
    {
        PC = 0,
        Human = 1
    }

    public enum PlayerColour
    {
        Black = 0,
        White = 1
    }

    public interface IPlayer
    {
        public int Id { get; }
        public string Name { get; }

        public PlayerType PlayerType { get; }

        public PlayerColour PlayerColour { get; }
    }
}
