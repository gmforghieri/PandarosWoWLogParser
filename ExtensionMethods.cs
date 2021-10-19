using Autofac;
using CombatLogParser;
using System;
using System.Collections.Generic;

namespace PandarosWoWLogParser
{
    /// <summary>
    /// Extension methods that convert combat log string values to their corresponding C# types
    /// </summary>
    public static class Extensions
    {
        public static SpellSchool ToSpellSchool(this string s)
        {
            return (SpellSchool)(s.ToInt());
        }

        public static int ToInt(this string s)
        {
            if (s == "nil")
                return 0;
            if (s.Length > 2 && s.Substring(0, 2) == "0x")
                return Convert.ToInt32(s, 16);
            else
                return int.Parse(s);
        }

        public static float ToFloat(this string s)
        {
            return float.Parse(s);
        }

        public static bool ToBool(this string s)
        {
            return !string.IsNullOrEmpty(s) && s == "1";
        }

        public static UnitFlags ToUnitFlags(this string s)
        {
            return new UnitFlags(s.ToInt());
        }

        public static PowerType ToPowerType(this string s)
        {
            return (PowerType)(s.ToInt());
        }

        public static string ToCustomString(this DateTime dt)
        {
            return $"{dt.Hour}:{dt.Minute}:{dt.Second}.{dt.Millisecond}";
        }

        public static BuffType ToBuffType(this string s)
        {
            return s == "BUFF" ? BuffType.Buff : BuffType.Debuff;
        }

        public static InstanceDifficulty ToInstanceDifficulty(this string s)
        {
            return (InstanceDifficulty)(s.ToInt());
        }

        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.Ticks - DatetimeMinTimeTicks) / 10000);
        }

        public static void AddValue<T>(this Dictionary<T, long> dic, T key, long value)
        {
            if (dic.TryGetValue(key, out long existingVal))
            {
                dic[key] = existingVal + value;
            }
            else
            {
                dic[key] = value;
            }
        }

        public static void AddValue<T>(this Dictionary<T, int> dic, T key, int value)
        {
            if (dic.TryGetValue(key, out int existingVal))
            {
                dic[key] = existingVal + value;
            }
            else
            {
                dic[key] = value;
            }
        }

        public static void AddValue<T, G>(this Dictionary<T, Dictionary<G, long>> dic, T key, G key2, long value)
        {
            if (!dic.TryGetValue(key, out Dictionary<G, long> dic2))
            {
                dic2 = new Dictionary<G, long>();
                dic.Add(key, dic2);
            }

            if (dic2.TryGetValue(key2, out long existingVal))
            {
                dic2[key2] = existingVal + value;
            }
            else
            {
                dic2[key2] = value;
            }
        }
    }
}
