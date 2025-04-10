using Autodesk.Revit.DB;
using RevitPlugin.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class GeometryBaseUtils
    {
        public static List<Solid> GetSolid(Document doc, Element element, ViewDetailLevel VDL, Document docLink = null, Transform revitLinkTransform = null)
        {
            List<Element> listTotalElement = new List<Element>();
            listTotalElement.Add(element);
            FamilyInstance fe = element as FamilyInstance;
            if (fe != null)
            {
                ICollection<ElementId> listsubid = fe.GetSubComponentIds();
                if (listsubid.Count() != 0)
                {
                    foreach (ElementId eleid in listsubid)
                    {
                        Element ele = null;
                        if (!docLink.IsNotNull())
                        {
                            doc.GetElement(eleid);
                        }
                        else
                        {
                            docLink.GetElement(eleid);
                        }
                        listTotalElement.Add(ele);
                    }
                }
            }
            List<Solid> ListResult = new List<Solid>();
            foreach (Element ele in listTotalElement)
            {
                if (ele is null) continue;
                Options opt = new Options();
                opt.DetailLevel = VDL;
                GeometryElement GeoE = ele.get_Geometry(opt);
                if (!GeoE.IsNotNull()) continue;
                foreach (GeometryObject GeoO in GeoE)
                {
                    GeometryInstance instance = GeoO as GeometryInstance;

                    if (null != instance)
                    {
                        Autodesk.Revit.DB.Transform transform = instance.Transform;
                        foreach (GeometryObject instObj in instance.GetSymbolGeometry(transform))
                        {
                            Solid solid = instObj as Solid;
                            if (solid == null)
                            {
                                continue;
                            }
                            if (((Autodesk.Revit.DB.Solid)instObj).Volume != 0 || ((Autodesk.Revit.DB.Solid)instObj).SurfaceArea != 0)
                            {
                                if (revitLinkTransform.IsNotNull())
                                {
                                    ListResult.Add(SolidUtils.CreateTransformed(solid, revitLinkTransform));

                                }
                                else
                                {
                                    ListResult.Add(solid);

                                }
                            }
                        }
                    }
                    else
                    {
                        if (GeoO is Solid)
                        {
                            if (((Autodesk.Revit.DB.Solid)GeoO).Volume != 0 || ((Autodesk.Revit.DB.Solid)GeoO).SurfaceArea != 0)
                            {
                                Solid SolidGeoO = GeoO as Solid;

                                if (revitLinkTransform.IsNotNull())
                                {
                                    ListResult.Add(SolidUtils.CreateTransformed(SolidGeoO, revitLinkTransform));
                                }
                                else
                                {
                                    ListResult.Add(SolidGeoO);
                                }
                            }
                        }
                    }
                }
            }

            return ListResult;
        }
        public static List<Solid> GetSolidOriginal(Document doc, Element element, ViewDetailLevel VDL)
        {
            List<Solid> ListResult = new List<Solid>();
            List<Element> listTotalElement = new List<Element>();
            listTotalElement.Add(element);
            FamilyInstance fe = element as FamilyInstance;
            if (fe != null)
            {
                ICollection<ElementId> listsubid = fe.GetSubComponentIds();
                if (listsubid.Count() != 0)
                {
                    foreach (ElementId eleid in listsubid)
                    {
                        Element ele = doc.GetElement(eleid);
                        listTotalElement.Add(ele);
                    }
                }
            }
            foreach (Element ele in listTotalElement)
            {
                FamilyInstance familyInstance = ele as FamilyInstance;
                GeometryElement GeoEOriginal = null;
                if (familyInstance != null)
                {
                    Options op = new Options();
                    op.DetailLevel = VDL;
                    op.ComputeReferences = false;
                    GeoEOriginal = familyInstance.GetOriginalGeometry(op);
                    Autodesk.Revit.DB.Transform transf = familyInstance.GetTransform();
                    GeoEOriginal = GeoEOriginal.GetTransformed(transf);
                }
                else
                {
                    Options op = new Options();
                    op.DetailLevel = VDL;
                    op.ComputeReferences = false;
                    GeoEOriginal = ele.get_Geometry(op);
                }
                foreach (GeometryObject GeoO in GeoEOriginal)
                {
                    GeometryInstance instance = GeoO as GeometryInstance;

                    if (null != instance)
                    {
                        Autodesk.Revit.DB.Transform transform = instance.Transform;
                        foreach (GeometryObject instObj in instance.GetSymbolGeometry(transform))
                        {
                            Solid solid = instObj as Solid;
                            if (solid == null)
                            {
                                continue;
                            }
                            //if (((Autodesk.Revit.DB.Solid)instObj).Volume != 0)
                            //{
                            ListResult.Add(solid);
                            //}
                        }
                    }
                    else
                    {
                        if (GeoO is Solid)
                        {
                            //if (((Autodesk.Revit.DB.Solid)GeoO).Volume != 0)
                            //{
                            Solid SolidGeoO = GeoO as Solid;
                            ListResult.Add(SolidGeoO);
                            //}
                        }
                    }
                }
            }

            return ListResult;
        }
        public static XYZ NormalOfFace(Face f)
        {
            return f.ComputeNormal(new UV(0, 0));
        }
        public static XYZ UvToXyz(UV uv, Face f)
        {
            return f.Evaluate(uv);
        }
        public static XYZ ProjectOnto(Plane plane, XYZ p)
        {

            double d = plane.SignedDistanceTo(p);
            XYZ q = p - d * plane.Normal;
            return q;
        }
        public static double SignedDistanceTo(this Plane plane, XYZ p)
        {
            XYZ v = p - plane.Origin;
            //dotproduct là hình chiếu của vecto này lên vecto kia
            //nên lấy vecto v chiếu lên vecto pháp tuyến sẽ ra khoản cách 
            //giữa điểm đang xét tới mặt phẳng
            return plane.Normal.DotProduct(v);
        }
        public static UV ProjectInto(this XYZ p, Plane plane )
        {
            XYZ q = ProjectOnto(plane, p);
            XYZ o = plane.Origin;
            XYZ d = q - o;
            double u = d.DotProduct(plane.XVec);
            double v = d.DotProduct(plane.YVec);
            return new UV(u, v);
        }

        public static XYZ GetVectorFrom2Points(XYZ point1, XYZ point2)
        {
            return new XYZ(point2.X - point1.X, point2.Y - point1.Y, point2.Z - point1.Z);
        }
        /// <summary>
        /// create curveloop from list curve
        /// </summary>
        /// <param name="cuva">list curve</param>
        /// <param name="round">input is mm</param>
        /// <returns></returns>
        public static CurveLoop SapXepThanhCurveLoop(List<Curve> cuva, double round)
        {
            List<Curve> Dem = new List<Curve>();
            CurveLoop ketqua = new CurveLoop();
            double roundNumber = UnitBaseUtils.MmToFt(round);
            foreach (Curve cv in cuva)
            {
                ketqua.Append(cv);
                Dem.Add(cv);
                cuva.Remove(cv);
                break;
            }
            for (int i = 0; i < Dem.Count; i++)
            {
                if (!cuva.Any())
                {
                    double dist0 = Math.Round(Dem.Last().GetEndPoint(1).DistanceTo(Dem.First().GetEndPoint(0)), 6);
                    if (dist0 <= roundNumber)
                    {
                        if (dist0 == 0)
                        {

                        }
                        else
                        {
                            Curve addCurve = Line.CreateBound(Dem.Last().GetEndPoint(1), Dem.First().GetEndPoint(0));
                            ketqua.Append(addCurve);
                            break;
                        }

                    }
                }
                foreach (Curve cv in cuva)
                {
                    double dist0 = Math.Round(Dem[i].GetEndPoint(1).DistanceTo(cv.GetEndPoint(0)), 6);
                    double dist1 = Math.Round(Dem[i].GetEndPoint(1).DistanceTo(cv.GetEndPoint(1)), 6);

                    if (dist0 <= roundNumber)
                    {
                        if (dist0 == 0)
                        {
                            Dem.Add(cv);
                            ketqua.Append(cv);
                            cuva.Remove(cv);
                            break;
                        }
                        else
                        {
                            Curve addCurve = Line.CreateBound(Dem[i].GetEndPoint(1), cv.GetEndPoint(0));
                            Dem.Add(addCurve);
                            ketqua.Append(addCurve);
                            break;
                        }

                    }
                    else if (dist1 <= roundNumber)
                    {
                        if (dist1 == 0)
                        {
                            Dem.Add(cv.CreateReversed());
                            ketqua.Append(cv.CreateReversed());
                            cuva.Remove(cv);
                            break;
                        }
                        else
                        {
                            Curve addCurve = Line.CreateBound(Dem[i].GetEndPoint(1), cv.GetEndPoint(1));
                            Dem.Add(addCurve);
                            ketqua.Append(addCurve);
                            break;
                        }
                    }
                }
            }
            return ketqua;
        }
        public static List<CurveLoop> SapXepThanhNhieuCurveLoop(List<Curve> cuva)
        {
            List<CurveLoop> ResultFinal = new List<CurveLoop>();
        a:
            List<Curve> Dem = new List<Curve>();
            CurveLoop ketqua = new CurveLoop();
            foreach (Curve cv in cuva)
            {
                ketqua.Append(cv);
                Dem.Add(cv);
                cuva.Remove(cv);
                break;
            }
            for (int i = 0; i < Dem.Count; i++)
            {
                foreach (Curve cv in cuva)
                {
                    if (SoSanhGiua2Diem(Dem[i].GetEndPoint(1), cv.GetEndPoint(0)) == true)
                    {
                        Dem.Add(cv);
                        ketqua.Append(cv);
                        cuva.Remove(cv);
                        break;
                    }
                }
            }
            ResultFinal.Add(ketqua);
            if (cuva.Count() > 0)
            {
                goto a;
            }
            return ResultFinal;
        }
        public static bool SoSanhGiua2Diem(XYZ diem1, XYZ diem2)
        {
            bool ketqua = false;
            if (Math.Round(diem1.X, 6) == Math.Round(diem2.X, 6) && Math.Round(diem1.Y, 6) == Math.Round(diem2.Y, 6) && Math.Round(diem1.Z, 6) == Math.Round(diem2.Z, 6))
            {
                ketqua = true;
            }
            return ketqua;
        }
        public static XYZ trungDiem(XYZ diem1, XYZ diem2)
        {
            return new XYZ((diem1.X + diem2.X) / 2, (diem1.Y + diem2.Y) / 2, (diem1.Z + diem2.Z) / 2);
        }
        public static double AreaFrom3Points(XYZ point1,XYZ point2,XYZ point3)
        {
            double a = point1.DistanceTo(point2);
            double b = point2.DistanceTo(point3);
            double c = point3.DistanceTo(point1);
            double p = (a + b + c) / 2;
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return s;
        }
        public static XYZ CenterPoint(List<XYZ> points)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            foreach (var item in points)
            {
                x += item.X;
                y += item.Y;
                z += item.Z;
            }
            return new XYZ(x / points.Count, y / points.Count, z / points.Count);
        }
        /// <summary>
        /// Intersect this solid with given plane and return the list of intersecting curves.
        /// <para>Note that curves are not in order.</para>
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="plane"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Curve> SolidIntersectPlane(this Solid solid, Plane plane, double tolerance)
        {
            List<Curve> intersections = new List<Curve>();
            // cut the solid with positive side of the plane
            Solid s = BooleanOperationsUtils.CutWithHalfSpace(solid, plane);
            if (s == null)
            {
                // it could be that the plane is just on the top of the solid 
                // we cut the solid again by the reverse plane 
                s = BooleanOperationsUtils.CutWithHalfSpace
                       (solid, Plane.CreateByNormalAndOrigin(-plane.Normal, plane.Origin));
                if (s == null)
                    return null; // plane does not intersect the solid
            }
            // search for edges in the new solid geometry which lies on the plane 
            foreach (Edge edge in s.Edges)
            {
                Curve curve = edge.AsCurve();
                if (curve is Line)
                {
                    // check if the line lies on the plane 
                    Line l = curve as Line;
                    // add curve to the intersection list if it lies on the intersection plane
                    if (plane.Intersect(l, tolerance, out XYZ p, out double t) == Plane_Line.Subset)
                        intersections.Add(curve);
                }
                else
                {
                    // add the curve to an empty CurveLoop to find out if the curve is planar
                    CurveLoop cl = new CurveLoop();
                    cl.Append(curve);
                    // if curve is planar then check if the plane of the curve is coincident with intersection plane 
                    if (cl.HasPlane())
                    {
                        Plane pl = cl.GetPlane(); // retrieve the plane of the curve
                                                  // add curve to the intersection list of it lies on the intersection plane
                        if (plane.IsCoincident(pl, tolerance))
                            intersections.Add(curve);
                    }
                }

            }
            return intersections; // return the result.
        }
        public static bool IsCoincident(this Plane p1, Plane p2, double tolerance)
        {
            // check if the normal of this plane is equal (or reverse ) to the normal of the other Plane
            if (p1.Normal.IsAlmostEqualTo(p2.Normal) || p1.Normal.IsAlmostEqualTo(-p2.Normal))
            {
                // if both planes are parallel then check if they have the same distance to the origin.
                double d1 = Math.Abs(p1.Origin.DotProduct(p1.Normal));
                double d2 = Math.Abs(p2.Origin.DotProduct(p2.Normal));
                // the difference between d1 and d2 must be within given tolerance
                return (Math.Abs(d1 - d2) < tolerance);
            }
            else
            {
                return false; // if planes are not parallel then they cannot be coincide 
            }
        }
        /// <summary>
        /// The status of line-plane intersection
        /// </summary>
        public enum Plane_Line
        {
            /// <summary>
            /// Line is completely inside the plane
            /// </summary>
            Subset,
            /// <summary>
            /// Line is parallel to the plane 
            /// </summary>
            Disjoint,
            /// <summary>
            /// Line is intersecting with the plane
            /// </summary>
            Intersecting
        }
        /// <summary>
        /// Compute Plane-Line intersection
        /// </summary>
        /// <param name="p">This plane</param>
        /// <param name="l">Line to intersect with</param>
        /// <param name="tolerance">Tolerance</param>
        /// <param name="intersectionPoint">The intersection point, Null if it does not exist</param>
        /// <param name="parameter">The parameter of the intersection point on the line</param>
        /// <returns></returns>
        public static Plane_Line Intersect(this Plane p, Line l, double tolerance,
        out XYZ intersectionPoint, out double parameter)
        {
            //compute the dot prodcut and signed distance 
            double denominator = l.Direction.DotProduct(p.Normal);
            double numerator = (p.Origin - l.GetEndPoint(0)).DotProduct(p.Normal);
            //check if the dot product is almost zero 
            if (Math.Abs(denominator) < tolerance)
            {
                // line is parallel to plane (could be inside or outside the plane)
                if (Math.Abs(numerator) < tolerance)
                {
                    //line is inside the plane
                    intersectionPoint = null;
                    parameter = double.NaN;
                    return Plane_Line.Subset;
                }
                else
                {
                    // line is outside the plane                    
                    intersectionPoint = null;
                    parameter = double.NaN;
                    return Plane_Line.Disjoint;
                }
            }
            else
            {
                // line is intersecting wih plane
                // compute the line paramemer 
                parameter = numerator / denominator;
                intersectionPoint = l.GetEndPoint(0) + parameter * l.Direction;
                return Plane_Line.Intersecting;
            }
        }


        #region AutoJoin
        public static bool ElementsIsIntersectByCompareSolid(Document doc, Element elm1, Element el2 )
        {
            List<Solid> solide1 = GetSolid(doc,elm1,ViewDetailLevel.Fine);
            List<Solid> solide2 = GetSolid(doc, el2, ViewDetailLevel.Fine);

            foreach (Solid item in solide1)
            {
                foreach (Solid item2 in solide2)
                {
                    try
                    {
                        if (BooleanOperationsUtils.ExecuteBooleanOperation(
                            item, item2, BooleanOperationsType.Intersect).Volume != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        if (ElementsIsIntersect(doc, elm1, el2))
                        {
                            return true;
                        }

                    }

                }
            }
            return false;
        }
        public static bool ElementsIsIntersectConsiderFace(Document doc, Element elm1, Element elm2)
        {
            List<Face> faces1 = GetFaceOfElement(elm1);
            List<Face> faces2 = GetFaceOfElement(elm2);

            foreach (Face item1 in faces1)
            {
                foreach (Face item2 in faces2)
                {
                    try
                    {
                        if (CheckfaceIsIntersectByExtractCurve(item2, item1))
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        if (ElementsIsIntersect(doc, elm1, elm2))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }
        public static List<Face> GetFaceOfElement(Element e)
        {
            GeometryElement geoElement = e.get_Geometry(new Options());
            List<Face> FacceOfElement = new List<Face>();
            foreach (GeometryObject obj in geoElement)
            {
                if (obj is Solid)
                {
                    Solid geomSolid = obj as Solid;
                    foreach (Face geoFace in geomSolid.Faces)
                    {
                        FacceOfElement.Add(geoFace);
                    }
                }
                else if (obj is GeometryInstance)
                {
                    GeometryInstance geoInst = obj as GeometryInstance;

                    GeometryElement geoElem = geoInst.SymbolGeometry;

                    Transform transform = geoInst.Transform;

                    try
                    {
                        //geomSolid = SolidUtils.CreateTransformed(geomSolid, transform);
                        foreach (Solid sl in GetSolidOfElemFromGeObj(geoElem))
                        {
                            Solid transSolid = SolidUtils.CreateTransformed(sl, transform);
                            foreach (Face geoFace in transSolid.Faces)
                            {
                                FacceOfElement.Add(geoFace);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            return FacceOfElement;
        }
        public static List<Solid> GetSolidOfElemFromGeObj(GeometryElement geoElement)
        {
            List<Solid> SolidOfElement = new List<Solid>();
            foreach (GeometryObject obj in geoElement)
            {
                if (obj is Solid)
                {
                    Solid geomSolid = obj as Solid;
                    if (geomSolid.Volume.ToString() != "0")
                    {
                        SolidOfElement.Add(geomSolid);
                    }
                }
                else if (obj is GeometryInstance)
                {
                    GeometryInstance geoInst = obj as GeometryInstance;

                    GeometryElement geoElem = geoInst.SymbolGeometry;

                    Transform transform = geoInst.Transform;

                    try
                    {
                        //geomSolid = SolidUtils.CreateTransformed(geomSolid, transform);
                        foreach (Solid sl in GetSolidOfElemFromGeObj(geoElem))
                        {
                            if (sl != null)
                            {
                                SolidOfElement.Add(SolidUtils.CreateTransformed(sl, transform));
                            }
                        }
                        //SolidOfElement.AddRange(getSolidOfElemFromGeObj(geoElem));
                    }
                    catch (Exception)
                    {

                        //throw;
                    }

                }

            }
            return SolidOfElement;
        }
        public static bool CheckfaceIsIntersectByExtractCurve(Face f1, Face f2)
        {
            //Check round 1
            foreach (CurveLoop i in f1.GetEdgesAsCurveLoops())
            {
                foreach (Curve ii in i)
                {
                    //SetComparisonResult.
                    SetComparisonResult rslt = f2.Intersect(ii);
                    if (f2.Intersect(ii) != SetComparisonResult.Disjoint)
                    {
                        return true;
                    }
                }
            }
            //Check round 2
            foreach (CurveLoop i in f2.GetEdgesAsCurveLoops())
            {
                foreach (Curve ii in i)
                {
                    SetComparisonResult rslt = f1.Intersect(ii);
                    if (f1.Intersect(ii) != SetComparisonResult.Disjoint)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool ElementsIsIntersect(Document doc, Element elm1, Element el2)
        {
            ElementId catID2 = el2.Category.Id;
            ElementId elID2 = el2.Id;
            FilteredElementCollector Collector = new FilteredElementCollector(doc);
            Collector.WhereElementIsNotElementType();
            ElementCategoryFilter catefilter = new ElementCategoryFilter(catID2);
            ElementIntersectsElementFilter intersectFilter = new ElementIntersectsElementFilter(elm1);
            ICollection<ElementId> intersectElms = Collector.WherePasses(catefilter).WherePasses(intersectFilter).ToElementIds();

            if (intersectElms.Contains(elID2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
