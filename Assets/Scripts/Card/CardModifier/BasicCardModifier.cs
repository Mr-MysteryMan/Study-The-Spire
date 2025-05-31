using System;
using System.Linq;
using System.Reflection;
using Combat;
using Combat.Characters;

namespace Cards.Modifier
{
    namespace ModifyAttribute
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class Basic : Attribute
        {
            public ModifyType Type;

            public Basic(ModifyType type)
            {
                Type = type;
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class CharacterPowerAttribute : Attribute
        {
            public CharacterPowerType Type;
            public float Factor;
            public CharacterPowerAttribute(CharacterPowerType type, float factor = 1f)
            {
                Type = type;
                Factor = factor;
            }
        }
    }

    public static class BasicCardModifier
    {
        private static void Modify(this ICardData cardData, float factor, ModifyType modifyType, Func<int, float, int> func)
        {
            Type type = cardData.GetType();
            var modifiableFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(ModifyAttribute.Basic), true) != null && f.FieldType == typeof(int));
            foreach (var field in modifiableFields)
            {
                var attributes = field.GetCustomAttributes(typeof(ModifyAttribute.Basic), true);
                if (attributes.Length > 0 && attributes[0] is ModifyAttribute.Basic attr)
                {
                    if (attr.Type == modifyType || modifyType == ModifyType.All)
                    {
                        int value = (int)field.GetValue(cardData);
                        value = func(value, factor);
                        field.SetValue(cardData, value);
                    }
                }
            }
        }

        public static void ModifyAdd(this ICardData cardData, float factor, ModifyType modifyType)
        {
            if (cardData is IBasicModifiable modifiable)
            {
                modifiable.BasicModifyAdd(factor, modifyType);
                return;
            }
            Modify(cardData, factor, modifyType, (value, factor) => (int)(value + factor));
        }

        public static void ModifyMul(this ICardData cardData, float factor, ModifyType modifyType)
        {
            if (cardData is IBasicModifiable modifiable)
            {
                modifiable.BasicModifyMul(factor, modifyType);
                return;
            }
            Modify(cardData, factor, modifyType, (value, factor) => (int)(value * factor));
        }
    }

    public static class CharacterCardModifier
    {
        public static void Modify(this ICardData cardData, Character character)
        {
            if (cardData is ICharacterModifiable modifiable)
            {
                modifiable.CharacterModify(character);
                return;
            }
            Type type = cardData.GetType();
            var modifiableFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(ModifyAttribute.CharacterPowerAttribute), true) != null && f.FieldType == typeof(int));
            foreach (var field in modifiableFields)
            {
                var attributes = field.GetCustomAttributes(typeof(ModifyAttribute.CharacterPowerAttribute), true);
                if (attributes.Length > 0 && attributes[0] is ModifyAttribute.CharacterPowerAttribute attr)
                {
                    int value = (int)field.GetValue(cardData);
                    value = CharacterPowerModify(value, character, attr.Type, attr.Factor);
                    field.SetValue(cardData, value);
                }
            }
        }

        public static int CharacterPowerModify(int cardValue, Character character, CharacterPowerType type, float factor = 1f)
        {
            return (int)(cardValue + character.GetPowerValue(type) * factor);
        }
    }
}