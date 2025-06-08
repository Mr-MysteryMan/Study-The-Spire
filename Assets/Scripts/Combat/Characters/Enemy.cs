using Cards.CardEffect;
using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.Characters
{
    public abstract class Enemy : Character, IEnemy
    {
        public abstract Sprite Indent { get; }

        public abstract IEnemyEffect Effect {get;}
    }
}