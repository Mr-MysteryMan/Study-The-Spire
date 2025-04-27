using UnityEngine;
using System.Collections.Generic;

namespace Combat.StateMachine
{
    public abstract class ProbState: IState
    {
        public List<Transition> transitions = new List<Transition>(); // 转移列表
        public List<Transition> Transitions => transitions;

        protected StateMachine stateMachine; // 所属状态机

        // 设置状态机引用
        public void SetStateMachine(StateMachine machine)
        {
            stateMachine = machine;
        }

        // 生命周期方法
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        // 根据概率选择下一个状态
        public IState GetNextState()
        {
            if (transitions.Count == 0) return null;

            // 计算总权重
            float totalWeight = 0f;
            foreach (var t in transitions)
                totalWeight += t.probabilityWeight;

            if (totalWeight <= 0) return null;

            // 生成随机点并选择目标状态
            float randomPoint = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var t in transitions)
            {
                currentWeight += t.probabilityWeight;
                if (randomPoint <= currentWeight)
                    return t.targetState;
            }

            return null;
        }
    }
}