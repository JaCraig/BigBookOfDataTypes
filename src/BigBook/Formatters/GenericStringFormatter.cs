/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using BigBook.Formatters.Interfaces;
using System;
using System.Text;

namespace BigBook.Formatters
{
    /// <summary>
    /// Generic string formatter
    /// </summary>
    public class GenericStringFormatter : IFormatProvider, ICustomFormatter, IStringFormatter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStringFormatter()
            : this('#', '@', '\\')
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericStringFormatter"/> class.
        /// </summary>
        /// <param name="digitChar">The digit character.</param>
        /// <param name="alphaChar">The alpha character.</param>
        /// <param name="escapeChar">The escape character.</param>
        public GenericStringFormatter(char digitChar, char alphaChar, char escapeChar)
        {
            DigitChar = digitChar;
            AlphaChar = alphaChar;
            EscapeChar = escapeChar;
        }

        /// <summary>
        /// Represents alpha characters (defaults to @)
        /// </summary>
        public char AlphaChar { get; protected set; }

        /// <summary>
        /// Represents digits (defaults to #)
        /// </summary>
        public char DigitChar { get; protected set; }

        /// <summary>
        /// Represents the escape character (defaults to \)
        /// </summary>
        public char EscapeChar { get; protected set; }

        /// <summary>
        /// Formats the string
        /// </summary>
        /// <param name="format">Format to use</param>
        /// <param name="arg">Argument object to use</param>
        /// <param name="formatProvider">Format provider to use</param>
        /// <returns>The formatted string</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider) => Format(arg.ToString(), format);

        /// <summary>
        /// Formats the string based on the pattern
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="formatPattern">Format pattern</param>
        /// <returns>The formatted string</returns>
        public string Format(string input, string formatPattern)
        {
            if (!IsValid(formatPattern))
            {
                throw new ArgumentException("FormatPattern is not valid");
            }

            var ReturnValue = new StringBuilder();
            for (var x = 0; x < formatPattern.Length; ++x)
            {
                if (formatPattern[x] == EscapeChar)
                {
                    ++x;
                    ReturnValue.Append(formatPattern[x]);
                }
                else
                {
                    var NextValue = char.MinValue;
                    input = GetMatchingInput(input, formatPattern[x], out NextValue);
                    if (NextValue != char.MinValue)
                    {
                        ReturnValue.Append(NextValue);
                    }
                }
            }
            return ReturnValue.ToString();
        }

        /// <summary>
        /// Gets the format associated with the type
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>The appropriate formatter based on the type</returns>
        public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

        /// <summary>
        /// Gets matching input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="formatChar">Current format character</param>
        /// <param name="matchChar">The matching character found</param>
        /// <returns>The remainder of the input string left</returns>
        protected string GetMatchingInput(string input, char formatChar, out char matchChar)
        {
            var Digit = formatChar == DigitChar;
            var Alpha = formatChar == AlphaChar;
            if (!Digit && !Alpha)
            {
                matchChar = formatChar;
                return input;
            }
            var Index = 0;
            matchChar = char.MinValue;
            for (var x = 0; x < input.Length; ++x)
            {
                if ((Digit && char.IsDigit(input[x])) || (Alpha && char.IsLetter(input[x])))
                {
                    matchChar = input[x];
                    Index = x + 1;
                    break;
                }
            }
            return input.Substring(Index);
        }

        /// <summary>
        /// Checks if the format pattern is valid
        /// </summary>
        /// <param name="formatPattern">Format pattern</param>
        /// <returns>Returns true if it's valid, otherwise false</returns>
        protected bool IsValid(string formatPattern)
        {
            if (string.IsNullOrEmpty(formatPattern))
            {
                return false;
            }

            var EscapeCharFound = false;
            for (var x = 0; x < formatPattern.Length; ++x)
            {
                if (EscapeCharFound && formatPattern[x] != DigitChar
                        && formatPattern[x] != AlphaChar
                        && formatPattern[x] != EscapeChar)
                {
                    return false;
                }

                if (EscapeCharFound)
                {
                    EscapeCharFound = false;
                }
                else
                {
                    EscapeCharFound |= formatPattern[x] == EscapeChar;
                }
            }
            return !EscapeCharFound;
        }
    }
}