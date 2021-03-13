using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    /// <summary>
    /// Allows to display a dropdown of const, readonly, static fields and properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConstantsDropDownAttribute : PropertyAttribute
    {
        public readonly Type SelectFromType;

        public ConstantsDropDownAttribute(Type type)
        {
            SelectFromType = type;
        }
    }
}