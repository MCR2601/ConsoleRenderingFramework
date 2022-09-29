using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace ConsoleRenderingFramework.ConsoleSpeedUp
{
    public class FastGMU : GMU
    {
        public DirectConsoleAccess access;

        public FastGMU(int w, int h,int xOffset = 0,int yOffset = 0)
        {
            height = h;
            width = w;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
            
                       
            access = new DirectConsoleAccess(width, height, xOffset, yOffset);
            
            CreateScreen();
            ScreenBuffer.Populate(new PInfo().SetC(' ').SetBg(ConsoleColor.Red).SetFg(ConsoleColor.Blue));
            //access.PrintBuffer(ScreenBuffer);
        }
        public override void PrintFrame()
        {

            bool result = access.PrintBuffer(ScreenBuffer);
            if (!result)
            {
                
            }
        }
        public bool PrintBuffer(DirectConsoleAccess.CharInfo[] buffer, int width, int height)
        {
            return access.PrintBuffer(buffer,width,height);            
        }
    }
}
