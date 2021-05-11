using System;

namespace ToyRobot.Domain
{
    interface IEnumParser
    {
        TEnum Parse<TEnum>(string text, bool ignoreCase = true) where TEnum : struct;
    }

    class EnumParser : IEnumParser
    {
        public TEnum Parse<TEnum>(string text, bool ignoreCase = true) where TEnum : struct
        {
            bool success = Enum.TryParse(text, ignoreCase, out TEnum enumValue);
            if (success)
            {
                return enumValue;
            }

            throw new ArgumentException($"Value [{text}] is not supported by {typeof(TEnum).Name} enum");
        }
    }
}