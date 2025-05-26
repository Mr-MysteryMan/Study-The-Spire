using System.Collections;
using System.Collections.Generic;
using Combat;
namespace Cards.CardEffect
{
    public interface IEffect
    {
        void Work(Character source, List<Character> targets);
    }

    public interface IAsnycEffect
    {
        IEnumerator WorkAsync(Character source, List<Character> targets);
    }
}