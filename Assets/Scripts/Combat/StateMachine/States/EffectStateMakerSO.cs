using UnityEngine;
using System.Collections.Generic;
using Cards.CardEffect;
using System;
using Combat.Characters.EnemyEffect;
namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "EffectStateMaker", menuName = "Combat/StateMachine/EffectStateMaker")]
    public class EffectStateMakerSO : ScriptableObject
    {
        [Serializable]
        public struct EffectWithWeight
        {
            public TypeToIconEffectSO effect;
            public int weight;
        }

        [SerializeField] public List<EffectWithWeight> effectSOList;

        private List<Transition> transitions = null;
        private List<EffectState> effectStates = null;

        public EffectState CreateEffectState()
        {
            if (effectSOList.Count == 0)
            {
                Debug.LogError("EffectSOList is empty!");
                return null;
            }

            effectStates = new List<EffectState>();
            transitions = new List<Transition>();

            for (int i = 0; i < effectSOList.Count; i++)
            {
                TypeToIconEffectSO effectSO = effectSOList[i].effect;
                int weight = effectSOList[i].weight;
                EffectState effectState = new(effectSO);
                effectStates.Add(effectState);
                Transition transition = new()
                {
                    targetState = effectState,
                    probabilityWeight = weight
                };
                transitions.Add(transition);
            }

            for (int i = 0; i < effectStates.Count; i++)
            {
                effectStates[i].transitions = transitions;
            }

            return effectStates[0];
        }

        public EffectState State { 
            get { 
                if (effectStates == null) 
                    CreateEffectState(); 
                return effectStates?[0]; 
            } 
        }
    }
}