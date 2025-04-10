using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Helper.RevitUtils
{
    public static class RevitFamilyBaseUtils
    {
        public static List<FamilySymbol> FindFamilyTypesByDocument(Document doc, BuiltInCategory cat)
        {
            return new FilteredElementCollector(doc)
                  .WherePasses(new ElementClassFilter(typeof(FamilySymbol)))
                  .WherePasses(new ElementCategoryFilter(cat))
                  .Cast<FamilySymbol>().ToList();
        }
        public static List<Family> FindFamilysByDocument(Document doc, BuiltInCategory cat)
        {
            ElementId newElementId = new ElementId(cat);
            List<Family> listFamily = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().ToList();
            return listFamily.Where(x => x.FamilyCategoryId == newElementId).ToList();
        }
        public static string FindFamilyTypesByElement(Element ele)
        {
            string result = null;
            if (ele is FamilyInstance)
            {
                result = ((Autodesk.Revit.DB.FamilyInstance)ele).Symbol.Name;
            }
            else
            {
                result = ele.Name;
            }
            return result;
        }
    }
}
