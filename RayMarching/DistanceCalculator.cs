using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public static class DistanceCalculator
    {

        public static double SphereDist(Vector3 from, Vector3 pos, double radius = 1)
        {
            Vector3 d = (from - pos);
            d = new Vector3((d.X + 3) % 6, (d.Y + 3) % 6, (d.Z + 3) % 6) - new Vector3(3, 3, 3);
            return d.Length - radius;
        }

        public static double BoxDist(Vector3 from, Vector3 pos, Vector3 box)
        {
            Vector3 d = (pos - from).Abs() - box;
            return Vector3.Max(d, 0).Length + Math.Min(Math.Max(d.X, Math.Max(d.Y, d.Z)), 0.0);// - 0.3;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="pos"></param>
        /// <param name="t">contains info about radiuses(in X,Y)</param>
        /// <returns></returns>
        public static double TorusDist(Vector3 from, Vector3 pos, Vector3 t)
        {
            Vector3 p = from - pos;
            Vector3 q = new Vector3(new Vector3(p.X,0,p.Z).Length-t.X, p.Y);
            return q.Length - t.Y;
        }


    }
}
