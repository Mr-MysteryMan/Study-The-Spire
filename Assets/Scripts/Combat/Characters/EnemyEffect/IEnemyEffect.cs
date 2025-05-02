using Combat.CardEffect;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{

    public interface IICon
    {
        public Sprite Icon { get; }
    }

    public interface IEnemyEffect : IEffect, IICon, ITypedEffect
    {
    }
}