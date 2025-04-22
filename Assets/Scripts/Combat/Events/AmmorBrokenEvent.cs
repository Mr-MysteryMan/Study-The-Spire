public class AmmorBrokenEvent {
    public int AmmorDamage { get; } // 护甲伤害
    public int HPDamage { get; } // 生命值伤害

    public CharacterData Source { get; } // 伤害来源
    public CharacterData Target { get; } // 伤害目标

    public AmmorBrokenEvent(int ammorDamage, int hpDamage, CharacterData source, CharacterData target) {
        AmmorDamage = ammorDamage;
        HPDamage = hpDamage;
        Source = source;
        Target = target;
    }
}