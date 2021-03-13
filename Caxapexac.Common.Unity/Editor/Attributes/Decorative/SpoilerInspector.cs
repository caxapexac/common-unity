using System;
using System.Collections.Generic;
using Caxapexac.Common.Sharp.Editor.Attributes.Utils;
using MyBox.Internal;
using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.FieldsAccessibility
{
    public class SpoilerInspector
    {
        private Dictionary<string, CacheFoldProp> _cacheFolds = new Dictionary<string, CacheFoldProp>();
        private List<SerializedProperty> _props = new List<SerializedProperty>();
        private bool _initialized;

        private UnityEngine.Object _target;
        private SerializedObject _serializedObject;

        public SpoilerInspector(UnityEngine.Object target, SerializedObject serializedObject)
        {
            _target = target;
            _serializedObject = serializedObject;
        }


        public void OnDisable()
        {
            if (_target == null) return;

            foreach (var c in _cacheFolds)
            {
                EditorPrefs.SetBool(string.Format($"{c.Value.atr.name}{c.Value.props[0].name}{_target.name}"), c.Value.expanded);
                c.Value.Dispose();
            }
        }

        public void Update()
        {
            _serializedObject.Update();
            Setup();
        }

        public bool OverrideInspector
        {
            get => _props.Count > 0;
        }

        public void OnInspectorGUI()
        {
            Header();
            Body();

            _serializedObject.ApplyModifiedProperties();
        }

        private void Header()
        {
            using (new EditorGUI.DisabledScope("m_Script" == _props[0].propertyPath))
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_props[0], true);
                EditorGUILayout.Space();
            }
        }

        private void Body()
        {
            foreach (var pair in _cacheFolds)
            {
                EditorGUILayout.BeginVertical(StyleFramework.Box);
                Foldout(pair.Value);
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel = 0;
            }

            EditorGUILayout.Space();

            for (var i = 1; i < _props.Count; i++)
            {
                EditorGUILayout.PropertyField(_props[i], true);
            }

            EditorGUILayout.Space();
        }

        private void Foldout(CacheFoldProp cache)
        {
            cache.expanded = EditorGUILayout.Foldout(cache.expanded, cache.atr.name, true,
                StyleFramework.Foldout);

            if (cache.expanded)
            {
                EditorGUI.indentLevel = 1;

                for (int i = 0; i < cache.props.Count; i++)
                {
                    EditorGUILayout.BeginVertical(StyleFramework.BoxChild);
                    EditorGUILayout.PropertyField(cache.props[i], new GUIContent(cache.props[i].name.FirstLetterToUpperCase()), true);
                    EditorGUILayout.EndVertical();
                }
            }
        }

        private void Setup()
        {
            if (_initialized) return;

            SpoilerAttribute prevFold = default;

            var length = EditorTypes.Get(_target, out var objectFields);

            for (var i = 0; i < length; i++)
            {
                #region FOLDERS

                var fold = Attribute.GetCustomAttribute(objectFields[i], typeof(SpoilerAttribute)) as SpoilerAttribute;
                CacheFoldProp c;
                if (fold == null)
                {
                    if (prevFold != null && prevFold.foldEverything)
                    {
                        if (!_cacheFolds.TryGetValue(prevFold.name, out c))
                        {
                            _cacheFolds.Add(prevFold.name,
                                new CacheFoldProp {atr = prevFold, types = new HashSet<string> {objectFields[i].Name}});
                        }
                        else
                        {
                            c.types.Add(objectFields[i].Name);
                        }
                    }

                    continue;
                }

                prevFold = fold;

                if (!_cacheFolds.TryGetValue(fold.name, out c))
                {
                    var expanded = EditorPrefs.GetBool(string.Format($"{fold.name}{objectFields[i].Name}{_target.name}"), false);
                    _cacheFolds.Add(fold.name,
                        new CacheFoldProp {atr = fold, types = new HashSet<string> {objectFields[i].Name}, expanded = expanded});
                }
                else
                    c.types.Add(objectFields[i].Name);

                #endregion
            }

            var property = _serializedObject.GetIterator();
            var next = property.NextVisible(true);
            if (next)
            {
                do
                {
                    HandleFoldProp(property);
                } while (property.NextVisible(false));
            }

            _initialized = true;
        }

        private void HandleFoldProp(SerializedProperty prop)
        {
            bool shouldBeFolded = false;

            foreach (var pair in _cacheFolds)
            {
                if (pair.Value.types.Contains(prop.name))
                {
                    var pr = prop.Copy();
                    shouldBeFolded = true;
                    pair.Value.props.Add(pr);

                    break;
                }
            }

            if (shouldBeFolded == false)
            {
                var pr = prop.Copy();
                _props.Add(pr);
            }
        }


        private class CacheFoldProp
        {
            public HashSet<string> types = new HashSet<string>();
            public List<SerializedProperty> props = new List<SerializedProperty>();
            public SpoilerAttribute atr;
            public bool expanded;

            public void Dispose()
            {
                props.Clear();
                types.Clear();
                atr = null;
            }
        }
    }
}