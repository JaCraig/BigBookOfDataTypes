using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// String builder extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="builder">Builder object</param>
        /// <param name="provider">Format provider (CultureInfo) to use</param>
        /// <param name="format">Format string</param>
        /// <param name="objects">Objects to format</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat(this StringBuilder builder, IFormatProvider provider, string format, params object[] objects)
        {
            builder ??= new StringBuilder();
            if (string.IsNullOrEmpty(format))
            {
                return builder;
            }

            objects ??= Array.Empty<object>();
            provider ??= CultureInfo.InvariantCulture;
            return builder.AppendFormat(provider, format, objects).AppendLine();
        }

        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="builder">Builder object</param>
        /// <param name="format">Format string</param>
        /// <param name="objects">Objects to format</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] objects) => builder.AppendLineFormat(CultureInfo.InvariantCulture, format, objects);

        /// <summary>
        /// Trims the specified string.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="trimCharacters">The characters to trim.</param>
        /// <returns>The string minus the characters specified.</returns>
        public static StringBuilder? Trim(this StringBuilder? builder, params char[] trimCharacters)
        {
            if (builder is null)
                return builder;
            if (trimCharacters is null || trimCharacters.Length == 0)
                trimCharacters = new char[] { ' ', '\t', '\n' };
            return builder.TrimStart(trimCharacters).TrimEnd(trimCharacters);
        }

        /// <summary>
        /// Trims the end of the string.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="trimCharacters">The characters to trim.</param>
        /// <returns>The string builder minus the characters specified.</returns>
        public static StringBuilder? TrimEnd(this StringBuilder? builder, params char[] trimCharacters)
        {
            if (builder is null)
                return builder;
            if (trimCharacters is null || trimCharacters.Length == 0)
                trimCharacters = new char[] { ' ', '\t', '\n' };
            var x = builder.Length;
            for (; x > 0; --x)
            {
                if (!trimCharacters.Any(y => builder[x - 1] == y))
                    break;
            }
            if (x == builder.Length)
                return builder;
            builder.Remove(x, builder.Length - x);
            return builder;
        }

        /// <summary>
        /// Trims the start of the string.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="trimCharacters">The characters to trim.</param>
        /// <returns>The builder with the values trimmed.</returns>
        public static StringBuilder? TrimStart(this StringBuilder? builder, params char[] trimCharacters)
        {
            if (builder is null)
                return builder;
            if (trimCharacters is null || trimCharacters.Length == 0)
                trimCharacters = new char[] { ' ', '\t', '\n' };
            var x = 0;
            for (; x < builder.Length; ++x)
            {
                if (!trimCharacters.Any(y => builder[x] == y))
                    break;
            }
            if (x == 0)
                return builder;
            builder.Remove(0, x);
            return builder;
        }
    }
}