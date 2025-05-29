using System.Collections;
using System.Collections.Generic;
using Combat;
namespace Cards.CardEffect
{
    public interface IEffect
    {
        IEnumerator Work(Character source, List<Character> targets);
    }

    public interface ISyncEffect
    {
        void WorkSync(Character source, List<Character> targets);
    }
}