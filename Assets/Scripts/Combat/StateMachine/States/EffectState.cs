using BoolFunc = System.Func<bool>;
using System.Collections.Generic;
using Cards.CardEffect;
using Combat.Characters.EnemyEffect;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat.StateMachine.States
{
    public interface IEffectState : IState
    {
        public IEnemyEffect Effect { get; }
    }

    namespace Basic
    {
        public class BasicEffectState : IEffectState
        {
            public IEnemyEffect Effect { get; private set; }
            private IState nextState;

            public void SetNextState(IState nextState)
            {
                if (this.nextState != null)
                {
                    throw new System.InvalidOperationException("下一个状态已经设置，不能重复设置");
                }

                this.nextState = nextState;
            }

            public void OnEnter() { }
            public void OnExit() { }

            public void Init(IEnemyEffect effect, IEffectState nextState = null)
            {
                Effect = effect;
                this.nextState = nextState;
            }

            public IState GetNextState()
            {
                return nextState;
            }
        }

        public class BasicProbEffectState : IEffectState
        {
            public class Transition
            {
                public IEffectState targetState;  // 目标状态
                public float probabilityWeight; // 转移权重（非归一化）
            }

            private List<Transition> Transitions; // 转移列表

            private IEffectState curState;
            public IEnemyEffect Effect => curState.Effect;

            public void SetNextState(IState nextState)
            {
                Transitions.ForEach(t => t.targetState.SetNextState(nextState));
            }

            public void OnEnter()
            {
                curState = ChoseState();
                curState.OnEnter();
            }

            public void OnExit()
            {
                curState.OnExit();
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

            public void Init(List<Transition> transitions)
            {
                this.Transitions = transitions;
            }

            // 根据概率选择下一个状态
            public IState GetNextState()
            {
                return curState.GetNextState();
            }
        }

        public class BasicConditionEffectState : IEffectState
        {
            public IEnemyEffect Effect => curState.Effect;
            private IEffectState curState;
            private BoolFunc condition;
            private IEffectState trueSate, falseState;

            public void Init(IEffectState trueState, IEffectState falseState, BoolFunc condition)
            {
                this.condition = condition;
                this.trueSate = trueState;
                this.falseState = falseState;
            }

            public void SetNextState(IState nextState)
            {
                trueSate.SetNextState(nextState);
                falseState.SetNextState(nextState);
            }

            public void OnEnter()
            {
                curState = condition() ? trueSate : falseState;
                curState.OnEnter();
            }

            public void OnExit()
            {
                curState.OnExit();
            }

            public IState GetNextState()
            {
                return curState.GetNextState();
            }
        }

        public class LinearEffectState : IEffectState
        {
            public IEnemyEffect Effect => curState.Effect;
            private IEffectState curState;

            private List<IEffectState> states;

            public void Init(List<IEffectState> states)
            {
                if (states == null || states.Count == 0)
                {
                    throw new System.ArgumentException("Effects list cannot be null or empty");
                }

                this.states = states;
                curState = states[0];

                for (int i = 1; i < states.Count; i++)
                {
                    curState.SetNextState(states[i]);
                    curState = states[i];
                }
            }

            public void SetNextState(IState nextState)
            {
                states[states.Count - 1].SetNextState(nextState);
            }

            public void OnEnter()
            {
                curState.OnEnter();
            }

            public void OnExit()
            {
                curState.OnExit();
            }

            public IState GetNextState()
            {
                return curState.GetNextState();
            }
        }

        public class LoopEffectState : IEffectState
        {
            public IEnemyEffect Effect => curState.Effect;
            private IEffectState curState;

            private List<IEffectState> states;

            public void Init(List<IEffectState> states)
            {
                if (states == null || states.Count == 0)
                {
                    throw new System.ArgumentException("Effects list cannot be null or empty");
                }

                this.states = states;
                curState = states[0];

                for (int i = 1; i < states.Count; i++)
                {
                    curState.SetNextState(states[i]);
                    curState = states[i];
                }

                states[states.Count - 1].SetNextState(states[0]); // 最后一个状态指向第一个状态，形成循环
            }

            public void SetNextState(IState nextState)
            {
                states[states.Count - 1].SetNextState(nextState);
            }

            public void OnEnter()
            {
                curState.OnEnter();
            }

            public void OnExit()
            {
                curState.OnExit();
            }

            public IState GetNextState()
            {
                return curState.GetNextState();
            }
        }
    }

    public class BasicEffectState : Basic.BasicEffectState
    {
        public BasicEffectState(IEnemyEffect effect, IEffectState nextState = null)
        {
            Init(effect, nextState);
        }
    }

    public class BasicProbEffectState : Basic.BasicProbEffectState
    {
        public BasicProbEffectState(List<Transition> transitions)
        {
            Init(transitions);
        }
    }

    public class BasicConditionEffectState : Basic.BasicConditionEffectState
    {
        public BasicConditionEffectState(IEffectState trueState, IEffectState falseState, BoolFunc condition)
        {
            Init(trueState, falseState, condition);
        }
    }

    public class LinearEffectState : Basic.LinearEffectState
    {
        public LinearEffectState(List<IEffectState> states)
        {
            Init(states);
        }
    }

    public class LoopEffectState : Basic.LoopEffectState
    {
        public LoopEffectState(List<IEffectState> states)
        {
            Init(states);
        }
    }
}