using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PlanetMap
    {
        public int PlanetSizeX { get; protected set; }
        public int PlanetSizeY { get; protected set; }
        public IEnumerable<Coordinates> Obstacles { get; protected set; }

        public PlanetMap(int planetSizeX, int planetSizeY, IEnumerable<Coordinates> obstacles)
        {
            #region Validations

            //check a minimum and a maximum for the planet size, to avoid incorrect configurations.
            if (planetSizeX > 30 || planetSizeX < 1 || planetSizeY > 30 || planetSizeY < 1)
                throw new Exception("Invalid planet size. Each dimension should be between 1 and 30");

            // each obstacle should be within the range of the planet.
            foreach (var obstacle in obstacles)
            {
                if (obstacle.X > planetSizeX ||
                    obstacle.X < 0 ||
                    obstacle.Y > planetSizeY ||
                    obstacle.Y < 0)
                    throw new Exception($"The following obstacle is outside the planet map: X:{obstacle.X}, Y:{obstacle.Y}");
            }

            #endregion

            PlanetSizeX = planetSizeX;
            PlanetSizeY = planetSizeY;
            Obstacles = obstacles;
        }
    }
}
