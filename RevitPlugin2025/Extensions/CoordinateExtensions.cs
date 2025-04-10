using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Extensions
{
    public static class CoordinateExtensions
    {
        public static XYZ MoveTo(this XYZ basePoint, XYZ direction, double length)
        {
            return basePoint.Add(direction * length);
        }
    }
}
