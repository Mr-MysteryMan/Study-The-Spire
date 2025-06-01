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
    }
}