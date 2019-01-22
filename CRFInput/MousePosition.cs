using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.Input
{
    public class MousePosition
    {
        public int X;
        public int Y;
        public int W;
        public int H;

        public MousePosition(int x, int y, int w, int h) : this(x, y)
        {
            W = w;
            H = h;
        }

        public MousePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
