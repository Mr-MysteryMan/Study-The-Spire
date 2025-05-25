using System;
using System.Linq;

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
    }

    public static class BasicCardModifier
    {
        public static void Modify(CardData cardData, float factor, ModifyType modifyType)
        {
            Type type = cardData.GetType();
            var modifiableFields = type.GetFields()
                .Where(f => f.GetCustomAttributes(typeof(ModifyAttribute.Basic), true) != null && f.FieldType == typeof(int))
                .Where(f => ((ModifyAttribute.Basic)f.GetCustomAttributes(typeof(ModifyAttribute.Basic), true).First()).Type == modifyType || modifyType == ModifyType.All);
            foreach (var field in modifiableFields)
            {
                int value = (int)field.GetValue(cardData);
                value = (int)(value * factor);
                field.SetValue(cardData, value);
            }
        }
    }
}