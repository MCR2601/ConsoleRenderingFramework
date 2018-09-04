using ConsoleRenderingFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicScreenManagerPackage;
using BasicRenderProviders;

namespace TestingProj
{
    class Program
    {
        static void Main(string[] args)
        {
            //Image img = Image.FromFile("test.file");
            

            Console.WriteLine(Console.WindowWidth);
            Console.WriteLine(Console.WindowHeight);

            GMU gmu = new GMU(40, 100);
            
            Console.WriteLine(Console.LargestWindowWidth);
            Console.WriteLine(Console.LargestWindowHeight);

            MultiSplitScreenManager msc = new MultiSplitScreenManager(gmu.PlacePixels,40,100);

            FullScreenManager fs1 = new FullScreenManager(50, 40, null);
            FullScreenManager fs2 = new FullScreenManager(50, 40, null);


            msc.AddScreen(fs1, new Rectangle(0, 0, 50, 40));
            msc.AddScreen(fs2, new Rectangle(50,0,50,40));

            Console.ReadKey();
            //gmu.PlacePixels(simpleSquare, 20, 8, null);

            fs1.App_DrawScreen(BasicProvider.getInked(48, 38, new PInfo(' ', ConsoleColor.White, ConsoleColor.Red)), 1, 1, null);
            fs2.App_DrawScreen(BasicProvider.getInked(48, 38, new PInfo(' ', ConsoleColor.White, ConsoleColor.Blue)), 1, 1, null);

            gmu.PrintFrame();

            Console.ReadLine();
        }

        public static PInfo[,] simpleSquare = new PInfo[,]{
                { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(),
                new PInfo(),
                new PInfo(),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(),
                new PInfo(),
                new PInfo(),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(),
                new PInfo(),
                new PInfo(),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(),
                new PInfo(),
                new PInfo(),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)
            },
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)}
            };
        
    }
}
