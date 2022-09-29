using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class GeometryColor
    {
        public int Red;
        public int Green;
        public int Blue;
        public GeometryColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public static GeometryColor Between(GeometryColor c1, GeometryColor c2, double amount)
        {
            return new GeometryColor(
                (int)(c1.Red * amount + c2.Red * (1 - amount)),
                (int)(c1.Green * amount + c2.Green * (1 - amount)),
                (int)(c1.Blue * amount + c2.Blue * (1 - amount)));
        }
        public static int Difference(GeometryColor c1, GeometryColor c2)
        {
            return (int)Math.Sqrt(Math.Pow(c1.Red - c2.Red, 2) + Math.Pow(c1.Green - c2.Green, 2) + Math.Pow(c1.Blue - c2.Blue, 2));
        }
        public GeometryColor Darken(double percentOfFull)
        {
            percentOfFull = Math.Max(Math.Min(percentOfFull, 1), 0);

            return new GeometryColor((int)(Red * percentOfFull), (int)(Green * percentOfFull), (int)(Blue * percentOfFull));
        }
    }
}
