using ConsoleRenderingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace RayMarching
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 ViewDirection;

        public double AngleLenght = 10;

        public double MaxLength = 50;
        public double LightHeight = 10;

        public int ScreenHeight;
        public int ScreenWidth;

        PInfo[,] ScreenBuffer;

        public double FOV; // in degree

        public double MarchPrecission = 0.05;

        public List<Geometry> Objects;

        public Vector3 LightPosition;
        public double LightAmbient;
        public double LightDiffuse;

        public ConsoleColor DefaultColor = ConsoleColor.Blue;


        public Camera(Vector3 position, Vector3 viewDirection, int screenHeight, int screenWidth, double fov, List<Geometry> objects)
        {
            ColorUtil.Initialize();
            Position = position;
            ViewDirection = viewDirection;
            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;
            FOV = fov;
            Objects = objects;
            ScreenBuffer = new PInfo[screenWidth, screenHeight];
            LightPosition = new Vector3(10, 10, 0);
            LightAmbient = 1;
            LightDiffuse = 1;
        }

        public Vector3[,] GetRays()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Vector3[,] pixels = new Vector3[ScreenWidth, ScreenHeight];

            // calculate the bottom left pixel

            // we rotate left and down for that 

            double radPerPixelWidth = Vector3.DegToRad(FOV) / ScreenWidth;
            double radPerPixelHeight = Vector3.DegToRad(FOV) / ScreenWidth;


            AngleVector viewAngle = ViewDirection.Angle;

            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    //pixels[x, y] =
                    //    new Vector3(
                    //        (-ScreenWidth / 2) + x,
                    //        (-ScreenHeight / 2) + y,
                    //        0);

                    pixels[x, y] =
                        new Vector3(0, 0, 1)
                        .RotateX((radPerPixelHeight * y) - (radPerPixelHeight * ScreenHeight / 2))
                        .RotateY((radPerPixelWidth * x) - (radPerPixelWidth * ScreenWidth / 2));
                }
            }
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {


                    Vector3 changed = (pixels[x, y].RotateX(viewAngle.Theta - Vector3.DegToRad(90))
                           .RotateY(-(viewAngle.Phi - (ViewDirection.X == 0 && ViewDirection.Z == 0 ? 0 : Vector3.DegToRad(90))))
                       );// + (ViewDirection.AsNormalized() * AngleLenght);
                    pixels[x, y] = changed;
                }

            }
            watch.Stop();
            Debug.WriteLine("GetRays:" + watch.ElapsedMilliseconds);
            return pixels;
        }

        public RayHit MarchRay(Vector3 rayDir, double maxRange, Vector3 startPos)
        {
            // define the current position of the ray
            Vector3 currentPos = startPos;

            // check if there is no geometry too close
            Geometry Closest = null;
            double distance = double.PositiveInfinity;

            double totalDistance = 0;

            var minDist = Objects.Aggregate(
                (currMin, o) => 
                (currMin == null || o.GetDistance(currentPos) < currMin.GetDistance(currentPos)) ? o : currMin);
            

            Closest = minDist;
            distance = minDist.GetDistance(currentPos);

            double PrecissionCorrection = MarchPrecission / 3;

            while (distance > MarchPrecission && totalDistance < maxRange)
            {
                // move the distance
                currentPos = currentPos + (rayDir.AsNormalized() * (distance - (PrecissionCorrection / 2)));

                // get next closest
                /*
                minDist = 
                    Objects.Aggregate(
                        (currMin, o) => (currMin == null || o.GetDistance(currentPos) < currMin.GetDistance(currentPos)) ? o : currMin);
                */
                minDist = Objects[0];
                double mDist = minDist.GetDistance(currentPos); 
                
                for (int i = 1; i <Objects.Count ; i++)
                {
                    double nextDist = Objects[i].GetDistance(currentPos);
                    if (nextDist < mDist)
                    {
                        minDist = Objects[i];
                        mDist = nextDist;
                    }
                }



                Closest = minDist;
                distance = mDist;
                totalDistance += distance;
            }

            if (distance <= MarchPrecission)
            {
                // we have hit an object
                Vector3 L = (LightPosition - currentPos).Normalize();
                Vector3 N = Closest.GetNormal(currentPos);
                Vector3 R = 2 * Vector3.DotProduct(L, N) * N - L;
                return new RayHit(currentPos, Closest,N ,(startPos-currentPos).Normalize(),R);
            }
            return new RayHit(currentPos, null,Vector3.Zero,Vector3.One,Vector3.Zero);
        }

        public PInfo[,] RenderImage()
        {

            Stopwatch watch = new Stopwatch();


            Vector3[,] rays = GetRays();

            List<Task> tasks = new List<Task>();

            watch.Start();
            int boxCount = 0;
            for (int x = 0; x < ScreenWidth; x++)
            {
                int tx = x;
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        for (int y = 0; y < ScreenHeight; y++)
                        {

                            int ty = y;


                            RayHit hit = MarchRay(rays[tx, ty], MaxLength, Position);

                            

                            if (hit.Object != null)
                            {
                                double illumination =
                                   LightAmbient * hit.Object.Properties.AmbientConstant +
                                   hit.Object.Properties.DiffuseConstant *
                                   (Vector3.DotProduct((LightPosition - hit.Position).Normalize(), hit.Normal)) * LightDiffuse;

                                GeometryColor color = hit.Object.Properties.Color;//.Darken(illumination);
                                GeometryColor colorDark = hit.Object.Properties.Color.Darken(illumination);


                                ScreenBuffer[tx, ty] = ColorUtil.GetRepresentation(colorDark.Red, colorDark.Green, colorDark.Blue);

                                
                            }
                            else
                            {
                                ScreenBuffer[tx, ty] = new PInfo(' ', ConsoleColor.White, DefaultColor);
                            }                            
                        }
                    }
                    catch (Exception)
                    {
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Debug.WriteLine("Final Render Time: " + watch.ElapsedMilliseconds);
            Debug.WriteLine("Boxes: " + boxCount);
            return ScreenBuffer;
        }

        public ConsoleColor Shade(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return ConsoleColor.Black;
                case ConsoleColor.DarkBlue:
                    return ConsoleColor.Blue;
                case ConsoleColor.DarkGreen:
                    return ConsoleColor.Green;
                case ConsoleColor.DarkCyan:
                    return ConsoleColor.Cyan;
                case ConsoleColor.DarkRed:
                    return ConsoleColor.Red;
                case ConsoleColor.DarkMagenta:
                    return ConsoleColor.Magenta;
                case ConsoleColor.DarkYellow:
                    return ConsoleColor.Yellow;
                case ConsoleColor.Gray:
                    return ConsoleColor.DarkGray;
                case ConsoleColor.DarkGray:
                    return ConsoleColor.Black;
                case ConsoleColor.Blue:
                    return ConsoleColor.DarkBlue;
                case ConsoleColor.Green:
                    return ConsoleColor.DarkGreen;
                case ConsoleColor.Cyan:
                    return ConsoleColor.DarkCyan;
                case ConsoleColor.Red:
                    return ConsoleColor.DarkRed;
                case ConsoleColor.Magenta:
                    return ConsoleColor.DarkMagenta;
                case ConsoleColor.Yellow:
                    return ConsoleColor.DarkYellow;
                case ConsoleColor.White:
                    return ConsoleColor.Gray;
                default:
                    return ConsoleColor.Black;
            }
        }


    }
}
