namespace Combat.Characters
{
    public enum CharacterPowerType
    {
        Attack,
        Heal,
        Defense,
    }

    public static class CharacterPowerExtensions
    {
        public static int GetPowerValue(this Character character, CharacterPowerType type)
        {
            return type switch
            {
                CharacterPowerType.Attack => character.AttackPower,
                CharacterPowerType.Heal => character.HealPower,
                CharacterPowerType.Defense => character.DefensePower,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static void SetPowerValue(this Character character, CharacterPowerType type, int value)
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