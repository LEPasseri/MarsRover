using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Coordinates
    {
        public int X {  get; protected set; }
        public int Y { get; protected set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
