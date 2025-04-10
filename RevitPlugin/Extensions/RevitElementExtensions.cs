using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Extensions
{
    public static class RevitElementExtensions
    {
        public static bool IsPhysicalElement(this Element e)

        {

            if (e.Category == null) return false;

            if (e.ViewSpecific) return false;

            // exclude specific unwanted categories

            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_HVAC_Zones) return false;
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_Lines) return false;
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_Cameras) return false;
            //if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_Mass) return false;
            //
            return e.Category.CategoryType == CategoryType.Model;

        }
        public static bool IsFamilyInstanceAndNoneStructure(this Element e)

        {
            if (!(e is FamilyInstance)) return false;
            if (((FamilyInstance)e).StructuralType == Autodesk.Revit.DB.Structure.StructuralType.NonStructural)
            {
                //ignore panels and mullions from curtain system (we will get later as structural element)
                if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainWallPanels) return false;
                if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainWallMullions) return false;
                return true;
            }
            return false;

        }
        public static bool IsTheRest(this Element e)
        {
            if (e.Category == null) return false;

            #region exclude specific unwanted categories
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainWallPanels) return false;
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainWallMullions) return false;
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainGridsWall) return false;
            if (((BuiltInCategory)e.Category.Id.IntegerValue) == BuiltInCategory.OST_CurtainGridsRoof) return false;
            #endregion
            if (!(e is FamilyInstance)) return true;
            if (((FamilyInstance)e).StructuralType == Autodesk.Revit.DB.Structure.StructuralType.NonStructural)
            {
                return false;
            }

            return true;

        }

    }
}
