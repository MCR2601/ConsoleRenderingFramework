using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace Doom
{
    class Object3D : IRenderable
    {
        //public string name;
        public bool isTransparent { get ; set; }
        /// <summary>
        /// this is the pattern for the object
        /// it will repeat after it has finished, perfect for walls
        /// </summary>
        public PInfo[,] pattern;
        public double patternwidth = 1;
        public double patternheight = 1;

        public Object3D()
        {

        }

        public virtual PInfo[,] Render(double Dist, int ScreenHeight, double PlayerHeight, double roomHeight, double DistFromLeft)
        {
            double updown = 30 * Math.PI / 180;
            
            // calculate the parts of top and bottom
            double upHigh = roomHeight - PlayerHeight;

            // calculate what left
            DistFromLeft = DistFromLeft % patternwidth;
            double side = DistFromLeft * pattern.GetLength(0);


            double unseenTop = Math.Tan(updown) * upHigh;
            double unseenBot = Math.Tan(updown) * PlayerHeight;

            double angleMidTop = Math.Atan(upHigh / Dist );
            double angleMidBot = Math.Atan(PlayerHeight / Dist );
            double angleTop = (90 * Math.PI / 180) - unseenTop - angleMidTop;
            double angleBot = (90 * Math.PI / 180) - unseenBot - angleMidBot;

            double halfheight = ScreenHeight / 2;

            int Bottom = (int)Math.Round((angleTop / (angleTop + angleMidTop)) * halfheight);
            int Mid = (int)Math.Round(((angleMidTop / (angleTop + angleMidTop)) * halfheight) + ((angleMidBot / (angleBot + angleMidBot)) * halfheight));
            int Top = (int)Math.Round((angleBot / (angleBot + angleMidBot)) * halfheight);

            while (Top + Mid + Bottom < ScreenHeight)
            {
                Mid++;
            }

            double pixelPerHeight = roomHeight / Mid;


            #region create strip
            //int notwall = (height - mid.Item1 ) / 2;
            PInfo[,] data = new PInfo[1, ScreenHeight];
            data.Populate();
            for (int y = 0; y < ScreenHeight; y++)
            {
                if (y < Bottom)
                {
                    data[0, y].Override(new PInfo().SetBg(ConsoleColor.Gray));
                }
                else
                {
                    if (y < Bottom + Mid)
                    {
                        // calculate what pixels
                        double up = ((y - Bottom) * pixelPerHeight ) % patternheight;
                        
                        data[0, y].Override(pattern[(int)Math.Floor(side), (int)Math.Floor(up) ]);
                    }
                    else
                    {
                        data[0, y].Override(new PInfo().SetBg(ConsoleColor.DarkGray));
                    }
                }
            }
            #endregion


            return data;
        }
    }
}