using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Cards.CardEffect
{
    using EffectCoroutine = Func<Character, List<Character>, IEnumerator>;
    /// <summary>
    /// 一个效果列表，用于组合多个 IEffect 实现的效果。
    /// 多个效果按顺序执行，会等待每个效果的异步执行完成。
    /// </summary>
    public class ListEffect : IEffect, IEnumerable<IEffect>
    {
        private List<IEffect> effects;
        public ListEffect(List<IEffect> effects, EffectCoroutine beforeWork = null,
            EffectCoroutine afterWork = null)
        {
            this.BeforeWork = beforeWork ?? (Wait);
            this.AfterWork = afterWork ?? (Wait);
            this.effects = effects;
        }

        public ListEffect(EffectCoroutine beforeWork = null,
            EffectCoroutine afterWork = null)
            : this(new List<IEffect>(), beforeWork, afterWork)
        { }

        public EffectCoroutine BeforeWork;
        public EffectCoroutine AfterWork;

        private IEnumerator Wait(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return BeforeWork(source, targets);
            foreach (var effect in effects)
            {
                yield return effect.Work(source, targets);
            }
            yield return AfterWork(source, targets);
        }

        public IEnumerator<IEffect> GetEnumerator()
        {
            return effects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IEffect effect)
        {
            effects.Add(effect);
        }
    }

    /// <summary>
    /// 针对实现了 ISyncEffect 接口的效果的列表。
    /// 使得多个效果可以被组合在一起，并且在执行时能够同步地处理。
    /// 而无需等待每个效果的异步执行（如特效，等待等）。
    /// 特效等异步操作请通过 beforeWork 和 afterWork 来处理。
    /// </summary>
    public class ListSyncEffect : IEffect, IEnumerable<ISyncEffect>
    {
        private List<ISyncEffect> effects;
        public ListSyncEffect(List<ISyncEffect> effects, EffectCoroutine beforeWork = null, EffectCoroutine afterWork = null)
        {
            this.BeforeWork = beforeWork ?? (Wait);
            this.AfterWork = afterWork ?? (Wait);
            this.effects = effects;
        }

        public ListSyncEffect() : this(new List<ISyncEffect>()) { }

        public EffectCoroutine BeforeWork;
        public EffectCoroutine AfterWork;

        private IEnumerator Wait(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return BeforeWork(source, targets);
            foreach (var effect in effects)
            {
                effect.WorkSync(source, targets);
            }
            yield return AfterWork(source, targets);
        }

        public IEnumerator<ISyncEffect> GetEnumerator()
        {
            return effects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ISyncEffect effect)
        {
            effects.Add(effect);
        }
    }
}