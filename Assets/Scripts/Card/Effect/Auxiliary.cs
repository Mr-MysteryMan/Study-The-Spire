using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Cards.CardEffect
{
    public class ListEffect : IEffect
    {
        private List<IEffect> effects;

        public ListEffect(List<IEffect> effects)
        {
            this.effects = effects;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var effect in effects)
            {
                yield return effect.Work(source, targets);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}