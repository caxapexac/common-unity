using System;
using UnityEngine;

#if UNITY_EDITOR

#endif


namespace Caxapexac.Common.Sharp.Editor.Attributes.Fields
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TagAttribute : PropertyAttribute
    {
    }
}