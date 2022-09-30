using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class RayHit
    {
        public Vector3 Position;
        public Geometry Object;
        public Vector3 Normal;
        public Vector3 ViewDirection;
        public Vector3 Reflection;

        public bool InShade;


        public RayHit(Vector3 position, Geometry @object, Vector3 normal, Vector3 viewDirection, Vector3 reflection, bool inShade = false)
        {
            Position = position;
            Object = @object;
            Normal = normal;
            ViewDirection = viewDirection;
            Reflection = reflection;
            InShade = inShade;
        }
    }
}
