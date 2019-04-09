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
            switch (Type)
            {
                case GType.Sphere:
                    return DistanceCalculator.SphereDist(target, Position);
                    break;
                case GType.Box:
                    return DistanceCalculator.BoxDist(target, Position, new Vector3(1, 1, 1));
                    break;
                case GType.Torus:
                    return DistanceCalculator.TorusDist(target, Position, new Vector3(1, 1, 1));
                    break;
                default:
                    break;
            }

            return DistanceCalculator.SphereDist(target, Position);
        }

        public Geometry(Vector3 position, ConsoleColor color, GType type = GType.Sphere)
        {
            Position = position;
            Type = GType.Sphere;
            Color = color;
            Type = type;
        }
    }
}
