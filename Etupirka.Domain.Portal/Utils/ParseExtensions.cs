using System;
using System.Collections.Generic;
using System.Reflection;

namespace Etupirka.Domain.Portal.Utils
{
    public static class ParseExtensions
    {
        public static T TryParse<T>(this string val, T defaultValue)
        {
            if (val == null || val.Trim() == string.Empty)
                return defaultValue;
            if (typeof(T) == typeof(string))
                return (T)(object)val;

            Type t = typeof(T);
            if (t.IsGenericType)
                t = t.GetGenericArguments()[0];

            MethodInfo tryParse = t.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                new Type[] { typeof(string), t.MakeByRefType() },
                new ParameterModifier[] { new ParameterModifier(2) });
            object[] parameters = new object[] { val, Activator.CreateInstance(t) };
            bool success = (bool)tryParse.Invoke(null, parameters);
            //成功返回转换后的值，否则返回类型的默认值
            if (success)
                return (T)parameters[1];

            return defaultValue;
        }

        public static T TryParse<T>(this string val)
        {
            return TryParse<T>(val, default(T));
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            if (dic == null)
                throw new ArgumentNullException(nameof(dic));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            TValue value;
            if (dic.TryGetValue(key, out value))
                return value;
            return default(TValue);
        }
    }
}
