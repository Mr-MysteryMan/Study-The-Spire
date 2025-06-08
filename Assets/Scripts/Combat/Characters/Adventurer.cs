using Combat;
using Combat.Events;
using Combat.EventVariable;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace Combat.Characters
{
    /// <summary>
    /// 角色类，表示游戏中的冒险者角色，继承自Character类，主要添加了法力值。
    /// 注意CardManager里也直接管理了法力值，如果不需要每个人一个法力值，下面的代码都没有什么用。
    /// </summary>
    public class Adventurer : Character
    {
        public int Mana => combatSystem.CardManager.EnergyPoint; // 目前直接以CardManager的法力值作为角色的法力值。

        private CharacterInfo oriCharacterInfo; // 原始角色信息，用于战斗结束后设置全局的状态
        public CharacterType CharacterType { get; private set; } // 角色类型

        public void InitOriInfo(CharacterInfo info)
        {
            this.oriCharacterInfo = info;
        }

        public void SetCharacterType(CharacterType type)
        {
            CharacterType = type;
        }

        public CharacterInfo ToInfo()
        {
            return new CharacterInfo(
                type: this.CharacterType, // 角色类型可以根据实际情况设置
                health: this.CurHp,
                maxHealth: this.MaxHp,
                attackPower: AttackPower,
                healPower: HealPower,
                defensePower: DefensePower
            );
        }

        public void SetToGlobalStatus()
        {
            oriCharacterInfo.Set(this.ToInfo());
        }
    }
}