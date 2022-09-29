using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class GeometryProperty
    {
        public GeometryColor Color;
        public double DiffuseConstant;
        public double AmbientConstant;

        public GeometryProperty(GeometryColor color, double diffuseConstant, double ambientConstant)
        {
            Color = color;
            DiffuseConstant = diffuseConstant;
            AmbientConstant = ambientConstant;
        }
    }
}
