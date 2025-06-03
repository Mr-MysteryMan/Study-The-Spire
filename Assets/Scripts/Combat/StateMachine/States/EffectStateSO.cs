using System;
using System.Collections.Generic;
using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    public abstract class EffectStateBaseSO : ScriptableObject, IEffectState
    {
        public abstract IEnemyEffect Effect { get; }

        public abstract IState GetNextState();
        public abstract void OnEnter();
        public abstract void OnExit();

        public void SetNextState(IState nextState) {}
    }
}