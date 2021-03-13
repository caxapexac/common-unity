// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.CustomEditor
{
    /// <summary>
    /// Default inspector for all objects, add drag & drop ordering behaviour for arrays / lists.
    /// </summary>
    [CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(Object), true, isFallback = true)]
    internal sealed class DefaultComponentInspector : UnityEditor.Editor
    {
        private static Dictionary<string, ReorderableListProperty> _reorderableLists;

        private void OnEnable()
        {
            if (_reorderableLists == null)
            {
                _reorderableLists = new Dictionary<string, ReorderableListProperty>(64);
            }
            _reorderableLists.Clear();
        }

        private void OnDisable()
        {
            if (_reorderableLists != null)
            {
                _reorderableLists.Clear();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var savedColor = GUI.color;
            var savedEnabled = GUI.enabled;
            var property = serializedObject.GetIterator();
            var isValid = property.NextVisible(true);
            if (isValid)
            {
                do
                {
                    GUI.color = savedColor;
                    GUI.enabled = savedEnabled;
                    DrawProperty(property);
                } while (property.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperty(SerializedProperty property)
        {
            if (property.name.Equals("m_Script")
                && property.type.Equals("PPtr<MonoScript>")
                && property.propertyType == SerializedPropertyType.ObjectReference
                && property.propertyPath.Equals("m_Script"))
            {
                GUI.enabled = false;
            }
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                DrawArray(property);
            }
            else
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private void DrawArray(SerializedProperty property)
        {
            if (EditorGUILayout.Foldout(property.isExpanded, property.displayName, true) != property.isExpanded)
            {
                property.isExpanded = !property.isExpanded;
            }
            if (property.isExpanded)
            {
                GetReorderableList(property).List.DoLayoutList();
            }
        }

        public static void DrawReorderableList(ReorderableListProperty listProperty)
        {
            var prop = listProperty.Property;
            if (EditorGUILayout.Foldout(prop.isExpanded, prop.displayName, true) != prop.isExpanded)
            {
                prop.isExpanded = !prop.isExpanded;
            }
            if (prop.isExpanded)
            {
                listProperty.List.DoLayoutList();
            }
        }

        private ReorderableListProperty GetReorderableList(SerializedProperty property)
        {
            ReorderableListProperty retVal;
            if (_reorderableLists.TryGetValue(property.name, out retVal))
            {
                retVal.Property = property;
                return retVal;
            }
            retVal = new ReorderableListProperty(property);
            _reorderableLists[property.name] = retVal;
            return retVal;
        }


        public class ReorderableListProperty
        {
            public ReorderableList List { get; private set; }

            public SerializedProperty Property
            {
                get => List.serializedProperty;
                set => List.serializedProperty = value;
            }

            public ReorderableListProperty(SerializedProperty property)
            {
                List = new ReorderableList(property.serializedObject, property, true, false, true, true);
                List.headerHeight = 0f;
                List.onCanRemoveCallback += OnCanRemove;
                List.drawElementCallback += OnDrawElement;
                List.elementHeightCallback += OnElementHeight;
            }

            private bool OnCanRemove(ReorderableList list)
            {
                return List.count > 0;
            }

            private float OnElementHeight(int id)
            {
                return 4f
                    + Mathf.Max(EditorGUIUtility.singleLineHeight,
                        EditorGUI.GetPropertyHeight(Property.GetArrayElementAtIndex(id), GUIContent.none, true));
            }

            private void OnDrawElement(Rect rect, int index, bool active, bool focused)
            {
                if (Property.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUI.LabelField(rect, Property.GetArrayElementAtIndex(index).displayName);
                }

                rect.height = EditorGUI.GetPropertyHeight(Property.GetArrayElementAtIndex(index), GUIContent.none, true);
                EditorGUI.PropertyField(rect, Property.GetArrayElementAtIndex(index), GUIContent.none, true);
            }
        }
    }
}