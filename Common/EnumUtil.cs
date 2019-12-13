using System;

namespace Common
{
    public static class EnumUtil
    {
        public static TEnum Parse<TEnum>(char chr) where TEnum : struct => Parse<TEnum>(chr.ToString());

        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, out var result) &&
                Enum.IsDefined(typeof(TEnum), result))
            {
                return result;
            }

            throw new InvalidOperationException($"Invalid {typeof(TEnum).Name}: {value}");
        }

        public static TEnum Parse<TEnum>(long value) where TEnum : struct => Parse<TEnum>(value.ToString());

        public static TEnum Parse<TEnum>(int value) where TEnum : struct => Parse<TEnum>(value.ToString());
    }
}
