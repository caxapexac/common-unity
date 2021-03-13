// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Other
{
    public enum GameObjectLabelIconType
    {
        Gray = 0,
        Blue,
        Teal,
        Green,
        Yellow,
        Orange,
        Red,
        Purple
    }


    public enum GameObjectImageIconType
    {
        CircleGray = 0,
        CircleBlue,
        CircleTeal,
        CircleGreen,
        CircleYellow,
        CircleOrange,
        CircleRed,
        CirclePurple,
        DiamondGray,
        DiamondBlue,
        DiamondTeal,
        DiamondGreen,
        DiamondYellow,
        DiamondOrange,
        DiamondRed,
        DiamondPurple
    }


    public static class GameObjectIcon
    {
#if UNITY_EDITOR
        private static readonly List<Texture2D> _icons;

        private const string SetIconMethodName = "SetIconForObject";

        private const string LabelIconMask = "sv_label_";

        private const string ImageIconMask = "sv_icon_dot";

        private const string ImageIconSmallSuffix = "_sml";

        private const string ImageIconLargeSuffix = "_pix16_gizmo";

        private static readonly MethodInfo _setIcon;

        private static readonly object[] _setIconArgs;

        static GameObjectIcon()
        {
            _setIconArgs = new object[2];
            _setIcon = typeof(UnityEditor.EditorGUIUtility)
                .GetMethod(SetIconMethodName, BindingFlags.Static | BindingFlags.NonPublic);
            _icons = new List<Texture2D>();
            FillIcons(_icons, LabelIconMask, string.Empty, 8);
            FillIcons(_icons, ImageIconMask, ImageIconSmallSuffix, 16);
            FillIcons(_icons, ImageIconMask, ImageIconLargeSuffix, 16);
        }

        private static void FillIcons(IList<Texture2D> dict, string baseName, string suffix, int count)
        {
            GUIContent content;
            for (var i = 0; i < count; i++)
            {
                content = UnityEditor.EditorGUIUtility.IconContent(baseName + i + suffix);
                dict.Add(content != null ? content.image as Texture2D : null);
            }
        }

        private static Texture2D GetIconTexture(int iconOffset, int iconId)
        {
            iconOffset = Mathf.Clamp(iconOffset + iconId, 0, _icons.Count - 1);
            return _icons[iconOffset];
        }

        private static void SetIcon(GameObject go, Texture2D icon)
        {
            _setIconArgs[0] = go;
            _setIconArgs[1] = icon;
            _setIcon.Invoke(null, _setIconArgs);
        }

#endif

        [Conditional("UNITY_EDITOR")]
        public static void SetLabelIcon(this GameObject go, GameObjectLabelIconType iconType)
        {
#if UNITY_EDITOR
            SetIcon(go, GetIconTexture(0, (int)iconType));
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetSmallImageIcon(this GameObject go, GameObjectImageIconType iconType)
        {
#if UNITY_EDITOR
            SetIcon(go, GetIconTexture(0 + 8, (int)iconType));
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetLargeImageIcon(this GameObject go, GameObjectImageIconType iconType)
        {
#if UNITY_EDITOR
            SetIcon(go, GetIconTexture(0 + 8 + 16, (int)iconType));
#endif
        }
    }
}