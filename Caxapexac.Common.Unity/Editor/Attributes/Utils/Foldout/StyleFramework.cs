using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.Utils
{
    internal static class StyleFramework
    {
        public static readonly GUIStyle Box;
        public static readonly GUIStyle BoxChild;
        public static readonly GUIStyle Foldout;

        static StyleFramework()
        {
            bool pro = EditorGUIUtility.isProSkin;

            var uiTex_in = Resources.Load<Texture2D>("IN foldout focus-6510");
            var uiTex_in_on = Resources.Load<Texture2D>("IN foldout focus on-5718");

            var c_on = pro ? Color.white : new Color(51 / 255f, 102 / 255f, 204 / 255f, 1);

            Foldout = new GUIStyle(EditorStyles.foldout);

            Foldout.overflow = new RectOffset(-10, 0, 3, 0);
            Foldout.padding = new RectOffset(25, 0, -3, 0);

            Foldout.active.textColor = c_on;
            Foldout.active.background = uiTex_in;
            Foldout.onActive.textColor = c_on;
            Foldout.onActive.background = uiTex_in_on;

            Foldout.focused.textColor = c_on;
            Foldout.focused.background = uiTex_in;
            Foldout.onFocused.textColor = c_on;
            Foldout.onFocused.background = uiTex_in_on;

            Foldout.hover.textColor = c_on;
            Foldout.hover.background = uiTex_in;

            Foldout.onHover.textColor = c_on;
            Foldout.onHover.background = uiTex_in_on;

            Box = new GUIStyle(GUI.skin.box);
            Box.padding = new RectOffset(10, 0, 10, 0);

            BoxChild = new GUIStyle(GUI.skin.box);
            BoxChild.active.textColor = c_on;
            BoxChild.active.background = uiTex_in;
            BoxChild.onActive.textColor = c_on;
            BoxChild.onActive.background = uiTex_in_on;

            BoxChild.focused.textColor = c_on;
            BoxChild.focused.background = uiTex_in;
            BoxChild.onFocused.textColor = c_on;
            BoxChild.onFocused.background = uiTex_in_on;

            EditorStyles.foldout.active.textColor = c_on;
            EditorStyles.foldout.active.background = uiTex_in;
            EditorStyles.foldout.onActive.textColor = c_on;
            EditorStyles.foldout.onActive.background = uiTex_in_on;

            EditorStyles.foldout.focused.textColor = c_on;
            EditorStyles.foldout.focused.background = uiTex_in;
            EditorStyles.foldout.onFocused.textColor = c_on;
            EditorStyles.foldout.onFocused.background = uiTex_in_on;

            EditorStyles.foldout.hover.textColor = c_on;
            EditorStyles.foldout.hover.background = uiTex_in;

            EditorStyles.foldout.onHover.textColor = c_on;
            EditorStyles.foldout.onHover.background = uiTex_in_on;
        }

        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static IList<Type> GetTypeTree(this Type t)
        {
            var types = new List<Type>();
            while (t.BaseType != null)
            {
                types.Add(t);
                t = t.BaseType;
            }

            return types;
        }
    }
}