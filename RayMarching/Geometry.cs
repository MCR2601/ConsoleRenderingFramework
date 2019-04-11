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

        public Vector3 Size;

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
                    return DistanceCalculator.BoxDist(target, Position, Size);
                    break;
                case GType.Torus:
                    return DistanceCalculator.TorusDist(target, Position, Size);
                    break;
                default:
                    break;
            }

            return DistanceCalculator.SphereDist(target, Position);
        }

        public Geometry(Vector3 position, ConsoleColor color, GType type = GType.Sphere, Vector3 size = null )
        {
            Position = position;
            Type = GType.Sphere;
            Color = color;
            Type = type;
            if (size == null)
            {
                Size = new Vector3(1, 1, 1);
            }
            else
            {
                Size = size;
            }
        }
    }
}
