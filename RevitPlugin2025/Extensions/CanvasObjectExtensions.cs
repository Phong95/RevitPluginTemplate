using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace RevitPlugin2025.Extensions
{
    public static class CanvasObjectExtensions
    {
        public static double Length(this Line line)
        {
            return Vector(line).Length;
        }
        public static Vector Vector(this Line line)
        {
            Vector vector = new Vector();
            vector.X = line.X2 - line.X1;
            vector.Y = line.Y2 - line.Y1;
            return vector;
        }
        public static Vector Center(this Line line)
        {
            Vector vector = new Vector();
            vector.X = (line.X2 + line.X1)/2;
            vector.Y = (line.Y2 + line.Y1)/2;
            return vector;
        }
        public static Vector Normal1(this Vector vector)
        {
            Vector result = new Vector();
            result.X = -vector.Y;
            result.Y = vector.X;
            return result;
        }
        public static Vector Normal2(this Vector vector)
        {
            Vector result = new Vector();
            result.X = vector.Y;
            result.Y = -vector.X;
            return result;
        }
    }
}
