using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Diagnostics;

namespace Doom
{
    class ObjectSprite : IRenderable
    {
        /// <summary>
        /// just a name
        /// it doesnt do anything, just helps with differentiating for the user
        /// </summary>
        public string Name;
        /// <summary>
        /// If this is the last visionblocker
        /// </summary>
        public bool isTransparent { get; set; }
        /// <summary>
        /// The image for this object
        /// </summary>
        public PInfo[,] Sprite;

        public double Width { get { return Sprite.GetLength(0); } }

        public ObjectSprite()
        {

        }


        public virtual PInfo[,] Render(double Dist, int ScreenHeight, double PlayerHeight, double RoomHeight, double DistFromLeft)
        {
            double updown = 30 * Math.PI / 180;
            // calculate required height of sprite
            // TODO: Mage the system "realistic" things further up will be visible differently than headon
            // currently only spreading the pixels over the calculated are
            

            double upHigh = RoomHeight - PlayerHeight;




            double unseenTop = Math.Tan(updown) * upHigh;
            double unseenBot = Math.Tan(updown) * PlayerHeight;

            double angleMidTop = Math.Atan(upHigh / Dist);
            double angleMidBot = Math.Atan(PlayerHeight / Dist);
            double angleTop = (90 * Math.PI / 180) - unseenTop - angleMidTop;
            double angleBot = (90 * Math.PI / 180) - unseenBot - angleMidBot;

            double halfheight = ScreenHeight / 2;

            double Top = (int)Math.Round((angleTop / (angleTop + angleMidTop)) * halfheight);
            double Mid = (int)Math.Round(((angleMidTop / (angleTop + angleMidTop)) * halfheight) + ((angleMidBot / (angleBot + angleMidBot)) * halfheight));
            double Bottom = (int)Math.Round((angleBot / (angleBot + angleMidBot)) * halfheight);
            while (Top + Mid + Bottom < ScreenHeight)
            {
                Mid++;
            }
            // now we have the height of our image
            

            // then return one by one the pixels of the sprite equivalent to the pixel on screen
            PInfo[,] data = new PInfo[1, ScreenHeight];
            data.Populate();
            double scale = Sprite.GetLength(1)/Mid * 1.0;

            int column = (int)((Sprite.GetLength(0)/Width) * DistFromLeft);

            for (int i = 0; i < Mid; i++)
            {
                // set pixel to the the right image
                data[0, i + (int)Bottom] = Sprite[column, (int)Math.Floor((i) * scale)];
                
            }
            //Debug.WriteLine("i showed a sprite");
            return data;
        }
    }
}
