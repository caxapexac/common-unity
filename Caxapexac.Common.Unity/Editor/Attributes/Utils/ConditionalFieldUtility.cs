using System;
using Caxapexac.Common.Sharp.Editor.Attributes.Utils.EditorExtensions;
using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Inspectors
{
    public static class ConditionalFieldUtility
    {
        public static bool PropertyIsVisible(SerializedProperty property, bool inverse, string[] compareAgainst)
        {
            if (property == null) return true;

            string asString = property.AsStringValue().ToUpper();

            if (compareAgainst != null && compareAgainst.Length > 0)
            {
                var matchAny = CompareAgainstValues(asString, compareAgainst);
                if (inverse) matchAny = !matchAny;
                return matchAny;
            }

            bool someValueAssigned = asString != "FALSE" && asString != "0" && asString != "NULL";
            if (someValueAssigned) return !inverse;

            return inverse;
        }

        /// <summary>
        /// True if the property value matches any of the values in '_compareValues'
        /// </summary>
        private static bool CompareAgainstValues(string propertyValueAsString, string[] compareAgainst)
        {
            for (var i = 0; i < compareAgainst.Length; i++)
            {
                bool valueMatches = compareAgainst[i] == propertyValueAsString;

                // One of the value is equals to the property value.
                if (valueMatches) return true;
            }

            // None of the value is equals to the property value.
            return false;
        }

        public static SerializedProperty FindRelativeProperty(SerializedProperty property, string propertyName)
        {
            if (property.depth == 0) return property.serializedObject.FindProperty(propertyName);

            var path = property.propertyPath.Replace(".Array.data[", "[");
            var elements = path.Split('.');

            var nestedProperty = NestedPropertyOrigin(property, elements);

            // if nested property is null = we hit an array property
            if (nestedProperty == null)
            {
                var cleanPath = path.Substring(0, path.IndexOf('['));
                var arrayProp = property.serializedObject.FindProperty(cleanPath);
                var target = arrayProp.serializedObject.targetObject;

                var who = "Property <color=brown>" + arrayProp.name + "</color> in object <color=brown>" + target.name + "</color> caused: ";
                var warning = who + "Array fields is not supported by [ConditionalFieldAttribute]";

                Debug.LogWarning(warning, target);

                return null;
            }

            return nestedProperty.FindPropertyRelative(propertyName);
        }

        // For [Serialized] types with [Conditional] fields
        private static SerializedProperty NestedPropertyOrigin(SerializedProperty property, string[] elements)
        {
            SerializedProperty parent = null;

            for (int i = 0; i < elements.Length - 1; i++)
            {
                var element = elements[i];
                int index = -1;
                if (element.Contains("["))
                {
                    index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal))
                        .Replace("[", "")
                        .Replace("]", ""));
                    element = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                }

                parent = i == 0
                    ? property.serializedObject.FindProperty(element)
                    : parent != null
                        ? parent.FindPropertyRelative(element)
                        : null;

                if (index >= 0 && parent != null) parent = parent.GetArrayElementAtIndex(index);
            }

            return parent;
        }


        public static bool BehaviourPropertyIsVisible(MonoBehaviour behaviour, string propertyName, ConditionalFieldAttribute appliedAttribute)
        {
            if (string.IsNullOrEmpty(appliedAttribute.FieldToCheck)) return true;

            var so = new SerializedObject(behaviour);
            var property = so.FindProperty(propertyName);
            var targetProperty = FindRelativeProperty(property, appliedAttribute.FieldToCheck);

            return PropertyIsVisible(targetProperty, appliedAttribute.Inverse, appliedAttribute.CompareValues);
        }
    }
}