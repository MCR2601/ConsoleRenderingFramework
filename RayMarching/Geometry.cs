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
            return DistanceCalculator.SphereDist(target, Position);
        }

        public Geometry(Vector3 position, ConsoleColor color)
        {
            Position = position;
            Type = GType.Sphere;
            Color = color;
        }
    }
}
