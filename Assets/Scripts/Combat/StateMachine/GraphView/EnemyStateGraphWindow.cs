using Combat.StateMachine.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EnemyStateMachine.Editor
{
    public class EnemyStateGraphWindow : EditorWindow
    {
        [MenuItem("Tools/Enemy/State Machine Editor")]
        private static void Open() => GetWindow<EnemyStateGraphWindow>();

        EnemyStateGraphView graphView;
        private EnemyStateGraphSO currentAsset;
        public EnemyStateGraphSO CurrentAsset => currentAsset;

        private bool Avaliable => workspace != null;

        void OnEnable()
        {
            titleContent = new UnityEngine.GUIContent("Enemy State Machine");
            graphView = new EnemyStateGraphView(this)
            {
                name = "Enemy State Graph",
                style = { flexGrow = 1 }
            };
            rootVisualElement.Add(graphView);

            // 顶部工具栏
            var toolbar = new Toolbar();
            toolbar.Add(new ToolbarButton(NewGraph) { text = "新建" });
            toolbar.Add(new ToolbarButton(OpenGraph) { text = "打开" });
            toolbar.Add(new ToolbarButton(Save) { text = "保存" });
            toolbar.Add(new ToolbarButton(Validate) { text = "校验" });
            rootVisualElement.Add(toolbar);
        }

        public string workspace;   // 当前图所在的文件夹

        void NewGraph()
        {
            string folder = EditorUtility.SaveFolderPanel("Select Workspace (will be created)", GraphViewSetting.BaseFolderPath, "");
            if (string.IsNullOrEmpty(folder)) return;
            if (!folder.StartsWith(Application.dataPath))
            { EditorUtility.DisplayDialog("Error", "Folder必须在Assets内", "OK"); return; }

            // 把绝对路径转Asset相对
            folder = "Assets" + folder.Substring(Application.dataPath.Length);

            // 创建GraphSO
            var graph = ScriptableObject.CreateInstance<EnemyStateGraphSO>();
            graph.editorFolderPath = folder;
            string graphPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/EnemyStateGraph.asset");
            AssetDatabase.CreateAsset(graph, graphPath);
            AssetDatabase.SaveAssets();

            workspace = folder;

            currentAsset = graph;
            graphView.SetCurrentGraph(graph);
        }

        void OpenGraph()
        {
            string path = EditorUtility.OpenFilePanel("Open EnemyStateGraph", GraphViewSetting.BaseFolderPath, "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace(Application.dataPath, "Assets");
            var graph = AssetDatabase.LoadAssetAtPath<EnemyStateGraphSO>(path);
            if (!graph) return;
            workspace = graph.editorFolderPath = System.IO.Path.GetDirectoryName(path);
            Load(graph);
        }

        void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

        public void Load(EnemyStateGraphSO asset)
        {
            currentAsset = asset;
            graphView.SetCurrentGraph(asset);
            graphView.Populate(asset);
        }

        public void Save()
        {
            if (!CurrentAsset)
            {
                return;
            }
            graphView.Serialize(CurrentAsset);
            EditorUtility.SetDirty(CurrentAsset);
            AssetDatabase.SaveAssets();
        }

        private void Validate()
        {
            if (!Avaliable)
            {
                EditorUtility.DisplayDialog("Error", "请先打开或创建一个状态机图。", "OK");
                return;
            }
            if (!graphView.Validate())
            {
                EditorUtility.DisplayDialog("Validation Failed", "存在未连接的输出节点或错误的连接，请检查图。", "OK");
            }
            else if (currentAsset.entry == null)
            {
                EditorUtility.DisplayDialog("Validation Failed", "请设置一个入口状态。", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Success", "图验证通过，没有错误。", "OK");
            }
        }
    }
}
