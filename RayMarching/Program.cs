using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        static bool UseConsole = true;

        [STAThread]
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
            
            // TODO: custom size with scaled down image
            
            if (UseConsole)
            {
                RunInConsole();
            }
            else
            {
                RunInForms();
            }
            
            
            #region easyTesting
                        /*
                        

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

                        
                        */
                        //gmu.PrintFrame();
            #endregion


            Console.ReadLine();
        }

        public static void RunInConsole()
        {
            GMU gmu = new GMU(width + 2, height + 2, 0, 0);

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width - 2, height - 2, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));
            gmu.PrintFrame();
            

            Camera c = GetCamera(GetGeometry());

            ConsoleKeyInfo input;

            double MoveDistance = 0.2;
            double rotateRad = Vector3.DegToRad(5);

            bool running = true;

            
            while (running)
            {
                System.Threading.Thread.Sleep(20);

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

                Debug.WriteLine("before render");
                screen.App_DrawScreen(c.RenderImage(), 0, 0, null);
                Debug.WriteLine("After render");
                gmu.PrintFrame();
            }



        }
        
        public static void RunInForms()
        {
            GMUF gmu = new GMUF(height + 2, width + 2);
            

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width - 2, height - 2, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));
            gmu.PrintFrame();

            Camera c = GetCamera(GetGeometry());

            NotAConsoleWindow w = new NotAConsoleWindow();

            w.IdleHandle = ()=>OnIdleHandle(c,screen,gmu);
            Thread t = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.Run(w);
            });
            (gmu).SetRederingForm(w);
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            Console.WriteLine("Running In forms");

            t.Join();
        }

        public static void OnIdleHandle(Camera c, IRenderingApplication screen, GMU gmu)
        {
            ConsoleKeyInfo input;

            double MoveDistance = 0.2;
            double rotateRad = Vector3.DegToRad(5);

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

            Debug.WriteLine("before render");
            screen.App_DrawScreen(c.RenderImage(), 0, 0, null);
            Debug.WriteLine("After render");
            gmu.PrintFrame();
        }

        public static List<Geometry> GetGeometry()
        {
            List<Geometry> geometries = new List<Geometry>();
            geometries.Add(new Geometry(new Vector3(1, 1, 5), ConsoleColor.Red, Geometry.GType.Box));
            geometries.Add(new Geometry(new Vector3(3, 0, 2), ConsoleColor.Yellow));
            geometries.Add(new Geometry(new Vector3(-2, 1, 4), ConsoleColor.Green, Geometry.GType.Box));
            geometries.Add(new Geometry(new Vector3(-1.5, -3, 4), ConsoleColor.Magenta));
            geometries.Add(new Geometry(new Vector3(2, 4, -4), ConsoleColor.Yellow));
            geometries.Add(new Geometry(new Vector3(1.5, -2, 4.5), ConsoleColor.Cyan));


            geometries.Add(new Geometry(new Vector3(0, -8, 0), ConsoleColor.White, Geometry.GType.Box, new Vector3(10, 0, 10)));
            return geometries;
        }

        public static Camera GetCamera(List<Geometry> geometries)
        {
            return new Camera(new Vector3(0, 2, 0), new Vector3(0, 0, 1), height, width, 110, geometries);
        }



    }
}
