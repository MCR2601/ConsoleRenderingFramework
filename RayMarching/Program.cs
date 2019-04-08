using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ConsoleRenderingFramework;
using ConsoleRenderingFramework.BasicScreenManagerPackage;
using ConsoleRenderingFramework.RenderProviders;

namespace RayMarching
{
    class Program
    {
        static int height = 60;
        static int width = 120;
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Vector3 start = new Vector3(1, -1, 0);
            Console.WriteLine(start.Angle);
            //Console.ReadLine();

            Console.WriteLine("Alles bitte einstellen");

            Console.ReadLine();
            Console.Clear();

            //height = Console.WindowHeight - 2;
            //width = Console.WindowWidth - 2;

            //GMU gmu = new GMU(width + 2, height + 2,0,0);

            // TODO: custom size with scaled down image
            GMUF gmu = new GMUF(height+2, width+2);


            
            NotAConsoleWindow w = new NotAConsoleWindow();

            Thread t = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.Run(w);
            });
            (gmu as GMUF).SetRederingForm(w);


            t.SetApartmentState(ApartmentState.STA);
            w.EnableRender();
            t.Start();
            

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width-2, height-2, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width-2, height-2));

            //screen.App_DrawScreen(BasicProvider.TextToPInfo("Hello World!", 7, 3, new PInfo().SetBg(ConsoleColor.Gray).SetFg(ConsoleColor.Green)),5,5,screen);



            // Camera and objects
            List<Geometry> geometries = new List<Geometry>();
            geometries.Add(new Geometry(new Vector3(1, 1, 5), ConsoleColor.Red));
            geometries.Add(new Geometry(new Vector3(3, 0, 2), ConsoleColor.Yellow));
            geometries.Add(new Geometry(new Vector3(-2, 1, 4), ConsoleColor.Green));
            geometries.Add(new Geometry(new Vector3(-1.5, -3, 4), ConsoleColor.Magenta));
            geometries.Add(new Geometry(new Vector3(2, 4, -4), ConsoleColor.Yellow));
            geometries.Add(new Geometry(new Vector3(1.5, -2, 4.5), ConsoleColor.Cyan));


            Camera c = new Camera(new Vector3(0,2,0), new Vector3(0, 0, 1), height, width, 110, geometries);
            
            
            screen.App_DrawScreen(c.RenderImage(), 0, 0, null);
            gmu.PrintFrame();

            ConsoleKeyInfo input;

            double MoveDistance = 0.2;
            double rotateRad = Vector3.DegToRad(5);

            bool running = true;

            Thread thread = new Thread(()
                =>
            {
                while (running)
                {
                    System.Threading.Thread.Sleep(50);

                    if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        running = false;
                    }

                    if (Keyboard.IsKeyDown(Key.A))
                    {
                        c.Position = c.Position + ((new Vector3(-c.ViewDirection.Z, 0, c.ViewDirection.X).AsNormalized()) * MoveDistance);
                    }

                    if (Keyboard.IsKeyDown(Key.S))
                    {
                        c.Position = c.Position + ((new Vector3(c.ViewDirection.X, 0, c.ViewDirection.Z).AsNormalized()) * -MoveDistance);
                    }

                    if (Keyboard.IsKeyDown(Key.D))
                    {
                        c.Position = c.Position + ((new Vector3(c.ViewDirection.Z, 0, -c.ViewDirection.X).AsNormalized()) * MoveDistance);
                    }

                    if (Keyboard.IsKeyDown(Key.W))
                    {
                        c.Position = c.Position + ((new Vector3(c.ViewDirection.X, 0, c.ViewDirection.Z).AsNormalized()) * MoveDistance);
                    }

                    if (Keyboard.IsKeyDown(Key.Space))
                    {
                        c.Position = c.Position + (new Vector3(0, 1, 0) * MoveDistance);
                    }


                    if (Keyboard.IsKeyDown(Key.C))
                    {
                        c.Position = c.Position + (new Vector3(0, -1, 0) * MoveDistance);
                    }

                    if (Keyboard.IsKeyDown(Key.Left))
                    {
                        c.ViewDirection = c.ViewDirection.RotateY(-rotateRad);
                    }

                    if (Keyboard.IsKeyDown(Key.Down))
                    {
                        double currAngle = c.ViewDirection.Angle.Y;
                        double target = currAngle + rotateRad;
                        if (target < Math.PI)
                        {
                            // we can change the angle

                            double realAngle = Math.Abs(target - (Math.PI / 2));

                            double realHeight = Math.Tan(realAngle);
                            double corrected = realHeight * Math.Sqrt(c.ViewDirection.X * c.ViewDirection.X + c.ViewDirection.Z * c.ViewDirection.Z);

                            c.ViewDirection.Y = corrected;

                            if (target > Math.PI / 2)
                            {
                                // negative
                                c.ViewDirection.Y = c.ViewDirection.Y * -1;
                            }
                        }
                    }


                    if (Keyboard.IsKeyDown(Key.Right))
                    {
                        c.ViewDirection = c.ViewDirection.RotateY(rotateRad);
                    }

                    if (Keyboard.IsKeyDown(Key.Up))
                    {
                        double currAngleI = c.ViewDirection.Angle.Y;
                        double targetI = currAngleI - rotateRad;
                        if (targetI < Math.PI)
                        {
                            // we can change the angle

                            double realAngle = Math.Abs(targetI - (Math.PI / 2));

                            double realHeight = Math.Tan(realAngle);
                            double corrected = realHeight * Math.Sqrt(c.ViewDirection.X * c.ViewDirection.X + c.ViewDirection.Z * c.ViewDirection.Z);

                            c.ViewDirection.Y = corrected;

                            if (targetI > Math.PI / 2)
                            {
                                // negative
                                c.ViewDirection.Y = c.ViewDirection.Y * -1;
                            }
                        }
                    }


                    screen.App_DrawScreen(c.RenderImage(), 0, 0, null);
                    gmu.PrintFrame();
                }
            });
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            t.Join();
            

            /*
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

            #region rotation Testing
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

            #endregion
            */
            //gmu.PrintFrame();



            Console.ReadLine();
        }
    }
}
