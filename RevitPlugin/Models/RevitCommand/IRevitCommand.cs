using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Models.RevitCommand
{
    public interface IRevitCommand
    {
        IRevitBaseCommandResult PreExecute();
        IRevitBaseCommandResult Execute();
        IRevitBaseCommandResult PostExecute();
    }
}
