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

using System;
using System.ComponentModel;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// What type of character is this
    /// </summary>
    [Flags]
    public enum CharIs
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// White space
        /// </summary>
        WhiteSpace = 1,

        /// <summary>
        /// Upper case
        /// </summary>
        Upper = 2,

        /// <summary>
        /// Symbol
        /// </summary>
        Symbol = 4,

        /// <summary>
        /// Surrogate
        /// </summary>
        Surrogate = 8,

        /// <summary>
        /// Punctuation
        /// </summary>
        Punctuation = 16,

        /// <summary>
        /// Number
        /// </summary>
        Number = 32,

        /// <summary>
        /// Low surrogate
        /// </summary>
        LowSurrogate = 64,

        /// <summary>
        /// Lower
        /// </summary>
        Lower = 128,

        /// <summary>
        /// letter or digit
        /// </summary>
        LetterOrDigit = 256,

        /// <summary>
        /// Letter
        /// </summary>
        Letter = 512,

        /// <summary>
        /// High surrogate
        /// </summary>
        HighSurrogate = 1024,

        /// <summary>
        /// Digit
        /// </summary>
        Digit = 2048,

        /// <summary>
        /// Control
        /// </summary>
        Control = 4096
    }

    /// <summary>
    /// Value type extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ValueTypeExtensions
    {
        /// <summary>
        /// Is the character of a specific type
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="characterType">Character type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this char value, CharIs characterType)
        {
            if ((characterType & CharIs.WhiteSpace) != 0)
            {
                return char.IsWhiteSpace(value);
            }

            if ((characterType & CharIs.Upper) != 0)
            {
                return char.IsUpper(value);
            }

            if ((characterType & CharIs.Symbol) != 0)
            {
                return char.IsSymbol(value);
            }

            if ((characterType & CharIs.Surrogate) != 0)
            {
                return char.IsSurrogate(value);
            }

            if ((characterType & CharIs.Punctuation) != 0)
            {
                return char.IsPunctuation(value);
            }

            if ((characterType & CharIs.Number) != 0)
            {
                return char.IsNumber(value);
            }

            if ((characterType & CharIs.LowSurrogate) != 0)
            {
                return char.IsLowSurrogate(value);
            }

            if ((characterType & CharIs.Lower) != 0)
            {
                return char.IsLower(value);
            }

            if ((characterType & CharIs.LetterOrDigit) != 0)
            {
                return char.IsLetterOrDigit(value);
            }

            if ((characterType & CharIs.Letter) != 0)
            {
                return char.IsLetter(value);
            }

            if ((characterType & CharIs.HighSurrogate) != 0)
            {
                return char.IsHighSurrogate(value);
            }

            if ((characterType & CharIs.Digit) != 0)
            {
                return char.IsDigit(value);
            }

            if ((characterType & CharIs.Control) != 0)
            {
                return char.IsControl(value);
            }

            return false;
        }

        /// <summary>
        /// Determines if a byte array is unicode
        /// </summary>
        /// <param name="input">Input array</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this byte[] input) => input?.ToString(Encoding.Unicode).Is(StringCompare.Unicode) != false;

        /// <summary>
        /// Converts a byte array into a base 64 string
        /// </summary>
        /// <param name="input">Input array</param>
        /// <param name="index">Index to start at</param>
        /// <param name="count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting at
        /// the index)
        /// </param>
        /// <returns>The equivalent byte array in a base 64 string</returns>
        public static string ToString(this byte[] input, int index = 0, int count = -1)
        {
            if (input == null)
            {
                return "";
            }

            if (index < 0)
            {
                index = 0;
            }

            if (index > input.Length - 1)
            {
                index = input.Length - 1;
            }

            if (count < 0)
            {
                count = input.Length - index;
            }

            return Convert.ToBase64String(input, index, count);
        }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="input">input array</param>
        /// <param name="encodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <param name="index">Index to start at</param>
        /// <param name="count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting at
        /// the index)
        /// </param>
        /// <returns>string of the byte array</returns>
        public static string ToString(this byte[] input, Encoding encodingUsing, int index = 0, int count = -1)
        {
            if (input == null)
            {
                return "";
            }

            if (count == -1)
            {
                count = input.Length - index;
            }

            encodingUsing = encodingUsing ?? Encoding.UTF8;
            return encodingUsing.GetString(input, index, count);
        }
    }
}