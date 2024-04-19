using API;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverRemoteControl
{
    public class EarthHQ
    {
        public MarsRoverController MarsRoverController { get; set; }

        public EarthHQ(MarsRoverController marsRoverController)
        {
            this.MarsRoverController = marsRoverController;
        }

        /// <summary>
        /// Sends to the rover the initial information that are needed to know its surroundings and its location
        /// </summary>
        public void InitializeRover()
        {
            Console.WriteLine("Initializing rover..");

            SendPlanetInformation();

            SendRoverLandingPosition();
        }


        /// <summary>
        /// Sends to the rover a list of commands to pilot it along the surface of the planet
        /// </summary>
        public void ChartRoverRoute()
        {
            Console.WriteLine("Input the rover's route step by step, including turns. (accepted instructions: f,b,l,r - delimeter: ,)\n");

            try
            {
                var routeSteps = Console.ReadLine()?.ToLower().Split(',');

                if (routeSteps == null)
                    throw new Exception("No steps were provided for the route");

                var routeOutcome = MarsRoverController.ChartRoute(routeSteps);

                Console.WriteLine("The rover moved succesfully along the designated route. This is its final position:\n" +
                    $"X: {routeOutcome.x}\n" +
                    $"Y: {routeOutcome.y}\n" +
                    $"Direction: {routeOutcome.direction}\n\n"
                    );
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"An error has occurred while charting the rover's route: {ex.Message} - Try again with a different route. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                ChartRoverRoute();
            }
        }

        /// <summary>
        /// Reads the information about the planet from a configuration file, and sends it to the rover
        /// </summary>
        private void SendPlanetInformation()
        {
            Console.WriteLine("Sending planet map with known obstacles..");

            try
            {
                #region Read planet data from config

                var planetSizeConfigValues = ConfigurationManager.AppSettings["PlanetSize"]?.Split(',');

                if (planetSizeConfigValues == null)
                    throw new Exception("Planet size is not configured");

                var planetSizeX = Convert.ToInt32(planetSizeConfigValues[0]);
                var planetSizeY = Convert.ToInt32(planetSizeConfigValues[1]);

                var obstaclesCoordinatesConfigValue = ConfigurationManager.AppSettings["ObstaclesCoordinates"]?.Split(';').ToList();

                if (obstaclesCoordinatesConfigValue == null)
                    throw new Exception("Obstacles coordinates are not configured");

                var obstacles = obstaclesCoordinatesConfigValue.Select(coordinates => (x: Convert.ToInt32(coordinates.Split(',')[0]), y: Convert.ToInt32(coordinates.Split(',')[1])));

                #endregion

                MarsRoverController.StorePlanetInformation(planetSizeX, planetSizeY, obstacles);

                Console.WriteLine("The planet has the following features: \n" +
                    $"width: {planetSizeX}\n" +
                    $"length: {planetSizeY}\n" +
                    $"obstacles: {String.Join(';', obstaclesCoordinatesConfigValue)} \n\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"An error has occurred while initializing the rover: {ex.Message} - The application will now close. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Sends information to the rover about where it landed on the planet based on the known map
        /// </summary>
        private void SendRoverLandingPosition()
        {
            Console.WriteLine("Which are the rover's landing coordinates, and which cardinal direction is it facing? (expected format: x,y,d)\n");

            try
            {
                var rawLandingPosition = Console.ReadLine()?.Split(',');

                if (rawLandingPosition == null)
                    throw new Exception("The landing coordinates and the direction must be provided in this format: x,y,d ");

                MarsRoverController.SetRoverLandingPosition(x: Convert.ToInt32(rawLandingPosition[0]), y: Convert.ToInt32(rawLandingPosition[1]), directionLabel: rawLandingPosition[2].ToCharArray()[0]);

                Console.WriteLine("Rover's landing information set\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"There was an error while setting the rover's initial position: {ex.Message} - Try again with a different value. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                SendRoverLandingPosition();
            }
        }


    }
}
