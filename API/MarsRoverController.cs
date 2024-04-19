using Domain;
using Domain.Services;

namespace API
{
    public class MarsRoverController
    {
        private Rover Rover { get; set; }
        private PlanetMap Planet { get; set; }
        private string[] ValidMovementCommands = ["f", "b", "l", "r"];

        /// <summary>
        /// Sets the planet's information in the rover's memory
        /// </summary>
        /// <param name="planetSizeX">Maximum value of the X coordinate for the planet's map</param>
        /// <param name="planetSizeY">Maximum value of the Y coordinate for the planet's map</param>
        /// <param name="obstacles">List of coordinates of the known obstacles on the planet's surface.</param>
        public void StorePlanetInformation(int planetSizeX, int planetSizeY, IEnumerable<(int x, int y)> obstacles)
        {
            var obstaclesCoordinates = obstacles.Select(coordinates => new Coordinates(coordinates.x, coordinates.y));
            this.Planet = new PlanetMap(planetSizeX, planetSizeY, obstaclesCoordinates);
        }

        /// <summary>
        /// Initializes the rover with the landing position on the planet's map
        /// </summary>
        /// <param name="x">X coordinate of the rover's landing position</param>
        /// <param name="y">Y coordinate of the rover's landing position</param>
        /// <param name="directionLabel">A character identifying one of the four cardinal directions: n,e,s,w</param>
        public void SetRoverLandingPosition(int x, int y, char directionLabel)
        {
            #region Validation

            // check if the rover position is within the planet's boundaries
            if (x > Planet.PlanetSizeX || x < 0 ||
                y > Planet.PlanetSizeY || y < 0)
                throw new Exception($"Rover's position is not within the planet's boundaries");

            #endregion

            var direction = CardinalDirections.GetByLabel(directionLabel);

            this.Rover = new Rover(x, y, direction);
        }

        /// <summary>
        /// Instructs the rover to travel along a charted route, wrapping around the planet's edges and stopping before an obstacle, if it's found along the path
        /// </summary>
        /// <param name="routeSteps">A set of characters describing the steps that make up the charted route. Possible values are f,b,l,r (step forward, step backward, turn left, turn right)</param>
        /// <returns>The final position of the rover</returns>
        public (int x,int y,char direction) ChartRoute(string[] routeSteps)
        {
            if (routeSteps.Any(step => !ValidMovementCommands.Contains(step)))
                throw new Exception("Unrecognized command in charted route");

            foreach (var step in routeSteps)
            {
                switch (step)
                {
                    case ("f"):
                        MovementService.PilotRoverToNextPosition(Rover, Planet, movementDirection: 1);
                        break;
                    case ("b"):
                        MovementService.PilotRoverToNextPosition(Rover, Planet, movementDirection: -1);
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


            return (Rover.Position.X, Rover.Position.Y, Rover.Direction.Label);
        }
    }
}
