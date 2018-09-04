using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework
{
    /// <summary>
    /// This is the Graphical Management Unit (GMU)
    /// it provides Everything you need to print to the console
    /// </summary>
    public class GMU
    {
        protected int height;
        protected int width;

        protected PInfo[,] ScreenBuffer;
        //PInfo[,] Screen;
        /// <summary>
        /// Just creates new Arrays
        /// </summary>
        protected void CreateScreen()
        {
            ScreenBuffer = new PInfo[width,height];
            //Screen = new PInfo[width, height];
            ScreenBuffer.Populate(new PInfo() { hasBackground = false, hasCharacter = false, hasForeground = false });
            //Screen.Populate(new PInfo() { hasBackground = false, hasCharacter = false, hasForeground = false });
        }


        /// <summary>
        /// creates a new GMU and creates a rendering Area with speciffic size
        /// </summary>
        /// <param name="h">height in characters</param>
        /// <param name="w">width in characters</param>
        public GMU(int h,int w)
        {
            if (h>Console.LargestWindowHeight||w>Console.LargestWindowWidth)
            {
                throw new ArgumentException("The size was greater than 50");
            }
            height = h;
            width = w;
            Console.SetWindowSize(1,1);            
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width,height);
            CreateScreen();
        }
        /// <summary>
        /// creates a new GMU and creates a rendering Area from current Console size
        /// </summary>
        public GMU()
        {
            height = Console.LargestWindowHeight;
            width = Console.LargestWindowWidth;
            CreateScreen();
        }

        /// <summary>
        /// Processes all Pixels
        /// Prints all new Pixels to the Console
        /// Moves new Screen to the Screen
        /// </summary>
        public void PrintFrame()
        {
            //List<PositionedPInfo> toPrint = new List<PositionedPInfo>();
            int changedPixels = 0;
            //TODO: Batching, grouping changes together to avoid changing color
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //if (Screen[x, y] != ScreenBuffer[x, y])
                    //{
                    if (ScreenBuffer[x, y].isChanged == true)
                    {
                        PrintPixel(y, x);
                        ScreenBuffer[x, y].isChanged = false;
                        changedPixels++;
                    }

                    //}
                }
            }
            Debug.WriteLine(changedPixels);
            //Screen = ScreenBuffer;
        }

        virtual protected void PrintPixel(int y, int x)
        {
            Console.SetCursorPosition(x, y);
            ScreenBuffer[x, y].PrintPixel();
        }

        /// <summary>
        /// Placing an array into the Screenbuffer at a specific location
        /// </summary>
        /// <param name="pxls">2-Dim Array of PixelInfos</param>
        /// <param name="xPos">x Position of the top left corner</param>
        /// <param name="yPos">y position of the top left corner</param>
        public void PlacePixels(PInfo[,] pxls,int xPos, int yPos,IRenderingApplication sender)
        {
            // write to log

            int xlength = pxls.GetLength(0);
            int ylength = pxls.GetLength(1);

            int xScreenlength = ScreenBuffer.GetLength(0);
            int yScreenlength = ScreenBuffer.GetLength(1);

            for (int x = xPos; x < xPos+xlength; x++)
            {
                for (int y = yPos; y < yPos+ylength; y++)
                {
                    if (x < xScreenlength && y < yScreenlength)
                    {
                        ScreenBuffer[x, y].Override (pxls[x - xPos, y - yPos]);
                    }                    
                }
            }
        }
        
        

    }
}
