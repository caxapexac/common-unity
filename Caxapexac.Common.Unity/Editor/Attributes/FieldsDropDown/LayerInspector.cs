using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Inspectors
{
    /// <summary>
    /// Helper for single layer selector.
    /// </summary>
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    internal sealed class LayerInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}