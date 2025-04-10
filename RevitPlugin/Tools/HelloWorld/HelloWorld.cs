using Autodesk.Revit.Attributes;
using RevitPlugin.Extensions;
using RevitPlugin.Models.RevitCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitPlugin.Tools.HelloWorld
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorld : RevitBaseCommand
    {
        public override IRevitBaseCommandResult Execute()
        {
            MessageBox.Show("Hello World!");
            return this.OkResult();
        }
    }
}
