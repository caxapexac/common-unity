using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    /// <summary>
    /// Create Popup with predefined values for string, int or float property
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ValuesDropDownAttribute : PropertyAttribute
    {
        public readonly object[] ValuesArray;

        public ValuesDropDownAttribute(params object[] definedValues)
        {
            ValuesArray = definedValues;
        }
    }
}