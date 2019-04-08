using ConsoleRenderingFramework;
using ConsoleRenderingFramework.BasicScreenManagerPackage;
using RayMarching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReRayMarching
{
    class Program
    {
        static int height = 60;
        static int width = 120;

        static void Main(string[] args)
        {

            GMU gmu = new GMU(width + 5, height + 5, 1, 1);

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width - 2, height - 2, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));

            //screen.App_DrawScreen(BasicProvider.TextToPInfo("Hello World!", 7, 3, new PInfo().SetBg(ConsoleColor.Gray).SetFg(ConsoleColor.Green)),5,5,screen);



            // Camera and objects
            List<Geometry> geometries = new List<Geometry>();
            geometries.Add(new Geometry(new Vector3(1, 1, 5), ConsoleColor.Red));
            geometries.Add(new Geometry(new Vector3(3, 0, 2), ConsoleColor.Yellow));
            geometries.Add(new Geometry(new Vector3(-2, 1, 4), ConsoleColor.Green));

            Camera c = new Camera(new Vector3(0, 2, 0), new Vector3(0, 0, 1), height-2, width-2, 110, geometries);

            PInfo[,] image = c.RenderImage();

            screen.App_DrawScreen(image, 0, 0, null);

            gmu.PrintFrame();

            //Console.ReadLine();
            Debug.WriteLine("first frame");
            System.Threading.Thread.Sleep(5000);
            c.ViewDirection = new Vector3(1, 0, 1);
            Debug.WriteLine("Direction Change");
            System.Threading.Thread.Sleep(2000);

            PInfo[,] data = c.RenderImage();
            Debug.WriteLine("render");
            System.Threading.Thread.Sleep(2000);
            screen.App_DrawScreen(data, 0, 0, screen);
            Debug.WriteLine("Screen");
            System.Threading.Thread.Sleep(2000);
            gmu.PrintFrame();
            Debug.WriteLine("PrintFrame");





            Console.ReadLine();
        }
    }
}
