using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TieFighter.Models
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class AncestorPathAttribute : Attribute
    {
        public AncestorPathAttribute(Type ancestor)
        {
            Ancestor = ancestor;
        }

        public Type Ancestor { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForiegnEntityAttribute : Attribute
    {
        public ForiegnEntityAttribute(string mapsToClassProperty)
        {
            MapsToClassProperty = mapsToClassProperty;
        }

        public string MapsToClassProperty { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsAncestorOfAttribute : Attribute
    {

    }
}
