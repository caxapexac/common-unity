using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Window.Scene
{
    public class AutoSave : EditorWindow
    {
        private bool _autoSaveScene = false;
        private bool _showMessage = true;
        private bool _isStarted = false;
        private int _intervalScene;
        private DateTime _lastSaveTimeScene = DateTime.Now;

        [MenuItem("Tools/Common Scene/Auto Save")]
        public static void ShowWindow()
        {
            AutoSave sw = GetWindow<AutoSave>("Auto Save");

            //sw.minSize = new Vector2(1, 0.1f);
            sw.Show();
            sw.Save();
        }

        private void OnGUI()
        {
            GUILayout.Label("Info:", EditorStyles.boldLabel);
            GUILayout.Label("Options:", EditorStyles.boldLabel);
            _autoSaveScene = EditorGUILayout.BeginToggleGroup("Auto save", _autoSaveScene);
            _intervalScene = EditorGUILayout.IntSlider("Interval (minutes)", _intervalScene, 1, 10);
            if (_isStarted)
            {
                EditorGUILayout.LabelField("Last save:", "" + _lastSaveTimeScene);
            }
            EditorGUILayout.EndToggleGroup();
            _showMessage = EditorGUILayout.BeginToggleGroup("Show Message", _showMessage);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button("Save now") && !EditorApplication.isPlaying)
            {
                Save();
            }
        }

        private void Update()
        {
            if (_autoSaveScene && !EditorApplication.isPlaying)
            {
                if (DateTime.Now.Minute >= (_lastSaveTimeScene.Minute + _intervalScene) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
                {
                    Save();
                }
            }
            else
            {
                _isStarted = false;
            }
        }

        private void Save()
        {
            EditorSceneManager.SaveOpenScenes();
            _lastSaveTimeScene = DateTime.Now;
            _isStarted = true;
            if (_showMessage)
            {
                Debug.Log("Auto Save on " + _lastSaveTimeScene);
            }
            AutoSave repaintSaveWindow = GetWindow<AutoSave>();
            repaintSaveWindow.Repaint();
        }
    }
}