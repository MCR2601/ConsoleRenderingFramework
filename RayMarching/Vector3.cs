using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace RayMarching
{
    public class Vector3
    {

        public double X;
        public double Y;
        public double Z;

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                }

                return 0;
            }
        }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            Clean();
        }

        public Vector3(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
            Clean();
        }

        public Vector3(params double[] dimensions)
        {
            X = 0;
            Y = 0;
            Z = 0;
            if (dimensions.Length > 0)
            {
                X = dimensions[0];
            }

            if (dimensions.Length > 1)
            {
                Y = dimensions[1];
            }

            if (dimensions.Length > 2)
            {
                Z = dimensions[2];
            }
            Clean();
        }


        public void Clean(int detail = 10)
        {
            X = Math.Round(X,detail);
            Y = Math.Round(Y, detail);
            Z = Math.Round(Z, detail);
        }

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public double LengthS => X * X + Y * Y + Z * Z;

        public Vector3 GetCopy() => new Vector3(X, Y, Z);
        public Vector3 Normalize()
        {
            double l = Length;
            X = X / l;
            Y = Y / l;
            Z = Z / l;
            return this;
        }

        public Vector3 AsNormalized()
        {
            double l = Length;
            return new Vector3(X / l, Y / l, Z / l);
        }

        public bool Normalized => Length == 1.0d;

        public Vector3 Abs() => new Vector3(Math.Abs(X),Math.Abs(Y),Math.Abs(Z));

        // TODO This formula is wrong, very WRONG
        public AngleVector Angle => 
            new AngleVector(
                Math.Atan2(Math.Sqrt(Y*Y+Z*Z),X), 
                Math.Atan2(Math.Sqrt(Z*Z+X*X), Y), 
                Math.Atan2(Math.Sqrt(X*X+Y*Y), Z),
                Math.Atan2(Z,X),
                Math.Acos(Y / Length));

        public Vector3 RotateX(double rad)
        {
            return this * new double[,]
            {
                {1, 0, 0},
                {0, Math.Cos(rad), -Math.Sin(rad)},
                {0, Math.Sin(rad), Math.Cos(rad)}
            };
        }
        public Vector3 RotateY(double rad)
        {
            return this * new double[,]
            {
                {Math.Cos(rad),0,Math.Sin(rad)},
                {0,1,0},
                {-Math.Sin(rad),0,Math.Cos(rad)}
            };
        }
        public Vector3 RotateZ(double rad)
        {
            return this * new double[,]
            {
                {Math.Cos(rad),-Math.Sin(rad),0},
                {Math.Sin(rad),Math.Cos(rad),0},
                {0,0,1}
            };
        }

        public static double RadToDeg(double rad)
        {
            return 180 / Math.PI;
        }

        public static double DegToRad(double deg)
        {
            return (Math.PI / 180) * deg;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vector3 operator *(Vector3 a, double v)
        {
            return new Vector3(a.X * v, a.Y * v, a.Z * v);
        }
        public static Vector3 operator /(Vector3 a, double v)
        {
            return new Vector3(a.X / v, a.Y / v, a.Z / v);
        }

        public static Vector3 operator *(Vector3 a, double[,] matrix)
        {
            List<double> values = new List<double>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                values.Add(0);
            }

            for (int dimension = 0; dimension < matrix.GetLength(1); dimension++)
            {
                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                    values[dimension] = values[dimension] + matrix[dimension, column] * a[column];
                }
            }
            return new Vector3(values.ToArray());
        }

        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        
        public static readonly Vector3 Zero = new Vector3(0,0,0);
        public static readonly Vector3 One = new Vector3(1,1,1);

        public static Vector3 Max(Vector3 a, Vector3 b )
        {
            return new Vector3(Math.Max(a.X,b.X), Math.Max(a.Y, b.Y), Math.Max(a.Y, b.Y));
        }

        public override string ToString()
        {
            return "[X:" + X + "|Y:" + Y + "|Z:" + Z + "]";
        }
    }

}
