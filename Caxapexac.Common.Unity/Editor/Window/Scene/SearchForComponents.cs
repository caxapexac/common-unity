using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Caxapexac.Common.Sharp.Editor.Window.Scene
{
    public class SearchForComponents : EditorWindow
    {
        [MenuItem("Tools/Common/Search For Components")]
        private static void Init()
        {
            SearchForComponents window = GetWindow<SearchForComponents>();
            window.Show();
            window.position = new Rect(20, 80, 550, 500);
        }

        private string[] _modes = {"Search for component usage", "Search for missing components"};
        private string[] _checkType = {"Check single component", "Check all components"};

        private List<string> _listResult;
        private List<ComponentNames> _prefabComponents;
        private List<ComponentNames> _notUsedComponents;
        private List<ComponentNames> _addedComponents;
        private List<ComponentNames> _existingComponents;
        private List<ComponentNames> _sceneComponents;
        private int _editorMode;
        private int _selectedCheckType;
        private MonoScript _targetComponent;
        private string _componentName = "";

        private bool _showPrefabs;
        private bool _showAdded;
        private bool _showScene;
        private bool _showUnused = true;
        private Vector2 _scroll;
        private Vector2 _scroll1;
        private Vector2 _scroll2;
        private Vector2 _scroll3;
        private Vector2 _scroll4;


        private class ComponentNames
        {
            public readonly string componentName;
            public readonly List<string> usageSource;
            public string namespaceName;
            public string assetPath;

            public ComponentNames(string comp, string space, string path)
            {
                componentName = comp;
                namespaceName = space;
                assetPath = path;
                usageSource = new List<string>();
            }

            public override bool Equals(object obj)
            {
                return ((ComponentNames)obj)?.componentName == componentName && ((ComponentNames)obj)?.namespaceName == namespaceName;
            }

            public override int GetHashCode()
            {
                return componentName.GetHashCode() + namespaceName.GetHashCode();
            }
        }


        private void OnGUI()
        {
            GUILayout.Label(position + "");
            GUILayout.Space(3);
            int oldValue = GUI.skin.window.padding.bottom;
            GUI.skin.window.padding.bottom = -20;
            Rect windowRect = GUILayoutUtility.GetRect(1, 17);
            windowRect.x += 4;
            windowRect.width -= 7;
            _editorMode = GUI.SelectionGrid(windowRect, _editorMode, _modes, 2, "Window");
            GUI.skin.window.padding.bottom = oldValue;

            switch (_editorMode)
            {
                case 0:
                    _selectedCheckType = GUILayout.SelectionGrid(_selectedCheckType, _checkType, 2, "Toggle");
                    GUI.enabled = _selectedCheckType == 0;
                    _targetComponent = (MonoScript)EditorGUILayout.ObjectField(_targetComponent, typeof(MonoScript), false);
                    GUI.enabled = true;

                    if (GUILayout.Button("Check component usage"))
                    {
                        AssetDatabase.SaveAssets();
                        switch (_selectedCheckType)
                        {
                            case 0:
                                _componentName = _targetComponent.name;
                                string targetPath = AssetDatabase.GetAssetPath(_targetComponent);
                                string[] allPrefabs = GetAllPrefabs();
                                _listResult = new List<string>();
                                foreach (string prefab in allPrefabs)
                                {
                                    string[] single = new string[] {prefab};
                                    string[] dependencies = AssetDatabase.GetDependencies(single);
                                    foreach (string dependedAsset in dependencies)
                                    {
                                        if (dependedAsset == targetPath)
                                        {
                                            _listResult.Add(prefab);
                                        }
                                    }
                                }
                                break;
                            case 1:
                                List<string> scenesToLoad = new List<string>();
                                _existingComponents = new List<ComponentNames>();
                                _prefabComponents = new List<ComponentNames>();
                                _notUsedComponents = new List<ComponentNames>();
                                _addedComponents = new List<ComponentNames>();
                                _sceneComponents = new List<ComponentNames>();

                                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                {
                                    string projectPath = Application.dataPath;
                                    projectPath = projectPath.Substring(0, projectPath.IndexOf("Assets", StringComparison.Ordinal));

                                    string[] allAssets = AssetDatabase.GetAllAssetPaths();

                                    foreach (string asset in allAssets)
                                    {
                                        int indexCs = asset.IndexOf(".cs", StringComparison.Ordinal);
                                        int indexJs = asset.IndexOf(".js", StringComparison.Ordinal);
                                        if (indexCs != -1 || indexJs != -1)
                                        {
                                            ComponentNames newComponent = new ComponentNames(NameFromPath(asset), "", asset);
                                            try
                                            {
                                                System.IO.FileStream fs = new System.IO.FileStream(projectPath + asset, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                                                    System.IO.FileShare.ReadWrite);
                                                System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                                                while (!sr.EndOfStream)
                                                {
                                                    var line = sr.ReadLine();
                                                    int index1 = line.IndexOf("namespace", StringComparison.Ordinal);
                                                    int index2 = line.IndexOf("{", StringComparison.Ordinal);
                                                    if (index1 != -1 && index2 != -1)
                                                    {
                                                        line = line.Substring(index1 + 9);
                                                        index2 = line.IndexOf("{", StringComparison.Ordinal);
                                                        line = line.Substring(0, index2);
                                                        line = line.Replace(" ", "");
                                                        newComponent.namespaceName = line;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                // ignored
                                            }

                                            _existingComponents.Add(newComponent);

                                            try
                                            {
                                                System.IO.FileStream fs = new System.IO.FileStream(projectPath + asset, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                                                    System.IO.FileShare.ReadWrite);
                                                System.IO.StreamReader sr = new System.IO.StreamReader(fs);

                                                int lineNum = 0;
                                                while (!sr.EndOfStream)
                                                {
                                                    lineNum++;
                                                    var line = sr.ReadLine();
                                                    int index = line.IndexOf("AddComponent", StringComparison.Ordinal);
                                                    if (index == -1) continue;
                                                    line = line.Substring(index + 12);
                                                    switch (line[0])
                                                    {
                                                        case '(':
                                                            line = line.Substring(1, line.IndexOf(')') - 1);
                                                            break;
                                                        case '<':
                                                            line = line.Substring(1, line.IndexOf('>') - 1);
                                                            break;
                                                        default:
                                                            continue;
                                                    }
                                                    line = line.Replace(" ", "");
                                                    line = line.Replace("\"", "");
                                                    index = line.LastIndexOf('.');
                                                    ComponentNames newComp;
                                                    if (index == -1)
                                                    {
                                                        newComp = new ComponentNames(line, "", "");
                                                    }
                                                    else
                                                    {
                                                        newComp = new ComponentNames(line.Substring(index + 1, line.Length - (index + 1)), line.Substring(0, index), "");
                                                    }
                                                    string pName = asset + ", Line " + lineNum;
                                                    newComp.usageSource.Add(pName);
                                                    index = _addedComponents.IndexOf(newComp);
                                                    if (index == -1)
                                                    {
                                                        _addedComponents.Add(newComp);
                                                    }
                                                    else
                                                    {
                                                        if (!_addedComponents[index].usageSource.Contains(pName)) _addedComponents[index].usageSource.Add(pName);
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                // ignored
                                            }
                                        }
                                        int indexPrefab = asset.IndexOf(".prefab", StringComparison.Ordinal);

                                        if (indexPrefab != -1)
                                        {
                                            string[] single = new string[] {asset};
                                            string[] dependencies = AssetDatabase.GetDependencies(single);
                                            foreach (string dependedAsset in dependencies)
                                            {
                                                if (dependedAsset.IndexOf(".cs", StringComparison.Ordinal) == -1 && dependedAsset.IndexOf(".js", StringComparison.Ordinal) == -1) continue;
                                                ComponentNames newComponent = new ComponentNames(NameFromPath(dependedAsset), GetNamespaceFromPath(dependedAsset), dependedAsset);
                                                int index = _prefabComponents.IndexOf(newComponent);
                                                if (index == -1)
                                                {
                                                    newComponent.usageSource.Add(asset);
                                                    _prefabComponents.Add(newComponent);
                                                }
                                                else
                                                {
                                                    if (!_prefabComponents[index].usageSource.Contains(asset)) _prefabComponents[index].usageSource.Add(asset);
                                                }
                                            }
                                        }
                                        int indexUnity = asset.IndexOf(".unity", StringComparison.Ordinal);
                                        if (indexUnity != -1)
                                        {
                                            scenesToLoad.Add(asset);
                                        }
                                    }

                                    for (int i = _addedComponents.Count - 1; i > -1; i--)
                                    {
                                        _addedComponents[i].assetPath = GetPathFromNames(_addedComponents[i].namespaceName, _addedComponents[i].componentName);
                                        if (_addedComponents[i].assetPath == "") _addedComponents.RemoveAt(i);
                                    }

                                    foreach (string scene in scenesToLoad)
                                    {
                                        EditorSceneManager.OpenScene(scene);
                                        IEnumerable<GameObject> sceneGOs = GetAllObjectsInScene();
                                        foreach (GameObject g in sceneGOs)
                                        {
                                            Component[] comps = g.GetComponentsInChildren<Component>(true);
                                            foreach (Component c in comps)
                                            {
                                                if (c == null || c.GetType() == null || c.GetType().BaseType == null || c.GetType().BaseType != typeof(MonoBehaviour)) continue;
                                                SerializedObject so = new SerializedObject(c);
                                                SerializedProperty p = so.FindProperty("m_Script");
                                                string path = AssetDatabase.GetAssetPath(p.objectReferenceValue);
                                                ComponentNames newComp = new ComponentNames(NameFromPath(path), GetNamespaceFromPath(path), path);
                                                newComp.usageSource.Add(scene);
                                                int index = _sceneComponents.IndexOf(newComp);
                                                if (index == -1)
                                                {
                                                    _sceneComponents.Add(newComp);
                                                }
                                                else
                                                {
                                                    if (!_sceneComponents[index].usageSource.Contains(scene)) _sceneComponents[index].usageSource.Add(scene);
                                                }
                                            }
                                        }
                                    }

                                    foreach (ComponentNames c in _existingComponents)
                                    {
                                        if (_addedComponents.Contains(c)) continue;
                                        if (_prefabComponents.Contains(c)) continue;
                                        if (_sceneComponents.Contains(c)) continue;
                                        _notUsedComponents.Add(c);
                                    }

                                    _addedComponents.Sort(SortAlphabetically);
                                    _prefabComponents.Sort(SortAlphabetically);
                                    _sceneComponents.Sort(SortAlphabetically);
                                    _notUsedComponents.Sort(SortAlphabetically);
                                }
                                break;
                        }
                    }
                    break;
                case 1:
                    if (GUILayout.Button("Search!"))
                    {
                        string[] allPrefabs = GetAllPrefabs();
                        _listResult = new List<string>();
                        foreach (string prefab in allPrefabs)
                        {
                            Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
                            try
                            {
                                var go = (GameObject)o;
                                Component[] components = go.GetComponentsInChildren<Component>(true);
                                foreach (Component c in components)
                                {
                                    if (c == null)
                                    {
                                        _listResult.Add(prefab);
                                    }
                                }
                            }
                            catch
                            {
                                Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");
                            }
                        }
                    }
                    break;
            }
            if (_editorMode == 1 || _selectedCheckType == 0)
            {
                if (_listResult == null) return;
                if (_listResult.Count == 0)
                {
                    GUILayout.Label(_editorMode == 0
                        ? (_componentName == "" ? "Choose a component" : "No prefabs use component " + _componentName)
                        : ("No prefabs have missing components!\nClick Search to check again"));
                }
                else
                {
                    GUILayout.Label(_editorMode == 0 ? ("The following prefabs use component " + _componentName + ":") : ("The following prefabs have missing components:"));
                    _scroll = GUILayout.BeginScrollView(_scroll);
                    foreach (string s in _listResult)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(s, GUILayout.Width(position.width / 2));
                        if (GUILayout.Button("Select", GUILayout.Width(position.width / 2 - 10)))
                        {
                            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(s);
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();
                }
            }
            else
            {
                _showPrefabs = GUILayout.Toggle(_showPrefabs, "Show prefab components");
                if (_showPrefabs)
                {
                    GUILayout.Label("The following components are attached to prefabs:");
                    DisplayResults(ref _scroll1, ref _prefabComponents);
                }
                _showAdded = GUILayout.Toggle(_showAdded, "Show AddComponent arguments");
                if (_showAdded)
                {
                    GUILayout.Label("The following components are AddComponent arguments:");
                    DisplayResults(ref _scroll2, ref _addedComponents);
                }
                _showScene = GUILayout.Toggle(_showScene, "Show Scene-used components");
                if (_showScene)
                {
                    GUILayout.Label("The following components are used by scene objects:");
                    DisplayResults(ref _scroll3, ref _sceneComponents);
                }
                _showUnused = GUILayout.Toggle(_showUnused, "Show Unused Components");
                if (_showUnused)
                {
                    GUILayout.Label("The following components are not used by prefabs, by AddComponent, OR in any scene:");
                    DisplayResults(ref _scroll4, ref _notUsedComponents);
                }
            }
        }

        private int SortAlphabetically(ComponentNames a, ComponentNames b)
        {
            return string.Compare(a.assetPath, b.assetPath, StringComparison.Ordinal);
        }

        private IEnumerable<GameObject> GetAllObjectsInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            GameObject[] allGOs = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject go in allGOs)
            {
                //if ( go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave )
                //    continue;

                string assetPath = AssetDatabase.GetAssetPath(go.transform.root.gameObject);
                if (!string.IsNullOrEmpty(assetPath)) continue;

                objectsInScene.Add(go);
            }

            return objectsInScene.ToArray();
        }

        private void DisplayResults(ref Vector2 scroller, ref List<ComponentNames> list)
        {
            if (list == null) return;
            scroller = GUILayout.BeginScrollView(scroller);
            foreach (ComponentNames c in list)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(c.assetPath, GUILayout.Width(position.width / 5 * 4));
                if (GUILayout.Button("Select", GUILayout.Width(position.width / 5 - 30)))
                {
                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(c.assetPath);
                }
                GUILayout.EndHorizontal();
                if (c.usageSource.Count == 1)
                {
                    GUILayout.Label("   In 1 Place: " + c.usageSource[0]);
                }
                if (c.usageSource.Count > 1)
                {
                    GUILayout.Label("   In " + c.usageSource.Count + " Places: " + c.usageSource[0] + ", " + c.usageSource[1] + (c.usageSource.Count > 2 ? ", ..." : ""));
                }
            }
            GUILayout.EndScrollView();
        }

        private string NameFromPath(string s)
        {
            s = s.Substring(s.LastIndexOf('/') + 1);
            return s.Substring(0, s.Length - 3);
        }

        private string GetNamespaceFromPath(string path)
        {
            foreach (var c in _existingComponents.Where(c => c.assetPath == path))
            {
                return c.namespaceName;
            }
            return "";
        }

        private string GetPathFromNames(string space, string componentName)
        {
            ComponentNames test = new ComponentNames(componentName, space, "");
            int index = _existingComponents.IndexOf(test);
            return index != -1 ? _existingComponents[index].assetPath : "";
        }

        public static string[] GetAllPrefabs()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            return temp.Where(s => s.Contains(".prefab")).ToArray();
        }
    }
}