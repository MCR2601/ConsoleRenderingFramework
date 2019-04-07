using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class AngleVector : Vector3
    {
        public double Phi; // top to bottom
        public double Theta; // from X axis

        public AngleVector(double x, double y, double z, double phi, double theta) : base(x, y, z)
        {
            Phi = phi;
            Theta = theta;
        }

        public override string ToString()
        {
            return "[X:" + X + "|Y:" + Y + "|Z:" + Z + "|Theta: " + Theta + "|Phi: " + Phi + "]";
        }
    }
}
