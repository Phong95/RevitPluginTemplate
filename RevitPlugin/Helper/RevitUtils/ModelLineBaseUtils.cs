using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class ModelLineBaseUtils
    {
        public static void CreateModelLine(Curve cv, Document doc)
        {
            if (cv is Line)
            {
                XYZ point = ((Autodesk.Revit.DB.Line)(cv)).Origin;
                XYZ dir = ((Autodesk.Revit.DB.Line)(cv)).Direction;
                double x = dir.X, y = dir.Y, z = dir.Z;
                XYZ n = new XYZ(z - y, x - z, y - x);
                Plane pla = Plane.CreateByNormalAndOrigin(n, point);
                SketchPlane spla = SketchPlane.Create(doc, pla);
                doc.Create.NewModelCurve(cv, spla);
            }
            if (cv is Arc)
            {
                XYZ normal = ((Autodesk.Revit.DB.Arc)(cv)).Normal;
                XYZ point = ((Autodesk.Revit.DB.Arc)(cv)).Center;
                Plane pla = Plane.CreateByNormalAndOrigin(normal, point);
                SketchPlane spla = SketchPlane.Create(doc, pla);
                doc.Create.NewModelCurve(cv, spla);
            }
            if (cv is HermiteSpline || cv is NurbSpline)
            {
                for (int i = 0; i < cv.Tessellate().Count(); i++)
                {
                    if (i == cv.Tessellate().Count() - 1)
                    {
                        break;
                    }
                    double para1 = cv.Project(cv.Tessellate()[i]).Parameter;
                    double para2 = cv.Project(cv.Tessellate()[i + 1]).Parameter;
                    Curve cv1 = cv.Clone();
                    cv1.MakeBound(para1, para2);
                    Arc arc = Arc.Create(cv1.GetEndPoint(0), cv1.GetEndPoint(1), cv1.Evaluate(0.5, true));
                    XYZ origin = arc.GetEndPoint(0);
                    XYZ normal = arc.ComputeDerivatives(0, true).BasisZ.Normalize();
                    SketchPlane skPlane = SketchPlane.Create(doc, Plane.CreateByNormalAndOrigin(normal, origin));
                    doc.Create.NewModelCurve(arc, skPlane);
                }
            }
        }
        public static void CreateModelLine(Curve cv, Document doc, ref ModelCurve modelCurve)
        {
            if (cv is Line)
            {
                XYZ point = ((Autodesk.Revit.DB.Line)(cv)).Origin;
                XYZ dir = ((Autodesk.Revit.DB.Line)(cv)).Direction;
                double x = dir.X, y = dir.Y, z = dir.Z;
                XYZ n = new XYZ(z - y, x - z, y - x);
                Plane pla = Plane.CreateByNormalAndOrigin(n, point);
                SketchPlane spla = SketchPlane.Create(doc, pla);
                modelCurve = doc.Create.NewModelCurve(cv, spla);
            }
            if (cv is Arc)
            {
                XYZ normal = ((Autodesk.Revit.DB.Arc)(cv)).Normal;
                XYZ point = ((Autodesk.Revit.DB.Arc)(cv)).Center;
                Plane pla = Plane.CreateByNormalAndOrigin(normal, point);
                SketchPlane spla = SketchPlane.Create(doc, pla);
                modelCurve = doc.Create.NewModelCurve(cv, spla);
            }
            if (cv is HermiteSpline || cv is NurbSpline)
            {
                for (int i = 0; i < cv.Tessellate().Count(); i++)
                {
                    if (i == cv.Tessellate().Count() - 1)
                    {
                        break;
                    }
                    double para1 = cv.Project(cv.Tessellate()[i]).Parameter;
                    double para2 = cv.Project(cv.Tessellate()[i + 1]).Parameter;
                    Curve cv1 = cv.Clone();
                    cv1.MakeBound(para1, para2);
                    Arc arc = Arc.Create(cv1.GetEndPoint(0), cv1.GetEndPoint(1), cv1.Evaluate(0.5, true));
                    XYZ origin = arc.GetEndPoint(0);
                    XYZ normal = arc.ComputeDerivatives(0, true).BasisZ.Normalize();
                    SketchPlane skPlane = SketchPlane.Create(doc, Plane.CreateByNormalAndOrigin(normal, origin));
                    modelCurve = doc.Create.NewModelCurve(arc, skPlane);
                }
            }
        }
    }
}
