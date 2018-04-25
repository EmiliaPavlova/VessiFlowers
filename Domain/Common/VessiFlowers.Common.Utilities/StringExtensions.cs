namespace VessiFlowers.Common.Utilities
{
    /// <summary>
    /// Provides utility methods for converting string values to other data types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }

        /// <summary>
        /// Fix the string to passed lenght ending with ...
        /// </summary>
        /// <param name="str">The main string</param>
        /// <param name="lenght">The wanted lenght</param>
        /// <returns>The processed string</returns>
        public static string ToFixedLenght(this string str, int lenght)
        {
            if (str.Length > lenght + 3)
            {
                return str.Substring(0, lenght - 3) + "...";
            }

            return str;
        }
    }
}