using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework
{
    /// <summary>
    /// This class provides integer bounds for screens and squares
    /// it only uses the top left point, width and height to store all the information
    /// </summary>
    public class Bounds
    {
        /// <summary>
        /// x-coordinate of the top left corner
        /// </summary>
        public int x;
        /// <summary>
        /// y-coordinate of the top left corner
        /// </summary>
        public int y;
        /// <summary>
        /// the width of the rectangle, representing these bounds
        /// </summary>
        public int width;
        /// <summary>
        /// the heigth of the rectangle, representing these bounds
        /// </summary>
        public int height;
        
        public Bounds(int x,int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }

        

    }
}
