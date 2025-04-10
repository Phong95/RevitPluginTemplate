using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitPlugin.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class GetElementBaseUtils
    {
        public class CategorySelectionFilter : ISelectionFilter
        {

            BuiltInCategory _builtIncategory;
            public CategorySelectionFilter(BuiltInCategory builtIncategory)
            {
                _builtIncategory = builtIncategory;
            }
            public bool AllowElement(Element element)
            {
                if (element.Category.IsNotNull())
                {
                    if (element.Category.Id.IntegerValue == (int)_builtIncategory)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }

            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
        public class CustomMultipleSelectionFilter : ISelectionFilter
        {
            List<string> listcategory;
            public CustomMultipleSelectionFilter(List<string> ListCategory)
            {
                listcategory = ListCategory;
            }
            public bool AllowElement(Element element)
            {
                if (listcategory.Contains(element.Category.Name))
                {
                    return true;
                }
                return false;
            }
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
        public class CustomSelectionFilterLink : ISelectionFilter
        {

            Autodesk.Revit.DB.Document doc = null;
            string category;
            public CustomSelectionFilterLink(Document document, string categoryname)
            {
                doc = document;
                category = categoryname;
            }
            public CustomSelectionFilterLink(Document document)
            {
                doc = document;
            }
            public bool AllowElement(Element element)
            {
                return true;
            }

            public bool AllowReference(Reference reference, XYZ point)
            {
                RevitLinkInstance revitlinkinstance = doc.GetElement(reference) as RevitLinkInstance;
                Autodesk.Revit.DB.Document docLink = revitlinkinstance.GetLinkDocument();
                Element element = docLink.GetElement(reference.LinkedElementId);
                if (element.Category.Name == category)
                {
                    return true;
                }
                return false;
            }
        }
        public class CustomMultipleSelectionFilterLink : ISelectionFilter
        {

            Autodesk.Revit.DB.Document doc = null;
            List<string> listcategory;
            public CustomMultipleSelectionFilterLink(Document document, List<string> ListCategory)
            {
                doc = document;
                listcategory = ListCategory;
            }

            public bool AllowElement(Element element)
            {
                return true;
            }

            public bool AllowReference(Reference reference, XYZ point)
            {
                RevitLinkInstance revitlinkinstance = doc.GetElement(reference) as RevitLinkInstance;
                Autodesk.Revit.DB.Document docLink = revitlinkinstance.GetLinkDocument();
                Element element = docLink.GetElement(reference.LinkedElementId);
                if (listcategory.Contains(element.Category.Name))
                {
                    return true;
                }
                return false;
            }
        }
        public static FilteredElementCollector GetStructuralElements(Document doc)
        {
            // what categories of family instances
            // are we interested in?

            BuiltInCategory[] bics = new BuiltInCategory[] {
    BuiltInCategory.OST_StructuralColumns,
    BuiltInCategory.OST_StructuralFraming,
    BuiltInCategory.OST_StructuralFoundation,
    BuiltInCategory.OST_GenericModel
  };

            IList<ElementFilter> a
              = new List<ElementFilter>(bics.Count());

            foreach (BuiltInCategory bic in bics)
            {
                a.Add(new ElementCategoryFilter(bic));
            }

            LogicalOrFilter categoryFilter
              = new LogicalOrFilter(a);

            LogicalAndFilter familyInstanceFilter
              = new LogicalAndFilter(categoryFilter,
                new ElementClassFilter(
                  typeof(FamilyInstance)));

            IList<ElementFilter> b
              = new List<ElementFilter>(6);

            b.Add(new ElementClassFilter(
              typeof(Wall)));

            b.Add(new ElementClassFilter(
              typeof(Floor)));

            b.Add(new ElementClassFilter(
              typeof(PointLoad)));

            b.Add(new ElementClassFilter(
              typeof(LineLoad)));

            b.Add(new ElementClassFilter(
              typeof(AreaLoad)));

            b.Add(familyInstanceFilter);

            LogicalOrFilter classFilter
              = new LogicalOrFilter(b);

            FilteredElementCollector collector
              = new FilteredElementCollector(doc);

            collector.WherePasses(classFilter);

            return collector;
        }
        public static IList<Element> GetElementsInView(Document doc)
        {
            FilteredElementCollector allElementsInView = new FilteredElementCollector(doc, doc.ActiveView.Id);
            return allElementsInView.ToElements();
        }
        public static IList<Element> GetInstanceElementsInView(Document doc)
        {
            FilteredElementCollector allElementsInView = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(FamilyInstance));
            return allElementsInView.ToElements();
        }
        public static List<Element> GetElementIntersect(Element ds, Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementIntersectsElementFilter eleinele = new ElementIntersectsElementFilter(ds);
            collector.WherePasses(eleinele);
            List<Element> EleList = collector.ToList();
            return EleList;
        }
        public static List<Element> GetElementIntersect(Element ds, ICollection<ElementId> listAnother, Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc, listAnother);
            ElementIntersectsElementFilter eleinele = new ElementIntersectsElementFilter(ds);
            collector.WherePasses(eleinele);
            List<Element> EleList = collector.ToList();
            return EleList;
        }
        public static List<Element> GetElementContact(Element ele, Document doc, Document docLink = null)
        {

            FilteredElementCollector collector = null;
            if (!docLink.IsNotNull())
            {
                collector = new FilteredElementCollector(doc);
            }
            else
            {
                collector = new FilteredElementCollector(docLink);
            }
            BoundingBoxXYZ bb = ele.get_BoundingBox(null);
            Outline outline = new Outline(bb.Min, bb.Max);
            BoundingBoxIntersectsFilter bbfilter = new BoundingBoxIntersectsFilter(outline);
            collector.WherePasses(bbfilter);

            List<Element> EleList = new List<Element>();
            foreach (Element i in collector)
            {
                EleList.Add(i);
            }
            return EleList;
        }
        public static IList<Element> GetElementContact(Element ele, ICollection<ElementId> listAnother, Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc, listAnother);
            BoundingBoxXYZ bb = ele.get_BoundingBox(null);
            Outline outline = new Outline(bb.Min, bb.Max);
            BoundingBoxIntersectsFilter bbfilter = new BoundingBoxIntersectsFilter(outline, 5 / 304.8);
            collector.WherePasses(bbfilter);

            IList<Element> EleList = collector.ToElements();

            return EleList;
        }
        public static List<Element> GetSelectedElement(UIDocument uidoc)
        {
            Document doc = uidoc.Document;
            List<Element> result = new List<Element>();
            ICollection<ElementId> elementidss = uidoc.Selection.GetElementIds();
            foreach (ElementId id in elementidss)
            {
                Element ele = doc.GetElement(id);
                result.Add(ele);
            }
            return result;
        }
        public static List<Element> ReferencesToElement(Document doc, IList<Reference> listreff)
        {
            return (from x in listreff select doc.GetElement(x)).ToList<Element>();
        }
        public static List<Element> IdsToElement(Document doc, ICollection<ElementId> ListSelected)
        {
            return (from x in ListSelected select doc.GetElement(x)).ToList<Element>();
        }

    }
}
