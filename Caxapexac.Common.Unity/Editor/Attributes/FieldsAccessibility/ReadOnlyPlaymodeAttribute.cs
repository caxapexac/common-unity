using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Fields
{
    /// <summary>
    /// Field will be Read-Only in Playmode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyPlaymodeAttribute : PropertyAttribute
    {
    }
}