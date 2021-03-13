// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Inspectors
{
    /// <summary>
    /// Helper for custom flags selector.
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    internal sealed class EnumFlagsInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}