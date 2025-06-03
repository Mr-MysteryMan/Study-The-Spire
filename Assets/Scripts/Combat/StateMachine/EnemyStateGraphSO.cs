using System.Collections.Generic;
using EnemyStateMachine.Editor;
using UnityEngine;

namespace Combat.StateMachine.States   // 与 EffectStateBaseSO 等保持同一命名空间
{
    /// <summary>
    ///   一个敌人状态机的“资源包”。
    ///   - <c>entry</c> 是运行时进入的起始状态  
    ///   - <c>states</c> 保存图中出现的所有状态 SO，方便遍历 / 序列化
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnemyStateGraph",
        menuName = "Combat/StateMachine/Enemy State Graph")]
    public class EnemyStateGraphSO : ScriptableObject
    {
        [Tooltip("状态机入口（起始状态）")]
        public EffectStateBaseSO entry;

        [Tooltip("该图中涉及到的所有状态脚本对象")]
        public List<EffectStateBaseSO> states = new();

        [System.Serializable]
        public class NodeMeta
        {
            public EffectStateBaseSO node;
            public Vector2 position;
        }

#if UNITY_EDITOR
        public List<NodeMeta> nodeMetas = new();
        // 方便在 Inspector 手动拖改时做一点小校验
        void OnValidate()
        {
            if (entry != null && !states.Contains(entry))
                states.Insert(0, entry);          // 保证 entry 一定在列表里
        }

        [HideInInspector] public string editorFolderPath = GraphViewSetting.BaseFolderPath;
#endif
    }
}