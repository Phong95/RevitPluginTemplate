using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugin2025.Models.RevitCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Helper.RevitUtils
{
    public static class CameraUtils
    {
        public static RevitCameraModel saveState(View3D view3D)
        {
            RevitCameraModel cameraMode = new RevitCameraModel();

            if (view3D == null || view3D.IsPerspective)
            {
                Autodesk.Revit.UI.TaskDialog ts = new Autodesk.Revit.UI.TaskDialog("Incorrect View selected")
                {
                    MainContent = "Please, select 3D Orthographic view."
                };

                ts.Show();
                return null;
            }

            // Get viewOrientation3D
            ViewOrientation3D viewOrientation3D = view3D.GetSavedOrientation();
            cameraMode.ForwardDirection = viewOrientation3D.ForwardDirection;
            cameraMode.UpDirection = viewOrientation3D.UpDirection;
            cameraMode.EyePosition = viewOrientation3D.EyePosition;
            cameraMode.Target = CalculateTarget(view3D, viewOrientation3D.ForwardDirection);
            cameraMode.Origin = view3D.Origin;
            view3D.IsSectionBoxActive = true;
            BoundingBoxXYZ bb = view3D.GetSectionBox();
            //coordinate from original point (0,0,0) need inverse to get correct position
            Transform trf = bb.Transform;
            //trf = trf.Inverse;
            cameraMode.Box.Min = trf.OfPoint(view3D.GetSectionBox().Min);
            cameraMode.Box.Max = trf.OfPoint(view3D.GetSectionBox().Max);

            return cameraMode;
        }

        private static XYZ CalculateTarget(View3D view, XYZ forward)
        {
            var target = view.Origin.Add(forward * (view.CropBox.Max.Z - view.CropBox.Min.Z));
            var targetElevation = view.get_Parameter(BuiltInParameter.VIEWER_TARGET_ELEVATION).AsDouble();
            if (target.Z != targetElevation) // check if target matches stored elevation
            {
                double magnitude = (targetElevation - view.Origin.Z) / forward.Z;
                target = view.Origin.Add(forward * magnitude);
            }
            return target;
        }
    }
}
