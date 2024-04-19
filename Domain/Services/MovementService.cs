using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public static class MovementService
    {
        /// <summary>
        /// Pilots the rover to its next position
        /// </summary>
        /// <param name="rover">The rover that needs to move</param>
        /// <param name="planet">The planet upon which the rover is at the moment</param>
        /// <param name="movementDirection">The direction of the movement: 1 means forward, -1 means backward</param>
        public static void PilotRoverToNextPosition(Rover rover, PlanetMap planet, int movementDirection)
        {
            var newPositionCoordinates = CalculateDestinationCoordinates(rover.Position, rover.Direction, movementDirection, planet.PlanetSizeX, planet.PlanetSizeY);

            // before moving the rover, check if an obstacle is in the new target position
            var obstacle = planet.Obstacles.Where(obstacle => obstacle.X == newPositionCoordinates.X && obstacle.Y == newPositionCoordinates.Y).FirstOrDefault();

            if (obstacle != null)
                throw new Exception($"Obstacle found along the charted route, at position ({obstacle.X},{obstacle.Y}). The rover will stop moving and remain at its current poisition: \n" +
                    $"X: {rover.Position.X}\n" +
                    $"Y: {rover.Position.Y}\n" +
                    $"Direction: {rover.Direction.Label}\n\n");

            rover.SetNewPosition(newPositionCoordinates);
        }

        /// <summary>
        /// Calculate the destination coordinates of the rover's movement, wrapping up around the planet's edges if needed
        /// </summary>
        /// <param name="roverPosition">The current coordinates of the rover</param>
        /// <param name="roverDirection">The direction the rover is facing</param>
        /// <param name="movementDirection">The direction of the movement the rover needs to perform</param>
        /// <param name="planetSizeX">The maximum X coordinate possible on the planet</param>
        /// <param name="planetSizeY">The maximum Y coordinate possible on the planet</param>
        /// <returns>The coordinates of the new position the rover will occupy after performing the movement</returns>
        private static Coordinates CalculateDestinationCoordinates(Coordinates roverPosition, CardinalDirection roverDirection, int movementDirection, int planetSizeX, int planetSizeY)
        {
            // calculate the new coordinates based on the direction the rover is facing, multiplied by the direction of the movement (1 = forward, -1 = backward)
            var newX = roverPosition.X + roverDirection.XMovement * movementDirection;
            var newY = roverPosition.Y + roverDirection.YMovement * movementDirection;


            // check if the movement brought the rover around the edges of the planet, and adjust the coordinates accordingly
            if (newX > planetSizeX)
                newX = newX - planetSizeX;
            if (newX < 0)
                newX = newX + planetSizeX;
            if(newY > planetSizeY)
                newY = newY - planetSizeY;
            if (newY < 0)
                newY = newY + planetSizeY;

            var destinationCoordinates = new Coordinates(newX, newY);

            return destinationCoordinates;
        }
    }
}
