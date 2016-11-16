/*
 * SharpTiles, R.Z. Slijp(2008), www.sharptiles.org
 *
 * This file is part of SharpTiles.
 * 
 * SharpTiles is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpTiles is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with SharpTiles.  If not, see <http://www.gnu.org/licenses/>.
 */
 using System;
using System.Collections.Generic;
 using System.Globalization;
 using System.Linq;

namespace org.SharpTiles.Common
{
    public static class TypeConverter
    {
        private static readonly IDictionary<Type, ConvertTo> REGISTERED = new Dictionary<Type, ConvertTo>();
        private static readonly IDictionary<Type, CanConvertTo> REGISTER_CAN = new Dictionary<Type, CanConvertTo>();
        private static readonly ICollection<Type> REGISTERED_NUMERICS = new HashSet<Type>();

        static TypeConverter()
        {
            REGISTERED.Add(typeof (int), (source, culture) => (object) Convert.ToInt32(source));
            REGISTERED.Add(typeof (decimal), (source, culture) => (object) Convert.ToDecimal(source, culture.NumberFormat));
            REGISTERED.Add(typeof (float), (source, culture) => (float) Convert.ToDouble(source, culture.NumberFormat));
            REGISTERED.Add(typeof (double), (source, culture) => (object) Convert.ToDouble(source, culture.NumberFormat));
            REGISTERED.Add(typeof (bool), (source, culture) => (object) Convert.ToBoolean(source));
            REGISTERED.Add(typeof (int?),
                           (source, culture) => source != null ? (object) Convert.ToInt32(source) : null);
            REGISTERED.Add(typeof (decimal?),
                           (source, culture) => source != null ? (object) Convert.ToDecimal(source, culture.NumberFormat) : null);
            REGISTERED.Add(typeof (float?),
                           (source, culture) => source != null ? (float?) Convert.ToDouble(source, culture.NumberFormat) : null);
            REGISTERED.Add(typeof (double?),
                           (source, culture) => source != null ? (object) Convert.ToDouble(source, culture.NumberFormat) : null);
            REGISTERED.Add(typeof (bool?),
                           (source, culture) => source != null ? (object) Convert.ToBoolean(source) : null);
            REGISTERED.Add(typeof (string),
                           (source, culture) => source != null ? source.ToString() : null);

            REGISTER_CAN.Add(typeof(int), (source, culture) =>
            {
                int result;
                if (source == null || "".Equals(source)) return false;
                return int.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(decimal), (source, culture) =>
            {
                decimal result;
                if (source == null || "".Equals(source)) return false;
                return decimal.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(float), (source, culture) =>
            {
                float result;
                if (source == null) return false;
                return float.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(double), (source, culture) =>
            {
                double result;
                if (source == null) return false;
                return double.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(bool), (source, culture) =>
            {
                bool result;
                if (source == null || "".Equals(source)) return false;
                return bool.TryParse(source.ToString(), out result);
            });
            REGISTER_CAN.Add(typeof(int?), (source, culture) =>
            {
                int result;
                if (source == null || "".Equals(source)) return true;
                return int.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(decimal?), (source, culture) =>
            {
                decimal result;
                if (source == null || "".Equals(source)) return true;
                return decimal.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(float?), (source, culture) =>
            {
                float result;
                if (source == null || "".Equals(source)) return true;
                return float.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(double?), (source, culture) =>
            {
                double result;
                if (source == null || "".Equals(source)) return true;
                return double.TryParse(source.ToString(), NumberStyles.Any, culture.NumberFormat, out result);
            });
            REGISTER_CAN.Add(typeof(bool?), (source, culture) =>
            {
                bool result;
                if (source == null || "".Equals(source)) return true;
                return bool.TryParse(source.ToString(), out result);
            });


            REGISTERED_NUMERICS.Add(typeof (int));
            REGISTERED_NUMERICS.Add(typeof (int?));
            REGISTERED_NUMERICS.Add(typeof (decimal));
            REGISTERED_NUMERICS.Add(typeof (decimal?));
            REGISTERED_NUMERICS.Add(typeof (double));
            REGISTERED_NUMERICS.Add(typeof (double?));
            REGISTERED_NUMERICS.Add(typeof (float));
            REGISTERED_NUMERICS.Add(typeof (float?));
            
        }

        public static object To(object source, Type target)
        {
            return To(source, target, CultureInfo.CurrentCulture);
        }

        public static object To(object source, Type target, CultureInfo culture)
        {
            try
            {
                if (source == null ||
                    Equals(target, typeof (object)) ||
                    Equals(source.GetType(), target) ||
                    source.GetType().GetInterfaces().Any(t => Equals(t, target)))
                {
                    return source;
                }
                var formattable = source as IFormattable;
                var text = formattable?.ToString(null, culture) ?? source.ToString();
                return REGISTERED[target](text, culture);
            }
            catch (FormatException)
            {
                throw ConvertException.CannotConvert(target, source);
            }
        }

        public static bool Possible(object source, Type target)
        {
            if (source == null && target.IsValueType) { return false; }
            if (source == null) return true;
            return Possible(source.GetType(), target);
        }


        public static bool Possible(Type source, Type target)
        {
            if (Equals(target, typeof(object)) ||
                Equals(source, target) ||
                source.GetInterfaces().Any(t => Equals(t, target)))
            {
                return true;
            }
            return REGISTERED.ContainsKey(target);
        }


        #region Nested type: ConvertTo

        private delegate object ConvertTo(string o, CultureInfo c);

        private delegate bool CanConvertTo(object o, CultureInfo c);
        

        #endregion

        public static object TryTo(object source, Type target, CultureInfo culture)
        {
            try
            {
                if (source == null ||
                    Equals(target, typeof(object)) ||
                    Equals(source.GetType(), target) ||
                    source.GetType().GetInterfaces().Any(t => Equals(t, target)))
                {
                    return source;
                }
                if (REGISTER_CAN[target](source, culture))
                {
                    return REGISTERED[target](source.ToString(), culture);
                }
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static object TryTo(string value, Type type)
        {
            return TryTo(value, type, CultureInfo.CurrentCulture);
        }

        public static bool IsNumeric(object source)
        {
            return source!=null && REGISTERED_NUMERICS.Contains(source.GetType());
        }

        public static bool IsEnum(object source)
        {
            return source?.GetType().IsEnum ?? false;
        }
    }
}
