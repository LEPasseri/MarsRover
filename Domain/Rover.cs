

namespace Domain
{
    public class Rover
    {
        public Coordinates Position { get; private set; }
        public CardinalDirection Direction { get; private set; }

        public Rover(int x, int y, CardinalDirection direction)
        {
            Position = new Coordinates(x, y);
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

        public void SetNewPosition(Coordinates newPositionCoordinates)
        {
            this.Position = newPositionCoordinates;
        }
    }
}
