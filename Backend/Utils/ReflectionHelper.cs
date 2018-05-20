using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Utils
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns the corresponding Value for the given propname
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName">as type.{prop1}*</param>
        /// <returns></returns>
        public static object GetPropValue(this object obj, string propName)
        {
            if (obj != null)
            {
                string[] nameParts = propName.Split('.');

                // skip first part, because first element represents the given type of the object (e.g. Company)
                foreach (String part in nameParts.Skip(1))
                {
                    PropertyInfo info = obj.GetType().GetProperty(part);
                    if (info == null) { return null; }

                    obj = info.GetValue(obj, null);
                }

            }
            return obj;
        }
    }
}
