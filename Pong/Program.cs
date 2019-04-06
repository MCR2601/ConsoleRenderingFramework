using ConsoleRenderingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Gma.System.MouseKeyHook;
using System.Windows.Input;
using System.Threading;
using ConsoleRenderingFramework.BasicScreenManagerPackage;
using ConsoleRenderingFramework.RenderProviders;

namespace Pong
{
    class Program
    {
        // how much the ball will move up or down
        static double Angle = 1.0d;
        // how much the ball moves right or left
        static double Forward = 1d;
        // how far it has to move for one tile forward
        static double distance = Math.Sqrt(2);

        static IKeyboardEvents m_GlobalHook = Hook.GlobalEvents();


        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            GMU gmu = new GMU(120, 40, 0, 0);

            gmu.PlacePixels(BasicProvider.getInked(120, 60, new PInfo().SetBg(ConsoleColor.Black)), 0, 0, null);
            gmu.PrintFrame();

            MultiSplitScreenManager Screen = new MultiSplitScreenManager(gmu.PlacePixels, 120, 60);

            

            WindowScreenManager p1Thing = new WindowScreenManager(1, 4, Screen.App_DrawScreen,new PInfo().SetBg(ConsoleColor.Gray).SetC('\u2656').SetFg(ConsoleColor.Blue));
            WindowScreenManager p2Thing = new WindowScreenManager(1, 4, Screen.App_DrawScreen,new PInfo().SetBg( ConsoleColor.Gray).SetC('\u2656').SetFg(ConsoleColor.Blue));
            WindowScreenManager background = new WindowScreenManager(120, 40, Screen.App_DrawScreen,new PInfo().SetBg(ConsoleColor.Black));

            WindowScreenManager bThing = new WindowScreenManager(1, 1, Screen.App_DrawScreen, new PInfo().SetBg(ConsoleColor.White));

            


            Screen.AddScreen(p1Thing, new Rectangle(1, 18, 1, 4),1);
            Screen.AddScreen(p2Thing, new Rectangle(118, 18, 1, 4),1);
            Screen.AddScreen(background, new Rectangle(0, 0, 120, 40),0);

            bool running = true;


            Point Ball = new Point(40, 20);


            Thread thread = new Thread(() => {
                while (running)
                {

                    System.Threading.Thread.Sleep(10);

                    int p1Move = 0;
                    int p2Move = 0;
                    #region input handeling
                    if (Keyboard.IsKeyDown(Key.W))
                    {
                        p1Move += -1;
                    }
                    if (Keyboard.IsKeyDown(Key.S))
                    {
                        p1Move += 1;
                    }
                    if (Keyboard.IsKeyDown(Key.Up))
                    {
                        p2Move += -1;
                    }
                    if (Keyboard.IsKeyDown(Key.Down))
                    {
                        p2Move += 1;
                    }
                    if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        running = false;
                    }
                    if (Keyboard.IsKeyDown(Key.Space))
                    {

                    }
                    #endregion

                    #region movement

                    #endregion
                    #region score

                    #endregion
                    #region Time

                    #endregion

                    Screen.Render();
                    gmu.PrintFrame();
                }
            });
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            
            

            Screen.Render();
            gmu.PrintFrame();

            Console.ReadKey();
        }
        public static void CalculateDistance()
        {
            distance = Math.Sqrt(Forward * Forward + Angle * Angle);
        }
    }
}
