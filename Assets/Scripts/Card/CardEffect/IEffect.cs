using Combat;
namespace Cards.CardEffect
{
    public interface IEffect
    {
        void Work(Character source, Character target);
    }
}