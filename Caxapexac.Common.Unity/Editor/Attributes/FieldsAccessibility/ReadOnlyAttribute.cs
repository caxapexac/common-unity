using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    /// <summary>
    /// Makes property grey-colored and unable to change
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}