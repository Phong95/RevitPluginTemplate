using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Helper.RevitUtils
{
    public static class SchemaBaseUtils
    {
        public static Schema CreateSchema(string schemaName, string description, List<string> listField)
        {
            Guid schemaGuid = new Guid("0DC954AE-ADEF-41c1-8D38-EB5B8465D255");

            SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);

            // set read access
            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);

            // set write access
            schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);

            // set schema name
            schemaBuilder.SetSchemaName(schemaName);

            // set documentation
            schemaBuilder.SetDocumentation(description);

            // create a field to store the bool value
            foreach (string fieldName in listField)
            {
                schemaBuilder.AddSimpleField(fieldName, typeof(String));

            }

            // register the schema
            Schema schema = schemaBuilder.Finish();

            return schema;
        }

        public static void AddSchemaEntityToProject(Schema schema, Document doc, Dictionary<string, string> setField)
        {
            ProjectInfo projectInfo = doc.ProjectInformation;
            // create an entity object (object) for this schema (class)
            Entity entity = new Entity(schema);
            foreach (KeyValuePair<string, string> keyvalue in setField)
            {
                // get the field from schema
                Field fieldPreventDeletion = schema.GetField(keyvalue.Key);

                // set the value for entity
                entity.Set(fieldPreventDeletion, keyvalue.Value);
            }

            // store the entity on the element
            projectInfo.SetEntity(entity);
        }
        public static Schema GetSchema(string schemaName)
        {
            Schema schema = null;
            IList<Schema> schemas = Schema.ListSchemas();
            if (schemas != null && schemas.Count > 0)
            {
                // get schema
                foreach (Schema s in schemas)
                {
                    if (s.SchemaName == schemaName)
                    {
                        schema = s;
                        break;
                    }
                }
            }
            return schema;
        }

        public static bool SchemaExist(string schemaName)
        {
            bool result = false;
            if (GetSchema(schemaName) != null)
            {
                result = true;
            }
            return result;
        }
    }
}
