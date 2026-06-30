using System;
using Combat.Command;

namespace Combat
{
    public static class CharacterExtension
    {
        public static int GetAttackDamgage(this Character character, Character target, int damage, DamageType damageType)
        {
            AttackCommand command = new AttackCommand(character, target, damage, damageType);
            character.combatSystem.ProcessOnly(ref command);
            return command.BaseDamage;
        }

        public static int GetHealAmount(this Character character, Character target, int heal)
        {
            HealCommand command = new HealCommand(character, target, heal);
            character.combatSystem.ProcessOnly(ref command);
            return command.HealAmount;
        }

        public static int GetAmmorAmount(this Character character, Character target, int ammor)
        {
            AddAmmorCommand command = new AddAmmorCommand(character, target, ammor);
            character.combatSystem.ProcessOnly(ref command);
            return command.AmmorAmount;
        }
    }
}