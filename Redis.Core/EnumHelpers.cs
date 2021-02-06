using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Redis.Core
{
        public static class EnumHelpers
        {
            /// <summary>
            /// Retrieve the description on the enum, e.g.
            /// [Description("Bright Pink")]
            /// BrightPink = 2,
            /// Then when you pass in the enum, it will retrieve the description
            /// </summary>
            /// <param name="en">The Enumeration</param>
            /// <returns>A string representing the friendly name</returns>
            /// <example>EnumHelper.GetDescription(UserColours.BrightPink);
            /// UserColours.BrightPink.GetDescription(); </example>
            public static string GetDescription(this Enum en)
            {
                Type type = en.GetType();

                MemberInfo[] memInfo = type.GetMember(en.ToString());

                if (memInfo != null && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }

                return en.ToString();
            }
            public static T GetValueFromDescription<T>(string description) where T : Enum
            {
                foreach (var field in typeof(T).GetFields())
                {
                    if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                    {
                        //if (//(attribute.Description == description))
                        if (string.Equals(attribute.Description, description, StringComparison.CurrentCultureIgnoreCase))
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                            return (T)field.GetValue(null);
                    }
                }

                throw new ArgumentException("Not found.", nameof(description));
                // Or return default(T);
            }

            public static string GetEnumDescription<TEnum>(TEnum value)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if ((attributes != null) && (attributes.Length > 0))
                    return attributes[0].Description;
                else
                    return value.ToString();
            }

            public static IEnumerable<T> GetValues<T>()
            {
                return Enum.GetValues(typeof(T)).Cast<T>();
            }



            #region Parse
            /// <summary>
            /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
            /// </summary>
            public static object Parse(Type type, string stringValue)
            {
                return Parse(type, stringValue, false);
            }

            /// <summary>
            /// Parses the supplied enum and string value to find an associated enum value.
            /// </summary>
            public static object Parse(Type type, string stringValue, bool ignoreCase)
            {
                object output = null;
                string enumStringValue = null;

                if (!type.IsEnum)
                {
                    throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type));
                }

                //Look for our string value associated with fields in this enum
                foreach (FieldInfo fi in type.GetFields())
                {
                    //Check for our custom attribute
                    var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs != null && attrs.Length > 0)
                    {
                        enumStringValue = attrs[0].Value;
                    }

                    //Check for equality then select actual enum value.
                    if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                    {
                        output = Enum.Parse(type, fi.Name);
                        break;
                    }
                }

                return output;
            }
            #endregion

        }
}
