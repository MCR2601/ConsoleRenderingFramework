using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using ConsoleRenderingFramework.BasicScreenManagerPackage;
using ConsoleRenderingFramework.RenderProviders;

namespace RayMarching
{
    class Program
    {
        static int height = 40;
        static int width = 100;
        static void Main(string[] args)
        {
            // TODO: custom size with scaled down image
            GMU gmu = new GMU(width + 2, height + 2, 0, 0);

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width+2, height+2);
            FullScreenManager screen = new FullScreenManager(width, height, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width, height));

            screen.App_DrawScreen(BasicProvider.TextToPInfo("Hello World!", 7, 3, new PInfo().SetBg(ConsoleColor.Gray).SetFg(ConsoleColor.Green)),5,5,screen);

            

            #region easyTesting

            Vector3 test = new Vector3(1, 1, 0);
            Vector3 testRotated = test.RotateZ(Vector3.DegToRad(90));
            AngleVector testAngles = test.Angle;
            AngleVector RotatedAngles = testRotated.Angle;

            Console.WriteLine("Test");
            Console.WriteLine(test);
            Console.WriteLine(testAngles);
            Console.WriteLine("Rotated");
            Console.WriteLine(testRotated);
            Console.WriteLine(RotatedAngles);
            Console.WriteLine("-------");

            #endregion


            Vector3 desired = new Vector3(1, 1, 0);

            Vector3 existing = new Vector3(0, 1, 0);

            AngleVector anglesEx = existing.Angle;
            AngleVector anglesDes = desired.Angle;

            Vector3 resultY = desired.RotateX(anglesEx.Theta - Vector3.DegToRad(90));
            Vector3 resultX = resultY.RotateY(-(anglesEx.Phi - (existing.X==0? 0: Vector3.DegToRad(90))));

            Console.WriteLine("Desired");
            Console.WriteLine(desired);
            Console.WriteLine(anglesDes);
            Console.WriteLine("Existing");
            Console.WriteLine(existing);
            Console.WriteLine(anglesEx);
            Console.WriteLine("Results");
            Console.WriteLine("Y"+resultY +"\t\t"+ resultY.Angle);
            Console.WriteLine("X" + resultX+"\t\t" + resultY.Angle);

            //gmu.PrintFrame();



            Console.ReadLine();
        }
    }
}
