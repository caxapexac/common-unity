using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.FieldsAccessibility
{
    /// <summary>
    /// Apply to MonoBehaviour field to assert that this field is assigned via inspector (not null, false, empty of zero) on playmode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MustBeAssignedAttribute : PropertyAttribute
    {
    }
}