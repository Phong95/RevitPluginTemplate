using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class ElementColorBaseUtils
    {
        public static void OverideColor(Document doc, Element ele, int transparency)
        {
            try
            {
                ICollection<Element> fec = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).ToElements();
                ElementId patternid = null;
                foreach (FillPatternElement pattern in fec)
                {

                    if (pattern.GetFillPattern().IsSolidFill)
                    {
                        patternid = pattern.Id;
                        break;
                    }
                }
                Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(128, 255, 255);
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();

#if REVIT2018
                ogs.SetProjectionFillPatternId(patternid);
                ogs.SetProjectionFillColor(color);
                ogs.SetSurfaceTransparency(transparency);

#else
                ogs.SetSurfaceForegroundPatternId(patternid);
                ogs.SetSurfaceForegroundPatternColor(color);
                ogs.SetSurfaceTransparency(transparency);
#endif

                doc.ActiveView.SetElementOverrides(ele.Id, ogs);
            }
            catch (Exception ex)
            {
            }

        }
        public static void OverideColor(Document doc, List<Element> lstEle, int transparency, int red, int green, int blue)
        {
            try
            {
                ICollection<Element> fec = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).ToElements();
                ElementId patternid = null;
                foreach (FillPatternElement ele in fec)
                {

                    if (ele.GetFillPattern().IsSolidFill)
                    {
                        patternid = ele.Id;
                        break;
                    }
                }

                Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();

#if REVIT2018
                ogs.SetProjectionFillPatternId(patternid);
                ogs.SetProjectionFillColor(color);
                ogs.SetSurfaceTransparency(transparency);

#else
                ogs.SetSurfaceForegroundPatternId(patternid);
                ogs.SetSurfaceForegroundPatternColor(color);
                ogs.SetSurfaceTransparency(transparency);

#endif

                foreach (Element ele in lstEle)
                {
                    doc.ActiveView.SetElementOverrides(ele.Id, ogs);
                }
            }
            catch (Exception ex)
            {
            }

        }
        public static void OverideCategoryColor(Document doc, ElementId categoryId, int transparency, int red, int green, int blue)
        {
            try
            {
                ICollection<Element> fec = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).ToElements();
                ElementId patternid = null;
                foreach (FillPatternElement ele in fec)
                {

                    if (ele.GetFillPattern().IsSolidFill)
                    {
                        patternid = ele.Id;
                        break;
                    }
                }

                Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();

#if REVIT2018
                ogs.SetProjectionFillPatternId(patternid);
                ogs.SetProjectionFillColor(color);
                ogs.SetSurfaceTransparency(transparency);

#else
                ogs.SetSurfaceForegroundPatternId(patternid);
                ogs.SetSurfaceForegroundPatternColor(color);
                ogs.SetSurfaceTransparency(transparency);

#endif
                doc.ActiveView.SetCategoryOverrides(categoryId, ogs);
            }
            catch (Exception ex)
            {
            }

        }
        public static void ClearOverideColor(Document doc, Element ele)
        {
            try
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                doc.ActiveView.SetElementOverrides(ele.Id, ogs);
            }
            catch (Exception ex)
            {
            }
        }
        public static void ClearOverideCategoryColor(Document doc, ElementId categoryId)
        {
            try
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                doc.ActiveView.SetCategoryOverrides(categoryId, ogs);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
