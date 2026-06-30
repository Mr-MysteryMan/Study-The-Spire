using System.Collections.Generic;
using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicProbEffectState", menuName = "Combat/StateMachine/EffectStates/BasicProbEffectState")]
    public class BasicProbEffectStateSO : EffectStateBaseSO
    {
        [System.Serializable]
        public class Transition
        {
            public EffectStateBaseSO targetState;  // 目标状态
            public float probabilityWeight; // 转移权重（非归一化）
        }

        [SerializeField] private List<Transition> transitions = new List<Transition>(); // 转移列表

        private IEffectState state;
        public List<Transition> Transitions => transitions;

        public override IEnemyEffect Effect => state.Effect;

        public void AddTransition(EffectStateBaseSO targetState, float probabilityWeight)
        {
            transitions.Add(new Transition { targetState = targetState, probabilityWeight = probabilityWeight });
        }

        public override IState GetNextState()
        {
            return state.GetNextState();
        }

        private IEffectState ChoseState()
        {
            if (Transitions.Count == 0)
            {
                throw new System.InvalidOperationException("转移列表不能为空");
            }

            // 计算总权重
            float totalWeight = 0f;
            foreach (var t in Transitions)
                totalWeight += t.probabilityWeight;

            if (totalWeight <= 0)
            {
                throw new System.InvalidOperationException("总权重必须大于0");
            }

            // 生成随机点并选择目标状态
            float randomPoint = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var t in Transitions)
            {
                currentWeight += t.probabilityWeight;
                if (randomPoint <= currentWeight)
                    return t.targetState;
            }

            throw new System.InvalidOperationException("没有找到符合条件的目标状态");
        }

        public override void OnEnter()
        {
            state = ChoseState();
            state.OnEnter();
        }

        public override void OnExit() { }
    }
}