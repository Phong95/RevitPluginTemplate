using Autodesk.Revit.Attributes;
using RevitPlugin2025.Extensions;
using RevitPlugin2025.Models.RevitCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitPlugin2025.Tools.HelloWorld
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorld : RevitBaseCommand
    {
        public override IRevitBaseCommandResult Execute()
        {
            System.Windows.MessageBox.Show("Hello World!");
            return this.OkResult();
        }
    }
}
