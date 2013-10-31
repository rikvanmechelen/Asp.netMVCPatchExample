using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TestPatch.Models
{
    public abstract class AbstractModel
    {
        public void Patch(Object u)
        {
            var props = from p in this.GetType().GetProperties()
                        let attr = p.GetCustomAttribute(typeof(NotPatchableAttribute))
                        where attr == null
                        select p;
            foreach (var prop in props)
            {
                var val = prop.GetValue(this, null);
                if (val != null)
                    prop.SetValue(u, val);
            }
        }
    }

    public class NotPatchableAttribute : Attribute
    {
    }
}