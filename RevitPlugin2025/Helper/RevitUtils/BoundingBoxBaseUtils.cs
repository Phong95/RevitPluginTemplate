using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Helper.RevitUtils
{
    public static class BoundingBoxBaseUtils
    {
        public static void DrawBoundingBox(Document doc, Element ele)
        {
            BoundingBoxXYZ bbox = ele.get_BoundingBox(null);
            XYZ pt0 = new XYZ(bbox.Min.X, bbox.Min.Y, bbox.Min.Z);
            XYZ pt1 = new XYZ(bbox.Max.X, bbox.Min.Y, bbox.Min.Z);
            XYZ pt2 = new XYZ(bbox.Max.X, bbox.Max.Y, bbox.Min.Z);
            XYZ pt3 = new XYZ(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);

            // Edges in BBox coords

            Line edge0 = Line.CreateBound(pt0, pt1);
            Line edge1 = Line.CreateBound(pt1, pt2);
            Line edge2 = Line.CreateBound(pt2, pt3);
            Line edge3 = Line.CreateBound(pt3, pt0);
            ModelLineBaseUtils.CreateModelLine(edge0, doc);
            ModelLineBaseUtils.CreateModelLine(edge1, doc);
            ModelLineBaseUtils.CreateModelLine(edge2, doc);
            ModelLineBaseUtils.CreateModelLine(edge3, doc);

        }
        public static void CreateBoundingBox(Document doc, Element ele)
        {
            ElementId geneidcolumn = new ElementId(BuiltInCategory.OST_StructuralFoundation);
            BoundingBoxXYZ bbox = ele.get_BoundingBox(null);
            XYZ pt0 = new XYZ(bbox.Min.X, bbox.Min.Y, bbox.Min.Z);
            XYZ pt1 = new XYZ(bbox.Max.X, bbox.Min.Y, bbox.Min.Z);
            XYZ pt2 = new XYZ(bbox.Max.X, bbox.Max.Y, bbox.Min.Z);
            XYZ pt3 = new XYZ(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);

            // Edges in BBox coords

            Line edge0 = Line.CreateBound(pt0, pt1);
            Line edge1 = Line.CreateBound(pt1, pt2);
            Line edge2 = Line.CreateBound(pt2, pt3);
            Line edge3 = Line.CreateBound(pt3, pt0);

            // Create loop, still in BBox coords

            List<Curve> edges = new List<Curve>();
            edges.Add(edge0);
            edges.Add(edge1);
            edges.Add(edge2);
            edges.Add(edge3);

            double height = Math.Abs(bbox.Max.Z - bbox.Min.Z);
            CurveLoop baseLoop = CurveLoop.Create(edges);

            List<CurveLoop> loopList = new List<CurveLoop>();
            loopList.Add(baseLoop);
            try
            {
                Solid preTransformBox = GeometryCreationUtilities.CreateExtrusionGeometry(loopList, XYZ.BasisZ, height);
                Solid transformBox = SolidUtils.CreateTransformed(preTransformBox, bbox.Transform);

                DirectShape ds = DirectShape.CreateElement(doc, geneidcolumn);
                ds.SetShape(new GeometryObject[] { transformBox });
                ElementColorBaseUtils.OverideColor(doc, ds, 50);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
