// Sourced from http://www.codeproject.com/KB/cs/stringenum.aspx

using System;
using System.Collections;
using System.ComponentModel;

namespace XeroIngCsvParser.Classes
{
    #region Class StringEnum

    /// <summary>
    /// Helper class for working with 'extended' enums using <see cref="DescriptionAttribute"/> attributes.
    /// </summary>
    public class StringEnum
    {
        #region Instance implementation

        private readonly Type _enumType;
        private static readonly Hashtable StringValues = new Hashtable();

        /// <summary>
        /// Creates a new <see cref="StringEnum"/> instance.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        public StringEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", enumType));

            _enumType = enumType;
        }

        /// <summary>
        /// Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public string GetStringValue(string valueName)
        {
            string stringValue = null;
            try
            {
                var enumType = (Enum)Enum.Parse(_enumType, valueName);
                stringValue = GetStringValue(enumType);
                // ReSharper disable EmptyGeneralCatchClause
            }
            catch (Exception) { }//Swallow!
            // ReSharper restore EmptyGeneralCatchClause

            return stringValue;
        }

        /// <summary>
        /// Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public Array GetStringValues()
        {
            var values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (var fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                // ReSharper disable PossibleNullReferenceException
                if (attrs.Length > 0)
                    // ReSharper restore PossibleNullReferenceException
                    values.Add(attrs[0].Description);

            }

            return values.ToArray();
        }

        /// <summary>
        /// Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public IList GetListValues()
        {
            var underlyingType = Enum.GetUnderlyingType(_enumType);
            var values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (var fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                // ReSharper disable PossibleNullReferenceException
                if (attrs.Length > 0)
                    // ReSharper restore PossibleNullReferenceException
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(_enumType, fi.Name), underlyingType), attrs[0].Description));

            }

            return values;

        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue)
        {
            return Parse(_enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue, bool ignoreCase)
        {
            return Parse(_enumType, stringValue, ignoreCase) != null;
        }

        /// <summary>
        /// Gets the underlying enum type for this instance.
        /// </summary>
        /// <value></value>
        public Type EnumType
        {
            get { return _enumType; }
        }

        #endregion

        #region Static implementation

        /// <summary>
        /// Gets a string value for a particular enum value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>String Value associated via a <see cref="DescriptionAttribute"/> attribute, or null if not found.</returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            var type = value.GetType();

            if (StringValues.ContainsKey(value))
                output = ((DescriptionAttribute)StringValues[value]).Description;
            else
            {
                var fi = type.GetField(value.ToString());
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                // ReSharper disable PossibleNullReferenceException
                if (attrs.Length > 0)
                {
                    // ReSharper restore PossibleNullReferenceException
                    if (!StringValues.Contains(value))
                    {
                        StringValues.Add(value, attrs[0]);
                    }
                    output = attrs[0].Description;
                }

            }
            return output;

        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type));

            //Look for our string value associated with fields in this enum
            foreach (var fi in type.GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                // ReSharper disable PossibleNullReferenceException
                if (attrs.Length > 0)
                    // ReSharper restore PossibleNullReferenceException
                    enumStringValue = attrs[0].Description;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return Parse(enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return Parse(enumType, stringValue, ignoreCase) != null;
        }

        #endregion
    }

    #endregion
}