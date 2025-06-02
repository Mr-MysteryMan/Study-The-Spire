using System;
using System.Collections.Generic;
using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    public abstract class EffectStateBaseSO : ScriptableObject, IEffectState
    {
        public IEnemyEffect Effect => EffectState.Effect;

        public IState GetNextState() => EffectState.GetNextState();
        public void OnEnter()
        {
            EffectState.OnEnter();
        }
        public void OnExit()
        {
            EffectState.OnExit();
        }
        public void SetNextState(IState nextState)
        {
            EffectState.SetNextState(nextState);
        }

        public abstract void Init();

        protected abstract IEffectState EffectState { get; }
    }

    [CreateAssetMenu(fileName = "BasicEffectState", menuName = "Combat/StateMachine/EffectStates/BasicEffectState")]
    public class BasicEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private TypedEffectInfo typedEffect;
        private Basic.BasicEffectState state = new Basic.BasicEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            this.state.Init(Effect);
        }
    }

    [CreateAssetMenu(fileName = "BasicProbEffectState", menuName = "Combat/StateMachine/EffectStates/BasicProbEffectState")]
    public class BasicProbEffectStateSO : EffectStateBaseSO
    {
        [Serializable]
        private class Transition
        {
            public EffectStateBaseSO targetState;  // 目标状态
            public float probabilityWeight; // 转移权重（非归一化）
        }

        [SerializeField] private List<Transition> transitions; // 转移列表

        private Basic.BasicProbEffectState state = new Basic.BasicProbEffectState();
        protected override IEffectState EffectState => state;
        public override void Init()
        {
            this.transitions.ForEach(t => t.targetState.Init());
            this.state.Init(transitions.ConvertAll(t => new Basic.BasicProbEffectState.Transition
            {
                targetState = t.targetState,
                probabilityWeight = t.probabilityWeight
            }));
        }
    }

    [CreateAssetMenu(fileName = "BasicConditionEffectState", menuName = "Combat/StateMachine/EffectStates/BasicConditionEffectState")]
    public class BasicConditionEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private EffectStateBaseSO trueState;
        [SerializeField] private EffectStateBaseSO falseState;
        [SerializeField] private Func<bool> condition;
        private Basic.BasicConditionEffectState state = new Basic.BasicConditionEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            trueState.Init();
            falseState.Init();
            this.state.Init(trueState, falseState, condition);
        }
    }

    [CreateAssetMenu(fileName = "LinearEffectState", menuName = "Combat/StateMachine/EffectStates/LinearEffectState")]
    public class LinearEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private List<EffectStateBaseSO> states;
        private Basic.LinearEffectState state = new Basic.LinearEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            this.states.ForEach(s => s.Init());
            this.state.Init(states.ConvertAll(s => s as IEffectState));
        }
    }

    [CreateAssetMenu(fileName = "LoopEffectState", menuName = "Combat/StateMachine/EffectStates/LoopEffectState")]
    public class LoopEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private List<EffectStateBaseSO> states;
        private Basic.LoopEffectState state = new Basic.LoopEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            this.states.ForEach(s => s.Init());
            this.state.Init(states.ConvertAll(s => s as IEffectState));
        }
    }
}