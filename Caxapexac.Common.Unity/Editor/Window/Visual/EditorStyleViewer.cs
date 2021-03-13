using UnityEditor;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Window.Visual
{
    public class EditorStyleViewer : EditorWindow
    {
        private Vector2 _scrollPosition = Vector2.zero;
        private string _search = string.Empty;

        [MenuItem("Tools/Common Visual/Editor Style Viewer")]
        public static void Init()
        {
            GetWindow<EditorStyleViewer>("Editor Style Viewer");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("Label", "label");
            GUILayout.FlexibleSpace();
            GUILayout.Label("Style:");
            _search = EditorGUILayout.TextField(_search);
            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            foreach (GUIStyle style in GUI.skin)
            {
                if (style.name.ToLower().Contains(_search.ToLower()))
                {
                    GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                    GUILayout.Space(7);
                    if (GUILayout.Button(style.name, style))
                    {
                        EditorGUIUtility.systemCopyBuffer = "\"" + style.name + "\"";
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.SelectableLabel("\"" + style.name + "\"");
                    GUILayout.EndHorizontal();
                    GUILayout.Space(11);
                }
            }

            GUILayout.EndScrollView();
        }
    }
}