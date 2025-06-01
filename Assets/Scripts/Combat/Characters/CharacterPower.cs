namespace Combat.Characters
{
    public enum CharacterPowerType
    {
        Attack,
        Heal,
        Defense,
    }

    public interface IPowered
    {
        int AttackPower { get; set; }
        int HealPower { get; set; }
        int DefensePower { get; set; }
    }

    public static class CharacterPowerExtensions
    {
        public static int GetPowerValue(this IPowered character, CharacterPowerType type)
        {
            return type switch
            {
                CharacterPowerType.Attack => character.AttackPower,
                CharacterPowerType.Heal => character.HealPower,
                CharacterPowerType.Defense => character.DefensePower,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static void SetPowerValue(this IPowered character, CharacterPowerType type, int value)
        {
            switch (type)
            {
                case CharacterPowerType.Attack:
                    character.AttackPower = value;
                    break;
                case CharacterPowerType.Heal:
                    character.HealPower = value;
                    break;
                case CharacterPowerType.Defense:
                    character.DefensePower = value;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}