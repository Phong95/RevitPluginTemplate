using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Extensions
{
    public static class ObjectExtensions
    {
        [Obsolete("This method is deprecated. Use 'obj != null' or 'obj is not null' instead. support from c# 9.0")]
        public static bool IsNotNull(this object obj)
        {
            return !(obj is null);
        }
        public static T GetPropValue<T>(this object src, string propName)
        {
            return (T)src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
