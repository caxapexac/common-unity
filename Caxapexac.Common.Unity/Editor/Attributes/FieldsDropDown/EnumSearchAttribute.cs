using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.FieldDropDown
{
    /// <summary>
    /// Put this attribute on a public (or SerializeField) enum in a
    /// MonoBehaviour or ScriptableObject to get an improved enum selector
    /// popup. The enum list is scrollable and can be filtered by typing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumSearchAttribute : PropertyAttribute
    {
    }
}