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

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            GMU gmu = new GMU(100, 40, 0, 0);
            
            MultiSplitScreenManager msc = new MultiSplitScreenManager(gmu.PlacePixels,40,100);


            FullScreenManager full = new FullScreenManager(40, 100,null);

            msc.AddScreen(full, new Rectangle(0, 0, 40, 100));

            FullScreenManager fs1 = new FullScreenManager(50, 40, null);
            FullScreenManager fs2 = new FullScreenManager(50, 40, null);

           

            full.App_DrawScreen(BasicProvider.getInked(4, 100, new PInfo().SetBg(ConsoleColor.Black).SetFg(ConsoleColor.White)), 0, 0, null);

            gmu.PrintFrame();

            full.App_DrawScreen(BasicProvider.TextToPInfo("Test", 20, 20, new PInfo().SetFg(ConsoleColor.White)),3,3,null);

            gmu.PrintFrame();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Console.WindowWidth);
            Console.WriteLine(Console.WindowHeight);

            Console.WriteLine(Console.LargestWindowWidth);
            Console.WriteLine(Console.LargestWindowHeight);

            msc.AddScreen(fs1, new Rectangle(0, 0, 50, 40));
            msc.AddScreen(fs2, new Rectangle(50,0,50,40));

            Console.ReadKey();
            gmu.PlacePixels(simpleSquare, 20, 8, null);

            fs1.App_DrawScreen(BasicProvider.getInked(48, 38, new PInfo(' ', ConsoleColor.White, ConsoleColor.Red)), 1, 1, null);
            fs2.App_DrawScreen(BasicProvider.getInked(48, 38, new PInfo(' ', ConsoleColor.White, ConsoleColor.Blue)), 1, 1, null);

            fs1.App_DrawScreen(simpleSquare, 1, 1, null);

            fs2.App_DrawScreen(BasicProvider.TextToPInfo("I try this new thing with text", 10, 10, new PInfo().SetFg(ConsoleColor.Black)), 3, 5, null);
            fs2.App_DrawScreen(BasicProvider.TextToPInfo("I try this new thing with text", 10, 10, new PInfo().SetFg(ConsoleColor.Red)), 3, 2, null);


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
                new PInfo(' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo(' '  , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo(' ', ConsoleColor.White, ConsoleColor.White)},
            { new PInfo(' ', ConsoleColor.White, ConsoleColor.White),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
                new PInfo( ' ' , ConsoleColor.Black, ConsoleColor.Black),
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
