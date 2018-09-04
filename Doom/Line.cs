using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doom
{
    struct Line
    {
        public double x_1;
        public double y_1;
        public double x_2;
        public double y_2;
        public Line(double x1, double y1, double x2, double y2)
        {
            x_1 = x1;
            y_1 = y1;
            x_2 = x2;
            y_2 = y2;
        }
    }
}
