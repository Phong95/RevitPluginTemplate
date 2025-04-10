using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.ProjectParameterData
{
    public class ProjectParameterData
    {
        public Definition Definition = null;
        public ElementBinding Binding = null;
        public string Name = null;                // Needed because accsessing the Definition later may produce an error.
        public bool IsSharedStatusKnown = false;  // Will probably always be true when the data is gathered
        public bool IsShared = false;
        public string GUID = null;
    }
}
