using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    /// <summary>
    /// Displays one inspector inside of another
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayInspectorInsideAttribute : PropertyAttribute
    {
        public readonly bool DisplayScript;

        public DisplayInspectorInsideAttribute(bool displayScriptField = true)
        {
            DisplayScript = displayScriptField;
        }
    }
}