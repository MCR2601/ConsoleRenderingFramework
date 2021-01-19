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

        public double MaxLength = 100;
        public double LightHeight = 10;

        public int ScreenHeight;
        public int ScreenWidth;

        PInfo[,] ScreenBuffer;

        public double FOV; // in degree

        public double MarchPrecission = 0.0001;

        public List<Geometry> Objects;

        public ConsoleColor DefaultColor = ConsoleColor.Blue;

        public Camera(Vector3 position, Vector3 viewDirection, int screenHeight, int screenWidth, double fov, List<Geometry> objects)
        {
            Position = position;
            ViewDirection = viewDirection;
            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;
            FOV = fov;
            Objects = objects;
            ScreenBuffer = new PInfo[screenWidth, screenHeight];
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

        public RayHit MarchRay(Vector3 rayDir,double maxRange,Vector3 startPos)
        {
            // define the current position of the ray
            Vector3 currentPos = startPos;

            // check if there is no geometry too close
            Geometry Closest = null;
            double distance = Double.PositiveInfinity;

            double totalDistance = 0;

            var distances = Objects.Select(obj => new { obj = obj, distance = obj.GetDistance(currentPos) })
                    .OrderBy(info => info.distance).ToList();

            var c = distances.First();

            Closest = c.obj;
            distance = c.distance;

            double PrecissionCorrection = MarchPrecission / 3;

            while (distance > MarchPrecission && totalDistance < maxRange)
            {
                // move the distance
                currentPos = currentPos + (rayDir.AsNormalized() * (distance - (PrecissionCorrection/2)));

                // get next closest
                distances = Objects.Select(obj => new { obj = obj, distance = obj.GetDistance(currentPos) })
                    .OrderBy(info => info.distance).ToList();

                c = distances.First();

                Closest = c.obj;
                distance = c.distance;
                totalDistance += distance;
            }

            if (distance <= MarchPrecission)
            {
                // we have hit an object
                return new RayHit(currentPos, Closest, Closest.Color);
            }
            return new RayHit(currentPos, null, DefaultColor);
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

                            RayHit skyHit = MarchRay(new Vector3(0, 1, 0), LightHeight - hit.Position.Y,
                                hit.Position + new Vector3(0, MarchPrecission * 2, 0));
                            if (hit.Object != null)
                            {
                                if (hit.Object.Type == Geometry.GType.Box)
                                {
                                    boxCount++;
                                }
                            }
                            ScreenBuffer[tx, ty] = new PInfo().SetBg(skyHit.Object == null ? hit.color : Shade(hit.color));
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
