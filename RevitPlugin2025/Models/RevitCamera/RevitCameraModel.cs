using Autodesk.Revit.DB;
using RevitPlugin2025.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.RevitCamera
{
    public class RevitCameraModel
    {
        public double Scale { get; set; }
        public XYZ Origin { get; set; }
        public XYZ EyePosition { get; set; }

        public XYZ UpDirection { get; set; }

        public XYZ ForwardDirection { get; set; }
        public XYZ Target { get; set; }
        public RevitBox Box { get; set; } = new RevitBox();
        public RevitCameraModel()
        {

        }

    }
    public class RevitBox
    {
        public XYZ Min { get; set; }
        public XYZ Max { get; set; }
    }
}
