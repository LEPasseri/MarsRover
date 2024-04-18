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

        public void InitializeRover()
        {
            Console.WriteLine("Initializing Rover..");

            SendPlanetInformation();

            SendRoverLandingPosition();

        }

        public void ChartRoverRoute()
        {
            Console.WriteLine("Input the Rover's route step by step, including turns. (accepted instructions: f,b,l,r - delimeter: ,)\n");

            try
            {
                var routeSteps = Console.ReadLine()?.ToLower().Split(',');

                if (routeSteps == null)
                    throw new Exception("No steps were provided for the route");

                var routeOutcome = MarsRoverController.ChartRoute(routeSteps);

                Console.WriteLine("The Rover moved succesfully along the designated route. This is it's final position:\n" +
                    $"X: {routeOutcome.x}\n" +
                    $"Y: {routeOutcome.x}\n" +
                    $"Direction: {routeOutcome.direction}\n\n"
                    );
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"An error has occurred while charting the Rover's route: {ex.Message} - Try again with a different route. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                ChartRoverRoute();
            }
        }

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

                var obstacles = obstaclesCoordinatesConfigValue.Select(coordinates => new Tuple<int, int>(Convert.ToInt32(coordinates.Split(',')[0]), Convert.ToInt32(coordinates.Split(',')[1])));

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
                Console.Write($"An error has occurred while initializing the Rover: {ex.Message} - The application will now close. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }
        }

        private void SendRoverLandingPosition()
        {
            Console.WriteLine("Which are the Rover's landing coordinates, and which cardinal direction is it facing? (expected format: x,y,d)\n");

            try
            {
                var rawLandingPosition = Console.ReadLine()?.Split(',');

                if (rawLandingPosition == null)
                    throw new Exception("Tee landing coordinates and the direction must be provided in this format: x,y,d ");

                MarsRoverController.SetRoverLandingPosition(x: Convert.ToInt32(rawLandingPosition[0]), y: Convert.ToInt32(rawLandingPosition[1]), directionLabel: rawLandingPosition[2].ToCharArray()[0]);

                Console.WriteLine("Rover's landing information set\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"There was an error while setting the Rover's initial position: {ex.Message} - Try again with a different value. \n\n");
                Console.ForegroundColor = ConsoleColor.White;
                SendRoverLandingPosition();
            }
        }


    }
}
