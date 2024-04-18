using Domain;

namespace API
{
    public class MarsRoverController
    {
        private Rover Rover { get; set; }
        private PlanetMap Planet { get; set; }
        private string[] ValidMovementCommands = ["f", "b", "l", "r"];

        public void StorePlanetInformation(int planetSizeX, int planetSizeY, IEnumerable<Tuple<int, int>> obstacles)
        {
            var obstaclesCoordinates = obstacles.Select(coordinates => new Coordinates(coordinates.Item1, coordinates.Item2));
            this.Planet = new PlanetMap(planetSizeX, planetSizeY, obstaclesCoordinates);
        }

        public void SetRoverLandingPosition(int x, int y, char directionLabel)
        {
            #region Validation

            // check if the Rover position is within the planet's boundaries
            if (x > Planet.PlanetSizeX || x < 0 ||
                y > Planet.PlanetSizeY || y < 0)
                throw new Exception($"Rover's position is not within the planet's boundaries");

            #endregion

            var direction = CardinalDirections.GetByLabel(directionLabel);

            this.Rover = new Rover(x, y, direction);
        }

        public (int x,int y,char direction) ChartRoute(string[] routeSteps)
        {
            if (routeSteps.Any(step => !ValidMovementCommands.Contains(step)))
                throw new Exception("Unrecognized command in charted route");

            foreach (var step in routeSteps)
            {
                switch (step)
                {
                    case ("f"):
                        break;
                    case ("b"):
                        break;
                    case ("l"):
                        Rover.TurnLeft();
                        break;
                    case ("r"):
                        Rover.TurnRight();
                        break;
                    default:
                        break;
                }

            }


            return (Rover.X, Rover.Y, Rover.Direction.Label);
        }
    }
}
