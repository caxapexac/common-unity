using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.FieldDropDown
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!_checkedType) PropertyTypeWarning(property);
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var spriteLayerNames = GetSpriteLayerNames();
            HandleSpriteLayerSelectionUI(position, property, label, spriteLayerNames);
        }

        private bool _checkedType;

        private void PropertyTypeWarning(SerializedProperty property)
        {
            Debug.LogWarning($"Property <color=brown>{property.name}</color> in object <color=brown>{property.serializedObject.targetObject}</color> is of wrong type. Expected: Int");
            _checkedType = true;
        }

        private void HandleSpriteLayerSelectionUI(Rect position, SerializedProperty property, GUIContent label, string[] spriteLayerNames)
        {
            EditorGUI.BeginProperty(position, label, property);

            // To show which sprite layer is currently selected.
            int currentSpriteLayerIndex;
            bool layerFound = TryGetSpriteLayerIndexFromProperty(out currentSpriteLayerIndex, spriteLayerNames, property);

            if (!layerFound)
            {
                // Set to default layer. (Previous layer was removed)
                Debug.Log(
                    $"Property <color=brown>{property.name}</color> in object <color=brown>{property.serializedObject.targetObject}</color> is set to the default layer. Reason: previously selected layer was removed.");
                property.intValue = 0;
                currentSpriteLayerIndex = 0;
            }

            int selectedSpriteLayerIndex = EditorGUI.Popup(position, label.text, currentSpriteLayerIndex, spriteLayerNames);

            // Change property value if user selects a new sprite layer.
            if (selectedSpriteLayerIndex != currentSpriteLayerIndex)
            {
                property.intValue = SortingLayer.NameToID(spriteLayerNames[selectedSpriteLayerIndex]);
            }

            EditorGUI.EndProperty();
        }

        private bool TryGetSpriteLayerIndexFromProperty(out int index, string[] spriteLayerNames, SerializedProperty property)
        {
            // To keep the property's value consistent, after the layers have been sorted around.
            string layerName = SortingLayer.IDToName(property.intValue);

            // Return the index where on it matches.
            for (int i = 0; i < spriteLayerNames.Length; ++i)
            {
                if (spriteLayerNames[i].Equals(layerName))
                {
                    index = i;
                    return true;
                }
            }

            // The current layer was removed.
            index = -1;
            return false;
        }

        private string[] GetSpriteLayerNames()
        {
            string[] result = new string[SortingLayer.layers.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = SortingLayer.layers[i].name;
            }
            return result;
        }
    }
}