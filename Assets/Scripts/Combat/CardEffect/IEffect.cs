namespace Combat.CardEffect
{
    public interface IEffect
    {
        void Work(Character source, Character target);
    }
}