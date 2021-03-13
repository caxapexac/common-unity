using System;
using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Inspectors
{
    [CustomPropertyDrawer(typeof(ValuesDropDownAttribute))]
    public class ValuesDropDownInspector : PropertyDrawer
    {
        private ValuesDropDownAttribute _dropDownAttribute;
        private Type _variableType;
        private string[] _values;
        private int _selectedValueIndex = -1;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_dropDownAttribute == null) Initialize(property);
            if (_values == null || _values.Length == 0 || _selectedValueIndex < 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginChangeCheck();
            _selectedValueIndex = EditorGUI.Popup(position, label.text, _selectedValueIndex, _values);
            if (EditorGUI.EndChangeCheck()) ApplyNewValue(property);
        }

        private void Initialize(SerializedProperty property)
        {
            _dropDownAttribute = (ValuesDropDownAttribute)attribute;
            if (_dropDownAttribute.ValuesArray == null || _dropDownAttribute.ValuesArray.Length == 0) return;
            _variableType = _dropDownAttribute.ValuesArray[0].GetType();
            if (TypeMismatch(property)) return;
            _values = new string[_dropDownAttribute.ValuesArray.Length];
            for (int i = 0; i < _dropDownAttribute.ValuesArray.Length; i++)
            {
                _values[i] = _dropDownAttribute.ValuesArray[i].ToString();
            }
            _selectedValueIndex = GetSelectedIndex(property);
        }

        private int GetSelectedIndex(SerializedProperty property)
        {
            for (var i = 0; i < _values.Length; i++)
            {
                if (IsString && property.stringValue == _values[i]) return i;
                if (IsInt && property.intValue == Convert.ToInt32(_values[i])) return i;
                if (IsFloat && Mathf.Approximately(property.floatValue, Convert.ToSingle(_values[i]))) return i;
            }
            return 0;
        }

        private bool TypeMismatch(SerializedProperty property)
        {
            if (IsString && property.propertyType != SerializedPropertyType.String) return true;
            if (IsInt && property.propertyType != SerializedPropertyType.Integer) return true;
            if (IsFloat && property.propertyType != SerializedPropertyType.Float) return true;
            return false;
        }

        private void ApplyNewValue(SerializedProperty property)
        {
            if (IsString)
                property.stringValue = _values[_selectedValueIndex];
            else if (IsInt)
                property.intValue = Convert.ToInt32(_values[_selectedValueIndex]);
            else if (IsFloat) property.floatValue = Convert.ToSingle(_values[_selectedValueIndex]);
            property.serializedObject.ApplyModifiedProperties();
        }

        private bool IsString
        {
            get => _variableType == typeof(string);
        }

        private bool IsInt
        {
            get => _variableType == typeof(int);
        }

        private bool IsFloat
        {
            get => _variableType == typeof(float);
        }
    }
}