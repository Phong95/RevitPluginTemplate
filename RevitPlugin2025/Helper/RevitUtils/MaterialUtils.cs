using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Helper.RevitUtils
{
    public class MaterialUtils
    {
        /// <summary>
        /// Extracts all materials used in an element.
        /// </summary>
        /// <param name="element">The Revit element to analyze.</param>
        /// <param name="doc">The document containing the element.</param>
        /// <returns>A list of materials used in the element.</returns>
        public static List<Material> GetMaterialsFromElement(Element element, Document doc)
        {
            HashSet<Material> materials = new HashSet<Material>();

            if (element == null || doc == null)
            {
                return materials.ToList();
            }

            // 1. Check if the element has a Material parameter
            Parameter materialParam = element.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM);
            if (materialParam != null && materialParam.HasValue)
            {
                Material material = doc.GetElement(materialParam.AsElementId()) as Material;
                if (material != null)
                {
                    materials.Add(material);
                }
            }

            // 2. Analyze the geometry for materials
            Options geomOptions = new Options();
            GeometryElement geometryElement = element.get_Geometry(geomOptions);

            if (geometryElement != null)
            {
                foreach (GeometryObject geomObj in geometryElement)
                {
                    if (geomObj is GeometryInstance instance)
                    {
                        GeometryElement instanceGeometry = instance.GetInstanceGeometry();
                        CollectMaterialsFromGeometry(instanceGeometry, materials, doc);
                    }
                    else
                    {
                        CollectMaterialsFromGeometry(new List<GeometryObject> { geomObj }, materials, doc);
                    }
                }
            }

            return materials.ToList();
        }

        /// <summary>
        /// Collects materials from geometry objects and adds them to the provided set.
        /// </summary>
        /// <param name="geometryObjects">A collection of geometry objects.</param>
        /// <param name="materials">A set to collect materials.</param>
        /// <param name="doc">The document containing the materials.</param>
        private static void CollectMaterialsFromGeometry(IEnumerable<GeometryObject> geometryObjects, HashSet<Material> materials, Document doc)
        {
            foreach (GeometryObject geomObj in geometryObjects)
            {
                if (geomObj is Solid solid)
                {
                    foreach (Face face in solid.Faces)
                    {
                        ElementId materialId = face.MaterialElementId;
                        if (materialId != ElementId.InvalidElementId)
                        {
                            Material material = doc.GetElement(materialId) as Material;
                            if (material != null)
                            {
                                materials.Add(material);
                            }
                        }
                    }
                }
                else if (geomObj is Mesh mesh)
                {
                    ElementId materialId = mesh.MaterialElementId;
                    if (materialId != ElementId.InvalidElementId)
                    {
                        Material material = doc.GetElement(materialId) as Material;
                        if (material != null)
                        {
                            materials.Add(material);
                        }
                    }
                }
            }
        }
    }
}
