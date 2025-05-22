using System.Collections;
using Combat;
namespace Cards.CardEffect
{
    public interface IEffect
    {
        void Work(Character source, Character target);
    }

    public interface IAsnycEffect
    {
        IEnumerator WorkAsync(Character source, Character target);
    }
}