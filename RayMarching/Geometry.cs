using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class Geometry
    {
        public Vector3 Position;

        public GType Type;

        public ConsoleColor Color;

        public enum GType
        {
            Sphere,
            Box,
            Torus
        };

        public virtual double GetDistance(Vector3 target)
        {
            return (Position - target).Length;
        }



    }
}
