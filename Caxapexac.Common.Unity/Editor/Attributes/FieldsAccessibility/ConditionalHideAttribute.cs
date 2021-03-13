using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public readonly string ConditionalSourceField;

        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public readonly bool HideInInspector = false;

        public ConditionalHideAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
        }
    }
}