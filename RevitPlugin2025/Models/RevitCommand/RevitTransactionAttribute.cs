using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Models.RevitCommand
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =true)]
    public class RevitTransactionAttribute: TransactionAttribute
    {
        public RevitTransactionAttribute(TransactionMode mode):base(mode)
        {
        }
    }
}
