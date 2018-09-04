using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace Doom
{
    /// <summary>
    /// this provides methodes that allow easy rendering a screen with specific splitting of color
    /// </summary>
    public static class SplitRenderer
    {
        public static PInfo[,] getColoredStrip(int height, Tuple<Tuple<int,int,int>,PInfo> mid,PInfo top, PInfo bottom)
        {
            //int notwall = (height - mid.Item1 ) / 2;
            PInfo[,] data = new PInfo[1, height];
            data.Populate();
            for (int y = 0; y < height; y++)
            {
                if (y < mid.Item1.Item1)
                {
                    data[0, y].Override(top);
                }
                else
                {
                    if (y < mid.Item1.Item1 + mid.Item1.Item2)
                    {
                        data[0, y].Override(mid.Item2);
                    }
                    else
                    {
                        data[0, y].Override(bottom);
                    }
                }
            }   

            return data;
        }
    }
}
