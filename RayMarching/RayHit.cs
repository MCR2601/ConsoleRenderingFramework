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

        public RayHit(Vector3 position, Geometry o)
        {
            Position = position;
            Object = o;
        }
    }
}
