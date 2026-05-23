using System;

namespace ParrotCode.Native.Inspector
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ButtonAttribute: Attribute
    {
        private readonly string label;
        public string Label => label;

        public ButtonAttribute(){}

        public ButtonAttribute(string label)
            => this.label = label;
    }
}
