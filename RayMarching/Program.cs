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
using ConsoleRenderingFramework.ConsoleSpeedUp;
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
            Console.OutputEncoding = Encoding.UTF8;


            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(" ░▒▓");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("▒░ ");
            Console.WriteLine();


            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(" ");

            

            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine(Console.LargestWindowHeight);
            Console.WriteLine(Console.LargestWindowWidth);

            Console.ReadKey();
            Console.ResetColor();
            ColorUtil.Initialize();
            Console.ReadKey();


            for (int r = 0; r < 10; r++)
            {
                for (int g = 0; g < 10; g++)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        ColorUtil.GetRepresentation(r*25, g*25, b*25).PrintPixel();
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("|");
                    System.Threading.Thread.Sleep(1);
                }
            }
            ColorUtil.GetRepresentation(0, 0, 42).PrintPixel();
            ColorUtil.GetRepresentation(0, 0, 43).PrintPixel();
            ColorUtil.GetRepresentation(0, 0, 44).PrintPixel();
            ColorUtil.GetRepresentation(0, 0, 45).PrintPixel();
            ColorUtil.GetRepresentation(0, 0, 46).PrintPixel();


            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

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
            //width = Console.LargestWindowWidth-2;
            //height = Console.LargestWindowHeight-2;

            FastGMU gmu = new FastGMU(width + 2, height + 2);

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width - 2, height - 2, null);
            FullScreenManager fps = new FullScreenManager(width - 2, 1, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));
            mssm.AddScreen(fps, new System.Drawing.Rectangle(0, 0, width - 2, 1));
            gmu.PrintFrame();

            gmu.access.AddAdditonalBuffer();
            gmu.access.AddAdditonalBuffer();
            gmu.access.AddAdditonalBuffer();
            gmu.access.AddAdditonalBuffer();
            gmu.access.AddAdditonalBuffer();
            

            Camera c = GetCamera(GetGeometry());

            ConsoleKeyInfo input;

            double MoveDistance = 2;
            double rotateRad = Vector3.DegToRad(75);

            bool running = true;

            Stopwatch simTime = new Stopwatch();

            long lastFrame = simTime.ElapsedMilliseconds;
            long nextFrame;
            double frameSec;
            simTime.Start();

            double maxXLight = 15;
            double oneRotationX = 10_000;

            double maxYLight = 10;
            double oneRotationY = 13_500;

            long[] frameTimes = new long[100]; // should automatically initialize to only 0
            long totalMsLast100Frames = 0;
            long frameTimesIndex = 0;
            long knownFrames = 1;
            double fpsCalc = 1;


            PInfo fpsStyle = new PInfo().SetBg(ConsoleColor.Black).SetFg(ConsoleColor.White).SetC(' ');

            while (running)
            {
                //System.Threading.Thread.Sleep(20);

                nextFrame = simTime.ElapsedMilliseconds;
                frameSec = (nextFrame - lastFrame) / 1000D;
                // update total fps
                totalMsLast100Frames = totalMsLast100Frames - frameTimes[frameTimesIndex] + (nextFrame - lastFrame);
                frameTimes[frameTimesIndex] = (nextFrame - lastFrame);
                frameTimesIndex = ( frameTimesIndex + 1) % frameTimes.Length;
                knownFrames = Math.Min(++knownFrames, 100);
                fpsCalc = 1000D/((double)totalMsLast100Frames / ((double)knownFrames)); // these are frame times in ms --> 1000ms/s --> 1000/<average frametime>

                long bInput = simTime.ElapsedMilliseconds;

                #region input handle

                double speed = MoveDistance;

                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    speed = speed * 1.75d;
                }

                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    running = false;
                }

                if (Keyboard.IsKeyDown(Key.A))
                {
                    c.Position = c.Position + ((new Vector3(-c.ViewDirection.Z, 0, c.ViewDirection.X).AsNormalized()) * speed * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.S))
                {
                    c.Position = c.Position + ((new Vector3(c.ViewDirection.X, 0, c.ViewDirection.Z).AsNormalized()) * -speed * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.D))
                {
                    c.Position = c.Position + ((new Vector3(c.ViewDirection.Z, 0, -c.ViewDirection.X).AsNormalized()) * speed * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.W))
                {
                    c.Position = c.Position + ((new Vector3(c.ViewDirection.X, 0, c.ViewDirection.Z).AsNormalized()) * speed * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.Space))
                {
                    c.Position = c.Position + (new Vector3(0, 1, 0) * speed * frameSec);
                }


                if (Keyboard.IsKeyDown(Key.C))
                {
                    c.Position = c.Position + (new Vector3(0, -1, 0) * speed * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.Left))
                {
                    c.ViewDirection = c.ViewDirection.RotateY(-rotateRad * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.Down))
                {
                    double currAngle = c.ViewDirection.Angle.Y;
                    double target = currAngle + (rotateRad * frameSec);
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
                    c.ViewDirection = c.ViewDirection.RotateY(rotateRad * frameSec);
                }

                if (Keyboard.IsKeyDown(Key.Up))
                {
                    double currAngleI = c.ViewDirection.Angle.Y;
                    double targetI = currAngleI - (rotateRad * frameSec);
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

                #endregion

                long bSim = simTime.ElapsedMilliseconds;

                #region Simulation

                Vector3 newLight = new Vector3(maxXLight * (Math.Sin((nextFrame / oneRotationX) * Math.PI)), 10, maxYLight * (Math.Sin((nextFrame / oneRotationY) * Math.PI))) ;

                c.LightPosition = newLight;

                #endregion

                long aSim = simTime.ElapsedMilliseconds;

                #region rendering
                //Debug.WriteLine("before render");
                screen.App_DrawScreen(c.RenderImage(), 0, 0, null);
                //Debug.WriteLine("After render");

                long aDraw = simTime.ElapsedMilliseconds;

                fps.App_DrawScreen(BasicProvider.getInked(width - 2, 1, fpsStyle), 0, 0, null);
                fps.App_DrawScreen(
                    BasicProvider.TextToPInfo(
                        "FPS " + (fpsCalc).ToString("#.00") +
                        " Debug values: " + BasicProvider.ValuesToString(
                            bSim - bInput,
                            aSim - bSim,
                            aDraw - aSim
                            ), width - 2, 1, fpsStyle), 0, 0, null);
                gmu.PrintFrameAsync();
                //gmu.PrintFrame();
                #endregion

                lastFrame = nextFrame;
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
            geometries.Add(new Geometry(new Vector3(1, 1, 5), new GeometryProperty(new GeometryColor(255,0,0),0.6,0.4), Geometry.GType.Box));
            //geometries.Add(new Geometry(new Vector3(3, 0, 2), new GeometryProperty(new GeometryColor(255, 255, 0), 0.5, 0.5)));
            geometries.Add(new Geometry(new Vector3(-2, 1, 4), new GeometryProperty(new GeometryColor(0, 255, 0), 0.6, 0.4), Geometry.GType.Box));
            //geometries.Add(new Geometry(new Vector3(-1.5, -3, 4), new GeometryProperty(new GeometryColor(255, 0, 255), 0.5, 0.5)));
            geometries.Add(new Geometry(new Vector3(2, 4, -4), new GeometryProperty(new GeometryColor(0, 255, 255), 0.6, 0.4),Geometry.GType.Torus,new Vector3(3,1,3)));
            geometries.Add(new Geometry(new Vector3(1.5, -2, 4.5), new GeometryProperty(new GeometryColor(255, 0, 255), 0.6, 0.4)));


            geometries.Add(new Geometry(new Vector3(0, -8, 0), new GeometryProperty(new GeometryColor(255, 255, 255), 0.6, 0.4), Geometry.GType.Box, new Vector3(10, 0, 10)));
            return geometries;
        }

        public static Camera GetCamera(List<Geometry> geometries)
        {
            return new Camera(new Vector3(0, 2, 0), new Vector3(0, 0, 1), height, width, 110, geometries);
        }



    }
}
