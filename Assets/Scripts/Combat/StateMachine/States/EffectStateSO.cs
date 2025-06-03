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

        private bool isInitialized = false;
        public void Init()
        {
            if (isInitialized) return;
            isInitialized = true;
            _Init();
        }

        protected abstract void _Init();

        protected abstract IEffectState EffectState { get; }
    }
}