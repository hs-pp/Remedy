using System;

namespace RemedySystem
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RemedyTypeAttribute : Attribute
    {
        public string TypeName { get; }
        
        public RemedyTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}