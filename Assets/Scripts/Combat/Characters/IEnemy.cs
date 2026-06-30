using Combat.Characters.EnemyEffect;
using UnityEngine;
namespace Combat.Characters
{
    public interface IEnemy
    {
        public Sprite Indent { get; } // 怪物意图的图标
        public IEnemyEffect Effect { get;} // 怪物意图的效果
    }
}