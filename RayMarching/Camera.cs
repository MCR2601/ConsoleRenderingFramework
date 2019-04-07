using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 ViewDirection;

        public int ScreenHeight;
        public int ScreenWidth;

        public double FOV; // in degree

        public double MarchPrecission = 0.5;

        public List<Geometry> Objects;

        public Camera(Vector3 position, Vector3 viewDirection, int screenHeight, int screenWidth, double fov, List<Geometry> objects)
        {
            Position = position;
            ViewDirection = viewDirection;
            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;
            FOV = fov;
            Objects = objects;
        }

        public Vector3[,] GetRays()
        {
            Vector3[,] pixels = new Vector3[ScreenWidth, ScreenHeight];

            // calculate the bottom left pixel

            // we rotate left and down for that 

            double radPerPixelWidth = Vector3.DegToRad(FOV) / ScreenWidth;
            double radPerPixelHeight = Vector3.DegToRad(FOV) / ScreenHeight;

            double heightDist = 0.5;

            AngleVector viewAngle = ViewDirection.Angle;

            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    pixels[x,y] = 
                        new Vector3(
                            (-ScreenWidth/2)+x,
                            (-ScreenHeight/2)+y,
                            0)
                            .RotateX(viewAngle.Theta - Vector3.DegToRad(90))
                            .RotateY(viewAngle.Phi - (ViewDirection.X==0?0:Vector3.DegToRad(90)))
                        + (ViewDirection);
                }
            }
            return pixels;
        }

        public RayHit MarchRay(Vector3 rayDir)
        {
            // define the current position of the ray
            Vector3 currentPos = Position;

            // check if there is no geometry too close
            Geometry Closest = null;
            double distance = Double.PositiveInfinity;

            double totalDistance = 0;

            do
            {
                var distances = Objects.Select(obj => new {obj = obj, distance = obj.GetDistance(currentPos)})
                    .OrderBy(info => info.distance).ToList();

                var c = distances.First();

                Closest = c.obj;
                distance = c.distance;

            } while (distance>MarchPrecission && totalDistance < 100);


        }



    }
}
