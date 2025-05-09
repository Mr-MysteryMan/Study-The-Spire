using System.Collections.Generic;
using Cards.CardEffect;
using Combat.Characters.EnemyEffect;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat.StateMachine.States
{
    public class EffectState : ProbState
    {
        public IEnemyEffect effect;

        public EffectState(IEnemyEffect effect)
        {
            this.effect = effect;
        }
    }
}