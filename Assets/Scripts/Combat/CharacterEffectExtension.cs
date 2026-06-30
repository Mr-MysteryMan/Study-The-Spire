using System;
using System.Collections.Generic;
using Combat.Buffs;
using Combat.Command;
using Combat.Command.Buff;
using static Combat.CardManager;

namespace Combat
{
    public static class CharacterEffectExtension
    {
        // 攻击target,造成damage点伤害，会触发相应事件
        public static void Attack(this Character source, Character target, int damage)
        {
            source.combatSystem.ProcessCommand(new AttackCommand(source, target, damage, DamageType.Normal));
        }

        // 为target加血，会触发相应事件
        public static void Heal(this Character source, Character target, int heal)
        {
            source.combatSystem.ProcessCommand(new HealCommand(source, target, heal));
        }

        // 为自己加血，会触发相应事件
        public static void Heal(this Character source, int heal)
        {
            source.combatSystem.ProcessCommand(new HealCommand(source, source, heal));
        }

        // 为target添加护甲值，会触发相应事件
        public static void AddAmmor(this Character source, Character target, int ammor)
        {
            source.combatSystem.ProcessCommand(new AddAmmorCommand(source, target, ammor));
        }

        // 为自己添加护甲值，会触发相应事件
        public static void AddAmmor(this Character source, int ammor)
        {
            source.combatSystem.ProcessCommand(new AddAmmorCommand(source, source, ammor));
        }

        public static void AddBuff(this Character source, Character target, IBuff buff, int count = 1)
        {
            source.combatSystem.ProcessCommand(new ApplyBuffCommand(source, target, buff, count));
        }

        public static void DecreaseBuff(this Character source, Character target, IBuff buff, int count = 1)
        {
            source.combatSystem.ProcessCommand(new ApplyBuffCommand(source, target, buff, -count));
        }

        public static void RemoveBuff(this Character source, Character target, Type type)
        {
            source.combatSystem.ProcessCommand(new RemoveBuffCommand(source, target, type));
        }

        public static void ModifyCard(this Character character, ICardData cardData, float factor,
            ModifyType type = ModifyType.All, ModifySubType subType = ModifySubType.Add)
        {
            ModifyCardCommand command = new ModifyCardCommand(character, cardData, factor, type, subType);
            character.combatSystem.ProcessCommand(command);
        }
    }
}