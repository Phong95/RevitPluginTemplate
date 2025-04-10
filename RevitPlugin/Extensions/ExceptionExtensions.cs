using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Extensions
{
    public static class ExceptionExtensions
    {
        public static int LineNumber(this Exception ex)
        {
            int n;
            int i = ex.StackTrace.LastIndexOf(" ");
            if (i > -1)
            {
                string s = ex.StackTrace.Substring(i + 1);
                if (int.TryParse(s, out n))
                    return n;
            }
            return -1;
        }
        public static string ClassName(this Exception ex)
        {
            string result = "Cant Define Class";
            if(ex.TargetSite.IsNotNull() && ex.TargetSite.DeclaringType.IsNotNull())
            {
                result = ex.TargetSite.DeclaringType.Name;
            }
            return result;
        }
    }
}
