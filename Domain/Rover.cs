
namespace Domain
{
    public class Rover
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public CardinalDirection Direction { get; private set; }

        public Rover(int x, int y, CardinalDirection direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public void TurnLeft()
        {
            this.Direction = CardinalDirections.GetByLabel(this.Direction.CounterclockwiseCardinalDirection);
        }

        public void TurnRight()
        {
            this.Direction = CardinalDirections.GetByLabel(this.Direction.ClockwiseCardinalDirection);
        }
    }
}
