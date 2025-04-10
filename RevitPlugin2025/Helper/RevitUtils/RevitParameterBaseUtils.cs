using Autodesk.Revit.DB;
using RevitPlugin2025.Models.ProjectParameterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitPlugin2025.Helper.RevitUtils
{
    public static class RevitParameterBaseUtils
    {
        public static Boolean CheckIfElementHadParameter(Element ele, string ParameterName)
        {
            Boolean result = false;
            if (ele is FamilyInstance)
            {
                FamilySymbol fs = ((Autodesk.Revit.DB.FamilyInstance)ele).Symbol;
                Family f = fs.Family;
                if (ele.LookupParameter(ParameterName) != null)
                {
                    result = true;
                }
                else if (fs.LookupParameter(ParameterName) != null)
                {
                    result = true;

                }
                else if (f.LookupParameter(ParameterName) != null)
                {
                    result = true;
                }
            }
            else
            {
                Parameter para = ele.LookupParameter(ParameterName);
                if (para != null)
                {
                    result = true;
                }
            }
            return result;
        }
        public static string GetStringValueParameter(Element ele, Definition definition)
        {
            string result = null;
            if (ele is FamilyInstance)
            {
                FamilySymbol fs = ((Autodesk.Revit.DB.FamilyInstance)ele).Symbol;
                Family f = fs.Family;
                var instanceParameterValue = GetValueParameter(ele, definition);
                var typeParameterValue = GetValueParameter(fs, definition);
                var familyParameterValue = GetValueParameter(f, definition);
                if (instanceParameterValue != null)
                {
                    result = instanceParameterValue;
                }
                else if (typeParameterValue != null)
                {
                    result = typeParameterValue;
                }
                else if (familyParameterValue != null)
                {
                    result = familyParameterValue;
                }
            }
            else
            {
                result = GetValueParameter(ele, definition);
            }

            return result;
        }
        public static object GetParameterValueFromBuiltinParameter(Element e, BuiltInParameter paraIndex)
        {
            Parameter p = e.get_Parameter(paraIndex);
            if (p == null)
            {
                return null;
            }
            object paravalue = null;
            switch (p.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Double:
                    paravalue = p.AsDouble();
                    break;
                case StorageType.Integer:
                    paravalue = p.AsInteger();
                    break;
                case StorageType.ElementId:
                    paravalue = p.AsElementId();
                    break;
                case StorageType.String:
                    paravalue = p.AsString();
                    break;
            }
            return paravalue;
        }
        public static string GetValueParameter(Element ele, Definition definition)
        {
            string result = null;
            Parameter para = ele.get_Parameter(definition);
            if (para == null)
            {
                return result;
            }
            if (para.StorageType == StorageType.Double)
            {
#if REVIT2021||REVIT2022|| REVIT2023|| REVIT2024|| REVIT2025
                if (para.GetUnitTypeId() == UnitTypeId.Meters)
                {
                    result = UnitBaseUtils.F3toM3(para.AsDouble()).ToString();
                }
                else if (para.GetUnitTypeId() == UnitTypeId.Millimeters)
                {
                    result = UnitBaseUtils.FtToMm(para.AsDouble()).ToString();
                }
                else if (para.GetUnitTypeId() == UnitTypeId.SquareMeters)
                {
                    result = UnitBaseUtils.F2toM2(para.AsDouble()).ToString();
                }
                else
                {
                    result = para.AsDouble().ToString();
                }
#else
                if (para.DisplayUnitType == DisplayUnitType.DUT_CUBIC_METERS)
                {
                    result = UnitBaseUtils.F3toM3(para.AsDouble()).ToString();
                }
                else if (para.DisplayUnitType == DisplayUnitType.DUT_MILLIMETERS)
                {
                    result = UnitBaseUtils.FtToMm(para.AsDouble()).ToString();
                }
                else if (para.DisplayUnitType == DisplayUnitType.DUT_SQUARE_METERS)
                {
                    result = UnitBaseUtils.F2toM2(para.AsDouble()).ToString();
                }
                else
                {
                    result = para.AsDouble().ToString();
                }
#endif

            }
            else if (para.StorageType == StorageType.String)
            {
                result = para.AsString();
            }
            else if (para.StorageType == StorageType.Integer)
            {
                result = para.AsInteger().ToString();
            }
            else
            {
                result = para.AsValueString();
            }
            return result;

        }
        public static string GetStringValueParameter2(Element ele, dynamic parameterGuid)
        {
            string result = null;
            Parameter para = ele.get_Parameter(parameterGuid);
            if (para == null)
            {
                return result;
            }
            if (para.StorageType == StorageType.String)
            {
                result = para.AsString();
            }
            else
            {
                result = para.AsValueString();
            }
            return result;

        }
        public static List<Parameter> GetAllParameter(Element ele)
        {
            List<Parameter> result = new List<Parameter>();
            List<Parameter> listInstanceParameters = ele.GetOrderedParameters() as List<Parameter>;
            //string typeName = ele.GetType().FullName;
            if (ele is Autodesk.Revit.DB.FamilyInstance)
            {
                FamilySymbol fs = ((Autodesk.Revit.DB.FamilyInstance)ele).Symbol;
                Family f = fs.Family;
                //list type parameters
                List<Parameter> list1 = (from Parameter p in fs.Parameters select p).ToList();
                //list family type parameters
                List<Parameter> list2 = (from Parameter p in f.Parameters select p).ToList();
                var newList = listInstanceParameters.Concat(list1).Concat(list2).ToList();
                result = newList;
            }
            else
            {
                result = listInstanceParameters;
            }
            return result;
        }
        public static List<Parameter> GetAllInstanceParameter(Element ele)
        {
            List<Parameter> listInstanceParameters = ele.GetOrderedParameters() as List<Parameter>;
            return listInstanceParameters.Where(x => x.StorageType == StorageType.String).ToList();
        }
        public static List<Parameter> GetAllInstanceParameter2(Element ele)
        {
            List<Parameter> listInstanceParameters = ele.GetOrderedParameters() as List<Parameter>;
            return listInstanceParameters;
        }
        public static List<ProjectParameterData> GetProjectParameterData(Document doc)
        {
            // Following good SOA practices, first validate incoming parameters
            List<ProjectParameterData> result
  = new List<ProjectParameterData>();
            if (doc == null)
            {
                MessageBox.Show("Cant not run in family.");
                return result;
            }

            if (doc.IsFamilyDocument)
            {
                MessageBox.Show("Cant not run in family.");
                return result;
            }



            BindingMap map = doc.ParameterBindings;
            DefinitionBindingMapIterator it
              = map.ForwardIterator();
            it.Reset();
            while (it.MoveNext())
            {
                ProjectParameterData newProjectParameterData
                  = new ProjectParameterData();

                newProjectParameterData.Definition = it.Key;
                newProjectParameterData.Name = it.Key.Name;
                newProjectParameterData.Binding = it.Current
                  as ElementBinding;

                result.Add(newProjectParameterData);
            }
            return result;
        }
        public static void CreateSharedParameterFile(string sharedParameterFile)
        {
            System.IO.FileStream fileStream = System.IO.File.Create(sharedParameterFile);
            fileStream.Close();
        }
        public static DefinitionFile SetAndOpenSharedParameterFile(Autodesk.Revit.ApplicationServices.Application application, string sharedParameterFile)
        {
            // set the path of shared parameter file to current Revit
            application.SharedParametersFilename = sharedParameterFile;
            // open the file
            return application.OpenSharedParameterFile();
        }
    }
}
