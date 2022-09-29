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

        public GeometryProperty Properties;

        public Vector3 Size;
        public AngleVector Rotation;

        public enum GType
        {
            Nothing,
            Sphere,
            Box,
            Torus
        };
        public enum ChildType
        {
            Union,
            SmoothUntion,
            Subtraction,
            SmootSubtraction,
            Intersection,
            SmootIntersection
        }

        public Geometry Child;
        public ChildType ChildRelation;
        public float Smoothing;



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
        private const double EPSILON = 0.01;
        public Vector3 GetNormal(Vector3 p)
        {
            return new Vector3(
                GetDistance(new Vector3(p.X + EPSILON, p.Y, p.Z)) - GetDistance(new Vector3(p.X - EPSILON, p.Y, p.Z)),
                GetDistance(new Vector3(p.X, p.Y + EPSILON, p.Z)) - GetDistance(new Vector3(p.X, p.Y - EPSILON, p.Z)),
                GetDistance(new Vector3(p.X, p.Y, p.Z + EPSILON)) - GetDistance(new Vector3(p.X, p.Y, p.Z - EPSILON))
                ).Normalize();
        }

        public Geometry(Vector3 position, GeometryProperty prop, GType type = GType.Sphere, Vector3 size = null )
        {
            Position = position;
            Type = GType.Sphere;
            Type = type;
            Properties = prop;
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
