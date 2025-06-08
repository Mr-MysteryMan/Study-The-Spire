using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Combat.StateMachine.States;

namespace EnemyStateMachine.Editor
{
    public class EnemyStateSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        EnemyStateGraphView graphView;
        EditorWindow host;

        public void Init(EnemyStateGraphView gv, EditorWindow window)
        {
            graphView = gv;
            host = window;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext ctx)
        {
            // 统一用脚本对象图标
            var icon = EditorGUIUtility.IconContent("ScriptableObject Icon").image;

            return new List<SearchTreeEntry>
            {
                /* 根节点必须是 SearchTreeGroupEntry，level = 0 */
                new SearchTreeGroupEntry(new GUIContent("Create State Nodes"), 0),

                /* 二级分组（可选） */
                new SearchTreeGroupEntry(new GUIContent("States"), 1),

                /* 具体可创建的节点 —— SearchTreeEntry，level >= 2 */
                new SearchTreeEntry(new GUIContent("Basic State",     icon)) { level = 2, userData = typeof(BasicEffectStateSO)     },
                new SearchTreeEntry(new GUIContent("ProbEffect State", icon)) { level = 2, userData = typeof(BasicProbEffectStateSO) },
                new SearchTreeEntry(new GUIContent("Condition State", icon)) { level = 2, userData = typeof(BasicConditionEffectStateSO) },
            };
        }



        public bool OnSelectEntry(SearchTreeEntry entry, Vector2 mousePos)
        {
            var mouseWorldPos = graphView.contentViewContainer.WorldToLocal(mousePos);
            var type = (Type)entry.userData;

            var graph = graphView.CurrentGraph;                          // 给 GraphView 暴露个只读属性
            string folder = graph.editorFolderPath;                      // 从 GraphSO 取

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(
                                  $"{folder}/{type.Name}.asset");
            var so = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(so, assetPath);
            AssetDatabase.SaveAssets();
            Undo.RegisterCreatedObjectUndo(so, "Create State");

            // 创建节点
            var node = graphView.CreateNodeView((EffectStateBaseSO)so, pos: mouseWorldPos);
            graphView.AddElement(node);

            return true;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var mousePos = context.screenMousePosition;
            return OnSelectEntry(SearchTreeEntry, mousePos);
        }
    }
}
