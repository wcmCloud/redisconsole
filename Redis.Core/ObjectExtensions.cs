using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Redis.Core
{
    public static class ObjectExtensions
    {
        public static string EncryptRSA(this string @this, string key)
        {
            CspParameters cspParameter = new CspParameters()
            {
                KeyContainerName = key
            };
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParameter)
            {
                PersistKeyInCsp = true
            };
            byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), true);
            return BitConverter.ToString(bytes);
        }

        public static string DecryptRSA(this string @this, string key)
        {
            CspParameters cspParameter = new CspParameters()
            {
                KeyContainerName = key
            };
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParameter)
            {
                PersistKeyInCsp = true
            };
            string[] strArrays = new string[] { "-" };
            byte[] decryptByteArray = Array.ConvertAll<string, byte>(@this.Split(strArrays, StringSplitOptions.None), (string s) => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber)));
            byte[] bytes = rsa.Decrypt(decryptByteArray, true);
            return Encoding.UTF8.GetString(bytes);
        }


        public static T As<T>(this object @this)
        {
            return (T)@this;
        }

        public static T AsOrDefault<T>(this object @this)
        {
            T t;
            try
            {
                t = (T)@this;
            }
            catch (Exception exception)
            {
                t = default(T);
            }
            return t;
        }

        public static T AsOrDefault<T>(this object @this, T defaultValue)
        {
            T t;
            try
            {
                t = (T)@this;
            }
            catch (Exception exception)
            {
                t = defaultValue;
            }
            return t;
        }

        public static T AsOrDefault<T>(this object @this, Func<T> defaultValueFactory)
        {
            T t;
            try
            {
                t = (T)@this;
            }
            catch (Exception exception)
            {
                t = defaultValueFactory();
            }
            return t;
        }

        public static T AsOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
        {
            T t;
            try
            {
                t = (T)@this;
            }
            catch (Exception exception)
            {
                t = defaultValueFactory(@this);
            }
            return t;
        }

        public static bool Between<T>(this T @this, T minValue, T maxValue)
        where T : IComparable<T>
        {
            if (minValue.CompareTo(@this) != -1)
            {
                return false;
            }
            return @this.CompareTo(maxValue) == -1;
        }

        public static T Chain<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }

        public static object ChangeType(this object value, TypeCode typeCode)
        {
            return Convert.ChangeType(value, typeCode);
        }

        public static object ChangeType(this object value, TypeCode typeCode, IFormatProvider provider)
        {
            return Convert.ChangeType(value, typeCode, provider);
        }

        public static object ChangeType(this object value, Type conversionType)
        {
            return Convert.ChangeType(value, conversionType);
        }

        public static object ChangeType(this object value, Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(value, conversionType, provider);
        }

        public static object ChangeType<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static object ChangeType<T>(this object value, IFormatProvider provider)
        {
            return (T)Convert.ChangeType(value, typeof(T), provider);
        }

        public static T Coalesce<T>(this T @this, params T[] values)
        where T : class
        {
            if (@this != null)
            {
                return @this;
            }
            T[] tArray = values;
            for (int i = 0; i < (int)tArray.Length; i++)
            {
                T value = tArray[i];
                if (value != null)
                {
                    return value;
                }
            }
            return default(T);
        }

        public static T CoalesceOrDefault<T>(this T @this, params T[] values)
        where T : class
        {
            if (@this != null)
            {
                return @this;
            }
            T[] tArray = values;
            for (int i = 0; i < (int)tArray.Length; i++)
            {
                T value = tArray[i];
                if (value != null)
                {
                    return value;
                }
            }
            return default(T);
        }

        public static T CoalesceOrDefault<T>(this T @this, Func<T> defaultValueFactory, params T[] values)
        where T : class
        {
            if (@this != null)
            {
                return @this;
            }
            T[] tArray = values;
            for (int i = 0; i < (int)tArray.Length; i++)
            {
                T value = tArray[i];
                if (value != null)
                {
                    return value;
                }
            }
            return defaultValueFactory();
        }

        public static T CoalesceOrDefault<T>(this T @this, Func<T, T> defaultValueFactory, params T[] values)
        where T : class
        {
            if (@this != null)
            {
                return @this;
            }
            T[] tArray = values;
            for (int i = 0; i < (int)tArray.Length; i++)
            {
                T value = tArray[i];
                if (value != null)
                {
                    return value;
                }
            }
            return defaultValueFactory(@this);
        }

        public static T DeepClone<T>(this T @this)
        {
            T t;
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, @this);
                stream.Seek((long)0, SeekOrigin.Begin);
                t = (T)formatter.Deserialize(stream);
            }
            return t;
        }

        public static object GetCustomAttribute(this object @this, Type attribute, bool inherit)
        {
            return @this.GetType().GetCustomAttributes(attribute, inherit)[0];
        }

        public static T GetCustomAttribute<T>(this object @this, bool inherit)
        where T : Attribute
        {
            return (T)@this.GetType().GetCustomAttributes(typeof(T), inherit)[0];
        }

        public static string GetCustomAttributeDescription(this object value)
        {
            DescriptionAttribute attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault<object>() as DescriptionAttribute;
            return attr.Description;
        }

        public static object[] GetCustomAttributes(this object @this, bool inherit)
        {
            return @this.GetType().GetCustomAttributes(inherit);
        }

        public static object[] GetCustomAttributes<T>(this object @this, bool inherit)
        where T : Attribute
        {
            return @this.GetType().GetCustomAttributes(typeof(T), inherit);
        }

        public static FieldInfo GetField<T>(this T @this, string name)
        {
            return @this.GetType().GetField(name);
        }

        public static FieldInfo GetField<T>(this T @this, string name, BindingFlags bindingAttr)
        {
            return @this.GetType().GetField(name, bindingAttr);
        }

        public static FieldInfo[] GetFields<T>(this T @this)
        {
            return @this.GetType().GetFields();
        }

        public static FieldInfo[] GetFields<T>(this T @this, BindingFlags bindingAttr)
        {
            return @this.GetType().GetFields(bindingAttr);
        }

        public static object GetFieldValue<T>(this T @this, string fieldName)
        {
            Type type = @this.GetType();
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(@this);
        }

        public static MethodInfo GetMethod<T>(this T @this, string name)
        {
            return @this.GetType().GetMethod(name);
        }

        public static MethodInfo GetMethod<T>(this T @this, string name, BindingFlags bindingAttr)
        {
            return @this.GetType().GetMethod(name, bindingAttr);
        }

        public static MethodInfo[] GetMethods<T>(this T @this)
        {
            return @this.GetType().GetMethods();
        }

        public static MethodInfo[] GetMethods<T>(this T @this, BindingFlags bindingAttr)
        {
            return @this.GetType().GetMethods(bindingAttr);
        }

        public static PropertyInfo[] GetProperties<T>(this T @this)
        {
            return @this.GetType().GetProperties();
        }

        public static PropertyInfo[] GetProperties<T>(this T @this, BindingFlags bindingAttr)
        {
            return @this.GetType().GetProperties(bindingAttr);
        }

        public static PropertyInfo GetProperty<T>(this T @this, string name)
        {
            return @this.GetType().GetProperty(name);
        }

        public static PropertyInfo GetProperty<T>(this T @this, string name, BindingFlags bindingAttr)
        {
            return @this.GetType().GetProperty(name, bindingAttr);
        }

        public static object GetPropertyValue<T>(this T @this, string propertyName)
        {
            Type type = @this.GetType();
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            return property.GetValue(@this, null);
        }

        public static TypeCode GetTypeCode(this object value)
        {
            return Convert.GetTypeCode(value);
        }

        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func)
        {
            TResult tResult;
            try
            {
                tResult = func(@this);
            }
            catch (Exception exception)
            {
                tResult = default(TResult);
            }
            return tResult;
        }

        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue)
        {
            TResult tResult;
            try
            {
                tResult = func(@this);
            }
            catch (Exception exception)
            {
                tResult = defaultValue;
            }
            return tResult;
        }

        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory)
        {
            TResult tResult;
            try
            {
                tResult = func(@this);
            }
            catch (Exception exception)
            {
                tResult = defaultValueFactory();
            }
            return tResult;
        }

        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<T, TResult> defaultValueFactory)
        {
            TResult tResult;
            try
            {
                tResult = func(@this);
            }
            catch (Exception exception)
            {
                tResult = defaultValueFactory(@this);
            }
            return tResult;
        }

        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func)
        where T : class
        {
            if (@this == null)
            {
                return default(TResult);
            }
            return func(@this);
        }

        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue)
        where T : class
        {
            if (@this == null)
            {
                return defaultValue;
            }
            return func(@this);
        }

        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory)
        where T : class
        {
            if (@this == null)
            {
                return defaultValueFactory();
            }
            return func(@this);
        }

        public static bool In<T>(this T @this, params T[] values)
        {
            return Array.IndexOf<T>(values, @this) != -1;
        }

        public static bool InRange<T>(this T @this, T minValue, T maxValue)
        where T : IComparable<T>
        {
            if (@this.CompareTo(minValue) < 0)
            {
                return false;
            }
            return @this.CompareTo(maxValue) <= 0;
        }

        public static object InvokeMethod<T>(this T obj, string methodName, params object[] parameters)
        {
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(methodName, (
                from o in parameters
                select o.GetType()).ToArray<Type>());
            return method.Invoke(obj, parameters);
        }

        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
        {
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(methodName, (
                from o in parameters
                select o.GetType()).ToArray<Type>());
            object value = method.Invoke(obj, parameters);
            if (value is T)
            {
                return (T)value;
            }
            return default(T);
        }

        public static bool IsArray<T>(this T @this)
        {
            return @this.GetType().IsArray;
        }

        public static bool IsAssignableFrom<T>(this object @this)
        {
            return @this.GetType().IsAssignableFrom(typeof(T));
        }

        public static bool IsAssignableFrom(this object @this, Type targetType)
        {
            return @this.GetType().IsAssignableFrom(targetType);
        }

        public static bool IsAttributeDefined(this object @this, Type attributeType, bool inherit)
        {
            return @this.GetType().IsDefined(attributeType, inherit);
        }

        public static bool IsAttributeDefined<T>(this object @this, bool inherit)
        where T : Attribute
        {
            return @this.GetType().IsDefined(typeof(T), inherit);
        }

        public static bool IsClass<T>(this T @this)
        {
            return @this.GetType().IsClass;
        }

        public static bool IsDBNull<T>(this T value)
        where T : class
        {
            return Convert.IsDBNull(value);
        }

        public static bool IsDefault<T>(this T source)
        {
            return source.Equals(default(T));
        }

        public static bool IsEnum<T>(this T @this)
        {
            return @this.GetType().IsEnum;
        }

        public static bool IsNotNull<T>(this T @this)
        where T : class
        {
            return @this != null;
        }

        public static bool IsNull<T>(this T @this)
        where T : class
        {
            return @this == null;
        }

        public static bool IsSubclassOf<T>(this T @this, Type type)
        {
            return @this.GetType().IsSubclassOf(type);
        }

        public static bool IsTypeOf<T>(this T @this, Type type)
        {
            return @this.GetType() == type;
        }

        public static bool IsTypeOrInheritsOf<T>(this T @this, Type type)
        {
            for (Type objectType = @this.GetType(); !objectType.Equals(type); objectType = objectType.BaseType)
            {
                if (objectType == objectType.BaseType || objectType.BaseType == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidBoolean(this object @this)
        {
            bool result;
            if (@this == null)
            {
                return true;
            }
            return bool.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidByte(this object @this)
        {
            byte result;
            if (@this == null)
            {
                return true;
            }
            return byte.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidChar(this object @this)
        {
            char result;
            return char.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidDateTime(this object @this)
        {
            DateTime result;
            if (@this == null)
            {
                return true;
            }
            return DateTime.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidDateTimeOffSet(this object @this)
        {
            DateTimeOffset result;
            if (@this == null)
            {
                return true;
            }
            return DateTimeOffset.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidDecimal(this object @this)
        {
            decimal result;
            if (@this == null)
            {
                return true;
            }
            return decimal.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidDouble(this object @this)
        {
            double result;
            if (@this == null)
            {
                return true;
            }
            return double.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidFloat(this object @this)
        {
            float result;
            if (@this == null)
            {
                return true;
            }
            return float.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidGuid(this object @this)
        {
            Guid result;
            return Guid.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidInt16(this object @this)
        {
            short result;
            if (@this == null)
            {
                return true;
            }
            return short.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidInt32(this object @this)
        {
            int result;
            if (@this == null)
            {
                return true;
            }
            return int.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidInt64(this object @this)
        {
            long result;
            if (@this == null)
            {
                return true;
            }
            return long.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidLong(this object @this)
        {
            long result;
            if (@this == null)
            {
                return true;
            }
            return long.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidSByte(this object @this)
        {
            sbyte result;
            if (@this == null)
            {
                return true;
            }
            return sbyte.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidShort(this object @this)
        {
            short result;
            if (@this == null)
            {
                return true;
            }
            return short.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidSingle(this object @this)
        {
            float result;
            if (@this == null)
            {
                return true;
            }
            return float.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidString(this object @this)
        {
            return true;
        }

        public static bool IsValidUInt16(this object @this)
        {
            ushort result;
            if (@this == null)
            {
                return true;
            }
            return ushort.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidUInt32(this object @this)
        {
            uint result;
            if (@this == null)
            {
                return true;
            }
            return uint.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidUInt64(this object @this)
        {
            ulong result;
            if (@this == null)
            {
                return true;
            }
            return ulong.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidULong(this object @this)
        {
            ulong result;
            if (@this == null)
            {
                return true;
            }
            return ulong.TryParse(@this.ToString(), out result);
        }

        public static bool IsValidUShort(this object @this)
        {
            ushort result;
            if (@this == null)
            {
                return true;
            }
            return ushort.TryParse(@this.ToString(), out result);
        }

        public static bool NotIn<T>(this T @this, params T[] values)
        {
            return Array.IndexOf<T>(values, @this) == -1;
        }

        public static T NullIf<T>(this T @this, Func<T, bool> predicate)
        where T : class
        {
            if (!predicate(@this))
            {
                return @this;
            }
            return default(T);
        }

        public static T NullIfEquals<T>(this T @this, T value)
        where T : class
        {
            if (!@this.Equals(value))
            {
                return @this;
            }
            return default(T);
        }

        public static T NullIfEqualsAny<T>(this T @this, params T[] values)
        where T : class
        {
            if (Array.IndexOf<T>(values, @this) == -1)
            {
                return @this;
            }
            return default(T);
        }

        public static new bool ReferenceEquals(this object objA, object objB)
        {
            return object.ReferenceEquals(objA, objB);
        }

        public static string SerializeBinary<T>(this T @this)
        {
            string str;
            BinaryFormatter binaryWrite = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryWrite.Serialize(memoryStream, @this);
                str = Encoding.Default.GetString(memoryStream.ToArray());
            }
            return str;
        }

        public static string SerializeBinary<T>(this T @this, Encoding encoding)
        {
            string str;
            BinaryFormatter binaryWrite = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryWrite.Serialize(memoryStream, @this);
                str = encoding.GetString(memoryStream.ToArray());
            }
            return str;
        }

        //public static string SerializeJavaScript<T>(this T @this)
        //{
        //    return (new JavaScriptSerializer()).Serialize(@this);
        //}

        public static string SerializeJson<T>(this T @this)
        {
            string str;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                str = Encoding.Default.GetString(memoryStream.ToArray());
            }
            return str;
        }

        public static string SerializeJson<T>(this T @this, Encoding encoding)
        {
            string str;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                str = encoding.GetString(memoryStream.ToArray());
            }
            return str;
        }

        public static string SerializeXml(this object @this)
        {
            string end;
            XmlSerializer xmlSerializer = new XmlSerializer(@this.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, @this);
                using (StringReader streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
                {
                    end = streamReader.ReadToEnd();
                }
            }
            return end;
        }

        public static void SetFieldValue<T>(this T @this, string fieldName, object value)
        {
            Type type = @this.GetType();
            type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(@this, value);
        }

        public static void SetPropertyValue<T>(this T @this, string propertyName, object value)
        {
            Type type = @this.GetType();
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            property.SetValue(@this, value, null);
        }

        public static T ShallowCopy<T>(this T @this)
        {
            MethodInfo method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)method.Invoke(@this, null);
        }

        public static T To<T>(this object @this)
        {
            if (@this != null)
            {
                Type targetType = typeof(T);
                if (@this.GetType() == targetType)
                {
                    return (T)@this;
                }
                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null && converter.CanConvertTo(targetType))
                {
                    return (T)converter.ConvertTo(@this, targetType);
                }
                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(@this.GetType()))
                {
                    return (T)converter.ConvertFrom(@this);
                }
                if (@this == DBNull.Value)
                {
                    return default(T);
                }
            }
            return (T)@this;
        }

        public static object To(this object @this, Type type)
        {
            if (@this != null)
            {
                Type targetType = type;
                if (@this.GetType() == targetType)
                {
                    return @this;
                }
                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null && converter.CanConvertTo(targetType))
                {
                    return converter.ConvertTo(@this, targetType);
                }
                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(@this.GetType()))
                {
                    return converter.ConvertFrom(@this);
                }
                if (@this == DBNull.Value)
                {
                    return null;
                }
            }
            return @this;
        }

        public static bool ToBoolean(this object @this)
        {
            return Convert.ToBoolean(@this);
        }

        public static bool ToBooleanOrDefault(this object @this)
        {
            bool flag;
            try
            {
                flag = Convert.ToBoolean(@this);
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        public static bool ToBooleanOrDefault(this object @this, bool defaultValue)
        {
            bool flag;
            try
            {
                flag = Convert.ToBoolean(@this);
            }
            catch (Exception exception)
            {
                flag = defaultValue;
            }
            return flag;
        }

        public static bool ToBooleanOrDefault(this object @this, Func<bool> defaultValueFactory)
        {
            bool flag;
            try
            {
                flag = Convert.ToBoolean(@this);
            }
            catch (Exception exception)
            {
                flag = defaultValueFactory();
            }
            return flag;
        }

        public static byte ToByte(this object @this)
        {
            return Convert.ToByte(@this);
        }

        public static byte ToByteOrDefault(this object @this)
        {
            byte num;
            try
            {
                num = Convert.ToByte(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static byte ToByteOrDefault(this object @this, byte defaultValue)
        {
            byte num;
            try
            {
                num = Convert.ToByte(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static byte ToByteOrDefault(this object @this, Func<byte> defaultValueFactory)
        {
            byte num;
            try
            {
                num = Convert.ToByte(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static char ToChar(this object @this)
        {
            return Convert.ToChar(@this);
        }

        public static char ToCharOrDefault(this object @this)
        {
            char chr;
            try
            {
                chr = Convert.ToChar(@this);
            }
            catch (Exception exception)
            {
                chr = '\0';
            }
            return chr;
        }

        public static char ToCharOrDefault(this object @this, char defaultValue)
        {
            char chr;
            try
            {
                chr = Convert.ToChar(@this);
            }
            catch (Exception exception)
            {
                chr = defaultValue;
            }
            return chr;
        }

        public static char ToCharOrDefault(this object @this, Func<char> defaultValueFactory)
        {
            char chr;
            try
            {
                chr = Convert.ToChar(@this);
            }
            catch (Exception exception)
            {
                chr = defaultValueFactory();
            }
            return chr;
        }

        public static DateTime ToDateTime(this object @this)
        {
            return Convert.ToDateTime(@this);
        }

        public static DateTimeOffset ToDateTimeOffSet(this object @this)
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }

        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this)
        {
            DateTimeOffset dateTimeOffset;
            try
            {
                dateTimeOffset = new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception exception)
            {
                dateTimeOffset = new DateTimeOffset();
            }
            return dateTimeOffset;
        }

        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, DateTimeOffset defaultValue)
        {
            DateTimeOffset dateTimeOffset;
            try
            {
                dateTimeOffset = new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception exception)
            {
                dateTimeOffset = defaultValue;
            }
            return dateTimeOffset;
        }

        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset> defaultValueFactory)
        {
            DateTimeOffset dateTimeOffset;
            try
            {
                dateTimeOffset = new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception exception)
            {
                dateTimeOffset = defaultValueFactory();
            }
            return dateTimeOffset;
        }

        public static DateTime ToDateTimeOrDefault(this object @this)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(@this);
            }
            catch (Exception exception)
            {
                dateTime = new DateTime();
            }
            return dateTime;
        }

        public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(@this);
            }
            catch (Exception exception)
            {
                dateTime = defaultValue;
            }
            return dateTime;
        }

        public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(@this);
            }
            catch (Exception exception)
            {
                dateTime = defaultValueFactory();
            }
            return dateTime;
        }

        public static decimal ToDecimal(this object @this)
        {
            return Convert.ToDecimal(@this);
        }

        public static decimal ToDecimalOrDefault(this object @this)
        {
            decimal num;
            try
            {
                num = Convert.ToDecimal(@this);
            }
            catch (Exception exception)
            {
                num = new decimal(0);
            }
            return num;
        }

        public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue)
        {
            decimal num;
            try
            {
                num = Convert.ToDecimal(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static decimal ToDecimalOrDefault(this object @this, Func<decimal> defaultValueFactory)
        {
            decimal num;
            try
            {
                num = Convert.ToDecimal(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static double ToDouble(this object @this)
        {
            return Convert.ToDouble(@this);
        }

        public static double ToDoubleOrDefault(this object @this)
        {
            double num;
            try
            {
                num = Convert.ToDouble(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static double ToDoubleOrDefault(this object @this, double defaultValue)
        {
            double num;
            try
            {
                num = Convert.ToDouble(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static double ToDoubleOrDefault(this object @this, Func<double> defaultValueFactory)
        {
            double num;
            try
            {
                num = Convert.ToDouble(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static float ToFloat(this object @this)
        {
            return Convert.ToSingle(@this);
        }

        public static float ToFloatOrDefault(this object @this)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = 0f;
            }
            return single;
        }

        public static float ToFloatOrDefault(this object @this, float defaultValue)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = defaultValue;
            }
            return single;
        }

        public static float ToFloatOrDefault(this object @this, Func<float> defaultValueFactory)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = defaultValueFactory();
            }
            return single;
        }

        public static Guid ToGuid(this object @this)
        {
            return new Guid(@this.ToString());
        }

        public static Guid ToGuidOrDefault(this object @this)
        {
            Guid guid;
            try
            {
                guid = new Guid(@this.ToString());
            }
            catch (Exception exception)
            {
                guid = Guid.Empty;
            }
            return guid;
        }

        public static Guid ToGuidOrDefault(this object @this, Guid defaultValue)
        {
            Guid guid;
            try
            {
                guid = new Guid(@this.ToString());
            }
            catch (Exception exception)
            {
                guid = defaultValue;
            }
            return guid;
        }

        public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory)
        {
            Guid guid;
            try
            {
                guid = new Guid(@this.ToString());
            }
            catch (Exception exception)
            {
                guid = defaultValueFactory();
            }
            return guid;
        }

        public static short ToInt16(this object @this)
        {
            return Convert.ToInt16(@this);
        }

        public static short ToInt16OrDefault(this object @this)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static short ToInt16OrDefault(this object @this, short defaultValue)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static int ToInt32(this object @this)
        {
            return Convert.ToInt32(@this);
        }

        public static int ToInt32OrDefault(this object @this)
        {
            int num;
            try
            {
                num = Convert.ToInt32(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static int ToInt32OrDefault(this object @this, int defaultValue)
        {
            int num;
            try
            {
                num = Convert.ToInt32(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory)
        {
            int num;
            try
            {
                num = Convert.ToInt32(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static long ToInt64(this object @this)
        {
            return Convert.ToInt64(@this);
        }

        public static long ToInt64OrDefault(this object @this)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = (long)0;
            }
            return num;
        }

        public static long ToInt64OrDefault(this object @this, long defaultValue)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static long ToInt64OrDefault(this object @this, Func<long> defaultValueFactory)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static long ToLong(this object @this)
        {
            return Convert.ToInt64(@this);
        }

        public static long ToLongOrDefault(this object @this)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = (long)0;
            }
            return num;
        }

        public static long ToLongOrDefault(this object @this, long defaultValue)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static long ToLongOrDefault(this object @this, Func<long> defaultValueFactory)
        {
            long num;
            try
            {
                num = Convert.ToInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static bool? ToNullableBoolean(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new bool?(Convert.ToBoolean(@this));
            }
            return null;
        }

        public static bool? ToNullableBooleanOrDefault(this object @this)
        {
            bool? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new bool?(Convert.ToBoolean(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new bool?(false);
            }
            return nullable;
        }

        public static bool? ToNullableBooleanOrDefault(this object @this, bool? defaultValue)
        {
            bool? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new bool?(Convert.ToBoolean(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static bool? ToNullableBooleanOrDefault(this object @this, Func<bool?> defaultValueFactory)
        {
            bool? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new bool?(Convert.ToBoolean(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static byte? ToNullableByte(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new byte?(Convert.ToByte(@this));
            }
            return null;
        }

        public static byte? ToNullableByteOrDefault(this object @this)
        {
            byte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new byte?(Convert.ToByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new byte?(0);
            }
            return nullable;
        }

        public static byte? ToNullableByteOrDefault(this object @this, byte? defaultValue)
        {
            byte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new byte?(Convert.ToByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static byte? ToNullableByteOrDefault(this object @this, Func<byte?> defaultValueFactory)
        {
            byte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new byte?(Convert.ToByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static char? ToNullableChar(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new char?(Convert.ToChar(@this));
            }
            return null;
        }

        public static char? ToNullableCharOrDefault(this object @this)
        {
            char? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new char?(Convert.ToChar(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new char?('\0');
            }
            return nullable;
        }

        public static char? ToNullableCharOrDefault(this object @this, char? defaultValue)
        {
            char? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new char?(Convert.ToChar(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static char? ToNullableCharOrDefault(this object @this, Func<char?> defaultValueFactory)
        {
            char? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new char?(Convert.ToChar(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static DateTime? ToNullableDateTime(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new DateTime?(Convert.ToDateTime(@this));
            }
            return null;
        }

        public static DateTimeOffset? ToNullableDateTimeOffSet(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }
            return new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero));
        }

        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this)
        {
            DateTimeOffset? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero));
                }
            }
            catch (Exception exception)
            {
                nullable = new DateTimeOffset?(new DateTimeOffset());
            }
            return nullable;
        }

        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, DateTimeOffset? defaultValue)
        {
            DateTimeOffset? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset?> defaultValueFactory)
        {
            DateTimeOffset? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static DateTime? ToNullableDateTimeOrDefault(this object @this)
        {
            DateTime? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTime?(Convert.ToDateTime(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new DateTime?(new DateTime());
            }
            return nullable;
        }

        public static DateTime? ToNullableDateTimeOrDefault(this object @this, DateTime? defaultValue)
        {
            DateTime? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTime?(Convert.ToDateTime(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static DateTime? ToNullableDateTimeOrDefault(this object @this, Func<DateTime?> defaultValueFactory)
        {
            DateTime? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new DateTime?(Convert.ToDateTime(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static decimal? ToNullableDecimal(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new decimal?(Convert.ToDecimal(@this));
            }
            return null;
        }

        public static decimal? ToNullableDecimalOrDefault(this object @this)
        {
            decimal? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new decimal?(Convert.ToDecimal(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new decimal?(new decimal(0));
            }
            return nullable;
        }

        public static decimal? ToNullableDecimalOrDefault(this object @this, decimal? defaultValue)
        {
            decimal? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new decimal?(Convert.ToDecimal(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static decimal? ToNullableDecimalOrDefault(this object @this, Func<decimal?> defaultValueFactory)
        {
            decimal? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new decimal?(Convert.ToDecimal(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static double? ToNullableDouble(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new double?(Convert.ToDouble(@this));
            }
            return null;
        }

        public static double? ToNullableDoubleOrDefault(this object @this)
        {
            double? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new double?(Convert.ToDouble(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new double?(0);
            }
            return nullable;
        }

        public static double? ToNullableDoubleOrDefault(this object @this, double? defaultValue)
        {
            double? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new double?(Convert.ToDouble(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static double? ToNullableDoubleOrDefault(this object @this, Func<double?> defaultValueFactory)
        {
            double? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new double?(Convert.ToDouble(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static float? ToNullableFloat(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new float?(Convert.ToSingle(@this));
            }
            return null;
        }

        public static float? ToNullableFloatOrDefault(this object @this)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new float?(0f);
            }
            return nullable;
        }

        public static float? ToNullableFloatOrDefault(this object @this, float? defaultValue)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static float? ToNullableFloatOrDefault(this object @this, Func<float?> defaultValueFactory)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static Guid? ToNullableGuid(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }
            return new Guid?(new Guid(@this.ToString()));
        }

        public static Guid? ToNullableGuidOrDefault(this object @this)
        {
            Guid? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new Guid?(new Guid(@this.ToString()));
                }
            }
            catch (Exception exception)
            {
                nullable = new Guid?(Guid.Empty);
            }
            return nullable;
        }

        public static Guid? ToNullableGuidOrDefault(this object @this, Guid? defaultValue)
        {
            Guid? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new Guid?(new Guid(@this.ToString()));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static Guid? ToNullableGuidOrDefault(this object @this, Func<Guid?> defaultValueFactory)
        {
            Guid? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new Guid?(new Guid(@this.ToString()));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static short? ToNullableInt16(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new short?(Convert.ToInt16(@this));
            }
            return null;
        }

        public static short? ToNullableInt16OrDefault(this object @this)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new short?(0);
            }
            return nullable;
        }

        public static short? ToNullableInt16OrDefault(this object @this, short? defaultValue)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static short? ToNullableInt16OrDefault(this object @this, Func<short?> defaultValueFactory)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static int? ToNullableInt32(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new int?(Convert.ToInt32(@this));
            }
            return null;
        }

        public static int? ToNullableInt32OrDefault(this object @this)
        {
            int? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new int?(Convert.ToInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new int?(0);
            }
            return nullable;
        }

        public static int? ToNullableInt32OrDefault(this object @this, int? defaultValue)
        {
            int? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new int?(Convert.ToInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static int? ToNullableInt32OrDefault(this object @this, Func<int?> defaultValueFactory)
        {
            int? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new int?(Convert.ToInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static long? ToNullableInt64(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new long?(Convert.ToInt64(@this));
            }
            return null;
        }

        public static long? ToNullableInt64OrDefault(this object @this)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new long?((long)0);
            }
            return nullable;
        }

        public static long? ToNullableInt64OrDefault(this object @this, long? defaultValue)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static long? ToNullableInt64OrDefault(this object @this, Func<long?> defaultValueFactory)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static long? ToNullableLong(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new long?(Convert.ToInt64(@this));
            }
            return null;
        }

        public static long? ToNullableLongOrDefault(this object @this)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new long?((long)0);
            }
            return nullable;
        }

        public static long? ToNullableLongOrDefault(this object @this, long? defaultValue)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static long? ToNullableLongOrDefault(this object @this, Func<long?> defaultValueFactory)
        {
            long? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new long?(Convert.ToInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static sbyte? ToNullableSByte(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new sbyte?(Convert.ToSByte(@this));
            }
            return null;
        }

        public static sbyte? ToNullableSByteOrDefault(this object @this)
        {
            sbyte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new sbyte?(Convert.ToSByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new sbyte?(0);
            }
            return nullable;
        }

        public static sbyte? ToNullableSByteOrDefault(this object @this, sbyte? defaultValue)
        {
            sbyte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new sbyte?(Convert.ToSByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static sbyte? ToNullableSByteOrDefault(this object @this, Func<sbyte?> defaultValueFactory)
        {
            sbyte? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new sbyte?(Convert.ToSByte(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static short? ToNullableShort(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new short?(Convert.ToInt16(@this));
            }
            return null;
        }

        public static short? ToNullableShortOrDefault(this object @this)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new short?(0);
            }
            return nullable;
        }

        public static short? ToNullableShortOrDefault(this object @this, short? defaultValue)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static short? ToNullableShortOrDefault(this object @this, Func<short?> defaultValueFactory)
        {
            short? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new short?(Convert.ToInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static float? ToNullableSingle(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new float?(Convert.ToSingle(@this));
            }
            return null;
        }

        public static float? ToNullableSingleOrDefault(this object @this)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new float?(0f);
            }
            return nullable;
        }

        public static float? ToNullableSingleOrDefault(this object @this, float? defaultValue)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static float? ToNullableSingleOrDefault(this object @this, Func<float?> defaultValueFactory)
        {
            float? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new float?(Convert.ToSingle(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static ushort? ToNullableUInt16(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new ushort?(Convert.ToUInt16(@this));
            }
            return null;
        }

        public static ushort? ToNullableUInt16OrDefault(this object @this)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new ushort?(0);
            }
            return nullable;
        }

        public static ushort? ToNullableUInt16OrDefault(this object @this, ushort? defaultValue)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static ushort? ToNullableUInt16OrDefault(this object @this, Func<ushort?> defaultValueFactory)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static uint? ToNullableUInt32(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new uint?(Convert.ToUInt32(@this));
            }
            return null;
        }

        public static uint? ToNullableUInt32OrDefault(this object @this)
        {
            uint? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new uint?(Convert.ToUInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new uint?(0);
            }
            return nullable;
        }

        public static uint? ToNullableUInt32OrDefault(this object @this, uint? defaultValue)
        {
            uint? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new uint?(Convert.ToUInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static uint? ToNullableUInt32OrDefault(this object @this, Func<uint?> defaultValueFactory)
        {
            uint? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new uint?(Convert.ToUInt32(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static ulong? ToNullableUInt64(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new ulong?(Convert.ToUInt64(@this));
            }
            return null;
        }

        public static ulong? ToNullableUInt64OrDefault(this object @this)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new ulong?((ulong)0);
            }
            return nullable;
        }

        public static ulong? ToNullableUInt64OrDefault(this object @this, ulong? defaultValue)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static ulong? ToNullableUInt64OrDefault(this object @this, Func<ulong?> defaultValueFactory)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static ulong? ToNullableULong(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new ulong?(Convert.ToUInt64(@this));
            }
            return null;
        }

        public static ulong? ToNullableULongOrDefault(this object @this)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new ulong?((ulong)0);
            }
            return nullable;
        }

        public static ulong? ToNullableULongOrDefault(this object @this, ulong? defaultValue)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static ulong? ToNullableULongOrDefault(this object @this, Func<ulong?> defaultValueFactory)
        {
            ulong? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ulong?(Convert.ToUInt64(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static ushort? ToNullableUShort(this object @this)
        {
            if (@this != null && @this != DBNull.Value)
            {
                return new ushort?(Convert.ToUInt16(@this));
            }
            return null;
        }

        public static ushort? ToNullableUShortOrDefault(this object @this)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = new ushort?(0);
            }
            return nullable;
        }

        public static ushort? ToNullableUShortOrDefault(this object @this, ushort? defaultValue)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValue;
            }
            return nullable;
        }

        public static ushort? ToNullableUShortOrDefault(this object @this, Func<ushort?> defaultValueFactory)
        {
            ushort? nullable;
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    nullable = null;
                }
                else
                {
                    nullable = new ushort?(Convert.ToUInt16(@this));
                }
            }
            catch (Exception exception)
            {
                nullable = defaultValueFactory();
            }
            return nullable;
        }

        public static T ToOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
        {
            T t;
            try
            {
                if (@this != null)
                {
                    Type targetType = typeof(T);
                    if (@this.GetType() != targetType)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(@this);
                        if (converter == null || !converter.CanConvertTo(targetType))
                        {
                            converter = TypeDescriptor.GetConverter(targetType);
                            if (converter != null && converter.CanConvertFrom(@this.GetType()))
                            {
                                t = (T)converter.ConvertFrom(@this);
                                return t;
                            }
                            else if (@this == DBNull.Value)
                            {
                                t = default(T);
                                return t;
                            }
                        }
                        else
                        {
                            t = (T)converter.ConvertTo(@this, targetType);
                            return t;
                        }
                    }
                    else
                    {
                        t = (T)@this;
                        return t;
                    }
                }
                t = (T)@this;
            }
            catch (Exception exception)
            {
                t = defaultValueFactory(@this);
            }
            return t;
        }

        public static T ToOrDefault<T>(this object @this, Func<T> defaultValueFactory)
        {
            return @this.ToOrDefault<T>((object x) => defaultValueFactory());
        }

        public static T ToOrDefault<T>(this object @this)
        {
            return @this.ToOrDefault<T>((object x) => default(T));
        }

        public static T ToOrDefault<T>(this object @this, T defaultValue)
        {
            return @this.ToOrDefault<T>((object x) => defaultValue);
        }

        public static sbyte ToSByte(this object @this)
        {
            return Convert.ToSByte(@this);
        }

        public static sbyte ToSByteOrDefault(this object @this)
        {
            sbyte num;
            try
            {
                num = Convert.ToSByte(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static sbyte ToSByteOrDefault(this object @this, sbyte defaultValue)
        {
            sbyte num;
            try
            {
                num = Convert.ToSByte(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static sbyte ToSByteOrDefault(this object @this, Func<sbyte> defaultValueFactory)
        {
            sbyte num;
            try
            {
                num = Convert.ToSByte(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static short ToShort(this object @this)
        {
            return Convert.ToInt16(@this);
        }

        public static short ToShortOrDefault(this object @this)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static short ToShortOrDefault(this object @this, short defaultValue)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static short ToShortOrDefault(this object @this, Func<short> defaultValueFactory)
        {
            short num;
            try
            {
                num = Convert.ToInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static float ToSingle(this object @this)
        {
            return Convert.ToSingle(@this);
        }

        public static float ToSingleOrDefault(this object @this)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = 0f;
            }
            return single;
        }

        public static float ToSingleOrDefault(this object @this, float defaultValue)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = defaultValue;
            }
            return single;
        }

        public static float ToSingleOrDefault(this object @this, Func<float> defaultValueFactory)
        {
            float single;
            try
            {
                single = Convert.ToSingle(@this);
            }
            catch (Exception exception)
            {
                single = defaultValueFactory();
            }
            return single;
        }

        public static string ToString(this object @this)
        {
            return Convert.ToString(@this);
        }

        public static string ToStringOrDefault(this object @this)
        {
            string str;
            try
            {
                str = Convert.ToString(@this);
            }
            catch (Exception exception)
            {
                str = null;
            }
            return str;
        }

        public static string ToStringOrDefault(this object @this, string defaultValue)
        {
            string str;
            try
            {
                str = Convert.ToString(@this);
            }
            catch (Exception exception)
            {
                str = defaultValue;
            }
            return str;
        }

        public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory)
        {
            string str;
            try
            {
                str = Convert.ToString(@this);
            }
            catch (Exception exception)
            {
                str = defaultValueFactory();
            }
            return str;
        }

        public static string ToStringSafe(this object @this)
        {
            if (@this == null)
            {
                return "";
            }
            return @this.ToString();
        }

        public static ushort ToUInt16(this object @this)
        {
            return Convert.ToUInt16(@this);
        }

        public static ushort ToUInt16OrDefault(this object @this)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static ushort ToUInt16OrDefault(this object @this, ushort defaultValue)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static ushort ToUInt16OrDefault(this object @this, Func<ushort> defaultValueFactory)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static uint ToUInt32(this object @this)
        {
            return Convert.ToUInt32(@this);
        }

        public static uint ToUInt32OrDefault(this object @this)
        {
            uint num;
            try
            {
                num = Convert.ToUInt32(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static uint ToUInt32OrDefault(this object @this, uint defaultValue)
        {
            uint num;
            try
            {
                num = Convert.ToUInt32(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static uint ToUInt32OrDefault(this object @this, Func<uint> defaultValueFactory)
        {
            uint num;
            try
            {
                num = Convert.ToUInt32(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static ulong ToUInt64(this object @this)
        {
            return Convert.ToUInt64(@this);
        }

        public static ulong ToUInt64OrDefault(this object @this)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = (ulong)0;
            }
            return num;
        }

        public static ulong ToUInt64OrDefault(this object @this, ulong defaultValue)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static ulong ToUInt64OrDefault(this object @this, Func<ulong> defaultValueFactory)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static ulong ToULong(this object @this)
        {
            return Convert.ToUInt64(@this);
        }

        public static ulong ToULongOrDefault(this object @this)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = (ulong)0;
            }
            return num;
        }

        public static ulong ToULongOrDefault(this object @this, ulong defaultValue)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static ulong ToULongOrDefault(this object @this, Func<ulong> defaultValueFactory)
        {
            ulong num;
            try
            {
                num = Convert.ToUInt64(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }

        public static ushort ToUShort(this object @this)
        {
            return Convert.ToUInt16(@this);
        }

        public static ushort ToUShortOrDefault(this object @this)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;
        }

        public static ushort ToUShortOrDefault(this object @this, ushort defaultValue)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValue;
            }
            return num;
        }

        public static ushort ToUShortOrDefault(this object @this, Func<ushort> defaultValueFactory)
        {
            ushort num;
            try
            {
                num = Convert.ToUInt16(@this);
            }
            catch (Exception exception)
            {
                num = defaultValueFactory();
            }
            return num;
        }
    }
}
