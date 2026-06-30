using Serializable = System.SerializableAttribute;
using System.Collections;
using System.Collections.Generic;
using Cards.CardEffect;
using Cards.CardEffect.Buffs;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    public class BasicTypedEffect : IEnemyEffect
    {
        public EnemyEffectType EffectType { get; }
        public CardEffectTarget TargetType { get; }

        public IEffect Effect { get; }
        public string Desc { get; }

        public BasicTypedEffect(EnemyEffectType effectType, CardEffectTarget targetType, IEffect effect, string desc)
        {
            EffectType = effectType;
            TargetType = targetType;
            Effect = effect;
            Desc = desc;
        }
    }

    public class ToEnemyEffect : BasicTypedEffect
    {
        public ToEnemyEffect(EnemyEffectType effectType, EnemyEffectTargetType targetType, IEffect effect, string desc)
            : base(effectType, GetTargetType(targetType), MakeEffect(targetType, effect), desc)
        { }

        private static CardEffectTarget GetTargetType(EnemyEffectTargetType targetType)
        {
            return targetType switch
            {
                EnemyEffectTargetType.Self => CardEffectTarget.AllySelf,
                EnemyEffectTargetType.All => CardEffectTarget.AllyAll,
                EnemyEffectTargetType.RandomOne => CardEffectTarget.AllyAll,
                _ => CardEffectTarget.None,
            };
        }

        private class RandomTargetEffectWrapper : IEffect
        {
            private readonly IEffect _effect;

            public RandomTargetEffectWrapper(IEffect effect)
            {
                _effect = effect;
            }

            public IEnumerator Work(Character source, List<Character> targets)
            {
                if (targets == null || targets.Count == 0)
                {
                    yield break; // 如果没有目标，直接返回
                }

                // 随机选择一个目标
                int randomIndex = Random.Range(0, targets.Count);
                Character randomTarget = targets[randomIndex];

                // 执行效果
                yield return _effect.Work(source, new List<Character> { randomTarget });
            }
        }

        private static IEffect MakeEffect(EnemyEffectTargetType targetType, IEffect effect)
        {
            if (targetType == EnemyEffectTargetType.RandomOne)
            {
                return new RandomTargetEffectWrapper(effect);
            }
            else
            {
                return effect;
            }
        }
    }

    public class ToAdventurerEffect : BasicTypedEffect
    {
        public ToAdventurerEffect(EnemyEffectType effectType, IEffect effect, string desc)
            : base(effectType, CardEffectTarget.EnemyOne, effect, desc) { }
    }

    public class TypedEffectLib
    {
        public enum EnemyEffectEnum
        {
            Attack,
            AddAmmorSelf,
            AddAmmorAll,
            AddAmmorRandomOne,
            GainStrength,
            Weaken,
            Fragil,
            Poison,
        }

        private static IEffect _GetEffect(EnemyEffectEnum effectType, int value)
        {
            return effectType switch
            {
                EnemyEffectEnum.Attack => new AttackEffect(value),
                EnemyEffectEnum.AddAmmorSelf => new DefenseEffect(value),
                EnemyEffectEnum.AddAmmorAll => new DefenseEffect(value),
                EnemyEffectEnum.AddAmmorRandomOne => new DefenseEffect(value),
                EnemyEffectEnum.GainStrength => new AddBuffEffect<Strength>(new Strength(), value),
                EnemyEffectEnum.Weaken => new AddBuffEffect<Weak>(new Weak(), value),
                EnemyEffectEnum.Fragil => new AddBuffEffect<Fragil>(new Fragil(), value),
                EnemyEffectEnum.Poison => new AddBuffEffect<Poison>(new Poison(), value),
                _ => null,
            };
        }

        public static IEnemyEffect GetEffect(EnemyEffectEnum effectType, int value)
        {
            return effectType switch
            {
                EnemyEffectEnum.Attack => new BasicTypedEffect(EnemyEffectType.Attack, CardEffectTarget.EnemyOne, _GetEffect(effectType, value), "造成伤害"),
                EnemyEffectEnum.AddAmmorSelf => new ToEnemyEffect(EnemyEffectType.Buff, EnemyEffectTargetType.Self, _GetEffect(effectType, value), "为该敌人增加护甲"),
                EnemyEffectEnum.AddAmmorAll => new ToEnemyEffect(EnemyEffectType.Buff, EnemyEffectTargetType.All, _GetEffect(effectType, value), "为所有敌人增加护甲"),
                EnemyEffectEnum.AddAmmorRandomOne => new ToEnemyEffect(EnemyEffectType.Buff, EnemyEffectTargetType.RandomOne, _GetEffect(effectType, value), "为随机敌人增加护甲"),
                EnemyEffectEnum.GainStrength => new ToEnemyEffect(EnemyEffectType.Buff, EnemyEffectTargetType.Self, _GetEffect(effectType, value), "为该敌人增加力量"),
                EnemyEffectEnum.Weaken => new ToAdventurerEffect(EnemyEffectType.Debuff, _GetEffect(effectType, value), "使我方虚弱"),
                EnemyEffectEnum.Fragil => new ToAdventurerEffect(EnemyEffectType.Debuff, _GetEffect(effectType, value), "使我方脆弱"),
                EnemyEffectEnum.Poison => new ToAdventurerEffect(EnemyEffectType.Debuff, _GetEffect(effectType, value), "使我方中毒"),
                _ => null,
            };
        }
    }

    [Serializable]
    public class TypedEffectInfo
    {
        public TypedEffectLib.EnemyEffectEnum EffectType;
        public int Value;

        public IEnemyEffect GetEnemyEffect()
        {
            return TypedEffectLib.GetEffect(EffectType, Value);
        }
    }
}