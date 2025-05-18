using System;
using System.Linq;

namespace Cards.Modifier
{
    namespace ModifyAttribute
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class Basic : Attribute { }
    }

    public static class BasicCardModifier
    {
        public static void Modify(CardData cardData, float factor)
        {
            Type type = cardData.GetType();
            var modifiableFields = type.GetFields()
                .Where(f => f.GetCustomAttributes(typeof(ModifyAttribute.Basic), true) != null && f.FieldType == typeof(int));
            foreach (var field in modifiableFields)
            {
                int value = (int)field.GetValue(cardData);
                value = (int)(value * Math.Pow(factor, 2));
                field.SetValue(cardData, value);
            }
        }
    }
}