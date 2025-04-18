namespace Combat
{
    public class CharacterData
    {
        public int MaxHp { get; private set; }
        public int HP { get; private set; }
        public int Ammor { get; private set; }

        public CharacterData(int maxHp, int hp, int ammor)
        {
            MaxHp = maxHp;
            HP = hp;
            Ammor = ammor;
        }

        public void TakeAmmorDamage(int damage)
        {
            Ammor -= damage;
        }

        public void TakeHpDamage(int damage)
        {
            HP -= damage;
        }

        public void Heal(int heal)
        {
            HP += heal;
        }

        public void AddAmmor(int ammor)
        {
            Ammor += ammor;
        }
    }
}
