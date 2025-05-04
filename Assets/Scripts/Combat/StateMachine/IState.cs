using UnityEngine;
using System.Collections.Generic;
using System;

namespace Combat.StateMachine
{
    public interface IState
    {
        public List<Transition> Transitions { get; }

        // 生命周期方法
        void OnEnter();
        void OnUpdate();
        void OnExit();

        // 根据概率选择下一个状态
        IState GetNextState();

        void SetStateMachine(StateMachine machine);
    }

    [Serializable]
    public class Transition
    {
        public IState targetState;  // 目标状态
        public float probabilityWeight; // 转移权重（非归一化）
    }
}