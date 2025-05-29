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
        public static void Modify(CardData cardData, float factor, ModifyType modifyType)
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
                        value = (int)(value * factor);
                        field.SetValue(cardData, value);
                    }
                }
            }
        }

        public static int BasicModify(int cardValue, float factor, ModifyType type)
        {
            return (int)(cardValue * factor);
        }
    }

    public static class CharacterCardModifier
    {
        public static void Modify(CardData cardData, Character character)
        {
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