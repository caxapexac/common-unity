// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Caxapexac.Common.Sharp.Data;
using Caxapexac.Common.Sharp.Editor.Other;
using Caxapexac.Common.Sharp.Runtime.Data;
using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Window.Visual
{
    internal sealed class FolderIconEditor : EditorWindow
    {
        private class FolderIconDesc
        {
            private Color32? _validColor;

            [JsonIgnore]
            public Color32 ValidColor
            {
                get
                {
                    if (!_validColor.HasValue)
                    {
                        Color col;
                        ColorUtility.TryParseHtmlString(HexColor, out col);
                        _validColor = col;
                    }
                    return _validColor.Value;
                }
                set
                {
                    _validColor = value;
                    HexColor = "#" + ColorUtility.ToHtmlStringRGB(value);
                }
            }

            [JsonName("c")]
            public string HexColor = "#ffffff";

            [JsonName("i")]
            public string OverlayIcon;
        }


        private const string Title = "Folder Icon Editor";

        private const int IconSize = 64;

        private const string FolderIconName = "Folder Icon";

        private const string StorageKey = "lg.folder-icons";

        private static Texture2D _folderIconBack;

        private static Dictionary<string, FolderIconDesc> _allDescriptions;

        private string _folderPath;

        private string _folderGuid;

        private FolderIconDesc _folderDesc;

        [MenuItem("Tools/Common Visual/Folder Icons/Editor")]
        private static void OpenUi()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path) && AssetDatabase.IsValidFolder(path))
            {
                var win = GetWindow<FolderIconEditor>(true);
                win.Init(path);
            }
            else
            {
                EditorUtility.DisplayDialog(Title, "Select valid folder first", "Close");
            }
        }

        [MenuItem("Tools/Common Visual/Folder Icons/Reset")]
        private static void ClearAllUi()
        {
            _allDescriptions.Clear();
            SaveInfo();
            EditorUtility.DisplayDialog(Title, "Successfully reset", "Close");
        }

        [InitializeOnLoadMethod]
        private static void InitFolderIconEditor()
        {
            EditorApplication.projectWindowItemOnGUI += OnDrawProjectWindowItem;
            if ((object)_folderIconBack == null)
            {
                _folderIconBack = EditorGUIUtility.Load(FolderIconName) as Texture2D;
            }
            LoadInfo();
            SaveInfo();
        }

        private static void LoadInfo()
        {
            try
            {
                _allDescriptions = Service<JsonSerialization>.Get()
                    .Deserialize<Dictionary<string, FolderIconDesc>>(
                        ProjectPrefs.GetString(StorageKey, "{}"));
                if (_allDescriptions == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                _allDescriptions = new Dictionary<string, FolderIconDesc>();
            }
        }

        private static void SaveInfo()
        {
            if (_allDescriptions.Count > 0)
            {
                try
                {
                    ProjectPrefs.SetString(StorageKey, Service<JsonSerialization>.Get().Serialize(_allDescriptions));
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("FolderIconEditor.SaveInfo: " + ex.Message);
                }
            }
            else
            {
                ProjectPrefs.DeleteKey(StorageKey);
            }
        }

        private static void OnDrawProjectWindowItem(string guid, Rect rect)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (AssetDatabase.IsValidFolder(path))
            {
                var icon = GetCustomIcon(guid);
                if (icon != null)
                {
                    if (rect.width > rect.height)
                    {
                        rect.width = rect.height;
                    }
                    else
                    {
                        rect.height = rect.width;
                    }
                    if (rect.width > IconSize)
                    {
                        var offset = (rect.width - IconSize) * 0.5f;
                        rect.Set(rect.x + offset, rect.y + offset, IconSize, IconSize);
                    }
                    var savedColor = GUI.color;
                    GUI.color = icon.ValidColor;
                    GUI.DrawTexture(rect, _folderIconBack);
                    if (icon.OverlayIcon != null)
                    {
                        GUI.color = savedColor;
                        var tex = EditorGUIUtility.Load(icon.OverlayIcon) as Texture2D;
                        if ((object)tex != null)
                        {
                            rect.width *= 0.5f;
                            rect.height *= 0.5f;
                            rect.x += rect.width;
                            rect.y += rect.height;
                            GUI.DrawTexture(rect, tex);
                        }
                    }
                    else
                    {
                        GUI.color = savedColor;
                    }
                }
            }
        }

        private static FolderIconDesc GetCustomIcon(string guid)
        {
            return _allDescriptions.ContainsKey(guid) ? _allDescriptions[guid] : null;
        }

        private void Init(string folderPath)
        {
            _folderPath = folderPath;
            _folderGuid = AssetDatabase.AssetPathToGUID(_folderPath);
            _folderDesc = GetCustomIcon(_folderGuid) ?? new FolderIconDesc();
        }

        private void OnEnable()
        {
            titleContent.text = Title;
        }

        private void OnGUI()
        {
            if (_folderDesc == null)
            {
                Close();
                return;
            }
            var needToSave = false;
            var color = _folderDesc.ValidColor;
            var newR = (byte)EditorGUILayout.IntSlider("Color R", color.r, 0, 255);
            var newG = (byte)EditorGUILayout.IntSlider("Color G", color.g, 0, 255);
            var newB = (byte)EditorGUILayout.IntSlider("Color B", color.b, 0, 255);
            needToSave |= newR != color.r || newG != color.g || newB != color.b;

            var iconName = _folderDesc.OverlayIcon ?? "";
            var newIconName = EditorGUILayout.TextField("Folder Overlay icon", iconName);
            needToSave |= string.CompareOrdinal(newIconName, iconName) != 0;

            if (needToSave)
            {
                _folderDesc.ValidColor = new Color32(newR, newG, newB, 255);
                _folderDesc.OverlayIcon = newIconName.Length > 0 ? newIconName : null;
                if (_folderDesc != GetCustomIcon(_folderGuid))
                {
                    _allDescriptions[_folderGuid] = _folderDesc;
                }
                EditorApplication.RepaintProjectWindow();
            }
            if (GUILayout.Button("Reset for this folder"))
            {
                if (GetCustomIcon(_folderGuid) != null)
                {
                    _allDescriptions.Remove(_folderGuid);
                }
                OnLostFocus();
            }
        }

        private void OnLostFocus()
        {
            Close();
        }

        private void OnDisable()
        {
            SaveInfo();
        }
    }
}