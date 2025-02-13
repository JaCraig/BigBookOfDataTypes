﻿/*
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

using BigBook;
using BigBook.ExtensionMethods;
using BigBook.Formatters;
using BigBook.Formatters.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BigBook.ExtensionMethods
{
    /// <summary>
    /// String extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        /// <summary>
        /// The HTML replacements
        /// </summary>
        private static readonly Dictionary<string, string> HtmlReplacements = new()
        {
            // convert br to new line
            { "br", "\n" },
            // convert p to new line
            { "p", "\n" },
            // convert div to new line
            { "div", "\n" },
            // convert li to *
            { "li", "* " },
            // convert ul to new line
            { "ul", "\n" },
            // convert ol to new line
            { "ol", "\n" }
        };

        /// <summary>
        /// The strip HTML regex
        /// </summary>
        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Gets the is unicode.
        /// </summary>
        /// <value>The is unicode.</value>
        private static Regex IsUnicode { get; } = new Regex(@"[^\u0000-\u007F]", RegexOptions.Compiled);

        /// <summary>
        /// Adds spaces to a string before capital letters. Acronyms are respected and left alone.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The resulting string.</returns>
        public static string AddSpaces(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var newText = new StringBuilder(input.Length * 2);
            _ = newText.Append(input[0]);
            for (int x = 1, inputLength = input.Length; x < inputLength; ++x)
            {
                if (char.IsUpper(input[x]))
                {
                    if ((input[x - 1] != ' ' && !char.IsUpper(input[x - 1]))
                        || (char.IsUpper(input[x - 1])
                            && x < input.Length - 1
                            && !(char.IsUpper(input[x + 1]) || input[x + 1] == ' ')))
                    {
                        _ = newText.Append(' ');
                    }
                }
                _ = newText.Append(input[x]);
            }
            return newText.ToString();
        }

        /// <summary>
        /// Centers the input string (if it's longer than the length) and pads it using the padding string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="padding"></param>
        /// <returns>The centered string</returns>
        public static string Center(this string? input, int length, string padding = " ")
        {
            input ??= "";
            padding ??= "";
            var Output = "";
            for (var x = 0; x < (length - input.Length) / 2; ++x)
            {
                Output += padding[x % padding.Length];
            }
            Output += input;
            for (var x = 0; x < (length - input.Length) / 2; ++x)
            {
                Output += padding[x % padding.Length];
            }
            return Output;
        }

        /// <summary>
        /// Converts a string to a string of another encoding
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="originalEncodingUsing">
        /// The type of encoding the string is currently using (defaults to UTF8)
        /// </param>
        /// <param name="encodingUsing">
        /// The type of encoding the string is converted into (defaults to UTF8)
        /// </param>
        /// <returns>string of the byte array</returns>
        public static string Encode(this string? input, Encoding? originalEncodingUsing = null, Encoding? encodingUsing = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            originalEncodingUsing ??= Encoding.UTF8;
            encodingUsing ??= Encoding.UTF8;
            return Encoding.Convert(originalEncodingUsing, encodingUsing, input.ToByteArray(originalEncodingUsing))
                           .ToString(encodingUsing);
        }

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="encodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string? input, Encoding encodingUsing)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var TempArray = Convert.FromBase64String(input);
            encodingUsing ??= Encoding.UTF8;
            return encodingUsing.GetString(TempArray, 0, TempArray.Length);
        }

        /// <summary>
        /// Converts base 64 string to a byte array
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>A byte array equivalent of the base 64 string</returns>
        public static byte[] FromBase64(this string? input) => string.IsNullOrEmpty(input) ? Array.Empty<byte>() : Convert.FromBase64String(input);

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="value">Value to compare</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string? value, StringCompare comparisonType)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            if (comparisonType == StringCompare.CreditCard)
            {
                long CheckSum = 0;
                value = value.Replace("-", "", StringComparison.Ordinal).Reverse();
                for (var x = 0; x < value.Length; ++x)
                {
                    if (!value[x].Is(CharIs.Digit))
                        return false;

                    var Tempvalue = (value[x] - '0') * (x % 2 == 1 ? 2 : 1);
                    while (Tempvalue > 0)
                    {
                        CheckSum += Tempvalue % 10;
                        Tempvalue /= 10;
                    }
                }
                return CheckSum % 10 == 0;
            }
            return comparisonType == StringCompare.Unicode
                ? string.IsNullOrEmpty(value) || IsUnicode.Replace(value, "") != value
                : value.Is("", StringCompare.Anagram);
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="value1">Value 1 to compare</param>
        /// <param name="value2">Value 2 to compare</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string? value1, string value2, StringCompare comparisonType)
        {
            return comparisonType != StringCompare.Anagram
                ? value1.Is(comparisonType)
                : new string(value1?.ToCharArray().OrderBy(x => x).ToArray()) == new string(value2?.ToCharArray().OrderBy(x => x).ToArray());
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string? input, string filter)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(filter))
                return string.Empty;

            var TempRegex = new Regex(filter);
            MatchCollection Collection = TempRegex.Matches(input);
            var Builder = new StringBuilder();
            for (int x = 0, CollectionCount = Collection.Count; x < CollectionCount; x++)
            {
                Match Match = Collection[x];
                _ = Builder.Append(Match.Value);
            }

            return Builder.ToString();
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string? input, StringFilter filter)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var Value = BuildFilter(filter);
            return input.Keep(Value);
        }

        /// <summary>
        /// Gets the first x number of characters from the left hand side
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Left(this string? input, int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }
            else if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            else
            {
                return input[..(input.Length > length ? length : input.Length)];
            }
        }

        /// <summary>
        /// Calculates the Levenshtein distance
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The Levenshtein distance (-1 if null values found)</returns>
        public static int LevenshteinDistance(this string? value1, string value2)
        {
            if (value1 is null || value2 is null)
                return -1;

            var Matrix = new int[value1.Length + 1, value2.Length + 1];
            for (var x = 0; x <= value1.Length; ++x)
            {
                Matrix[x, 0] = x;
            }

            for (var x = 0; x <= value2.Length; ++x)
            {
                Matrix[0, x] = x;
            }

            for (var x = 1; x <= value1.Length; ++x)
            {
                for (var y = 1; y <= value2.Length; ++y)
                {
                    var Cost = value1[x - 1] == value2[y - 1] ? 0 : 1;
                    Matrix[x, y] = new int[] { Matrix[x - 1, y] + 1, Matrix[x, y - 1] + 1, Matrix[x - 1, y - 1] + Cost }.Min();
                    if (x > 1 && y > 1 && value1[x - 1] == value2[y - 2] && value1[x - 2] == value2[y - 1])
                        Matrix[x, y] = new int[] { Matrix[x, y], Matrix[x - 2, y - 2] + Cost }.Min();
                }
            }
            return Matrix[value1.Length, value2.Length];
        }

        /// <summary>
        /// Masks characters to the left ending at a specific character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="endPosition">End position (counting from the left)</param>
        /// <param name="mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskLeft(this string? input, int endPosition = 4, char mask = '#')
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var Appending = "";
            for (var x = 0; x < endPosition; ++x)
            {
                Appending += mask;
            }

            return Appending + input.Remove(0, endPosition);
        }

        /// <summary>
        /// Masks characters to the right starting at a specific character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="startPosition">Start position (counting from the left)</param>
        /// <param name="mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskRight(this string? input, int startPosition = 4, char mask = '#')
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (startPosition > input.Length)
                return input;

            var Appending = "";
            for (var x = 0; x < input.Length - startPosition; ++x)
            {
                Appending += mask;
            }

            return input.Remove(startPosition) + Appending;
        }

        /// <summary>
        /// Minifies the file based on the data type specified
        /// </summary>
        /// <param name="Input">Input file</param>
        /// <param name="Type">Type of minification to run</param>
        /// <returns>A stripped file</returns>
        public static string Minify(this string? Input, MinificationType Type = MinificationType.HTML)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return string.Empty;
            }
            else if (Type == MinificationType.CSS)
            {
                return CSSMinify(Input);
            }
            else if (Type == MinificationType.JavaScript)
            {
                return JavaScriptMinify(Input);
            }
            else
            {
                return HTMLMinify(Input);
            }
        }

        /// <summary>
        /// returns the number of times a string occurs within the text
        /// </summary>
        /// <param name="input">input text</param>
        /// <param name="match">The string to match (can be regex)</param>
        /// <returns>The number of times the string occurs</returns>
        public static int NumberTimesOccurs(this string? input, string match) => string.IsNullOrEmpty(input) ? 0 : new Regex(match).Matches(input).Count;

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Regex expression of text to remove</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string? Remove(this string? input, string filter) => string.IsNullOrEmpty(input) || string.IsNullOrEmpty(filter) ? input : new Regex(filter).Replace(input, "");

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string? Remove(this string? input, StringFilter filter)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var Value = BuildFilter(filter);
            return input.Remove(Value);
        }

        /// <summary>
        /// Removes the diacritics from a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The resulting string with diacritics removed.</returns>
        public static string RemoveDiacritics(this string? input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty
                : new string(input
                .Normalize(NormalizationForm.FormD)
                .Where(x => CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark)
                .ToArray())
            .Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Replaces everything that is in the filter text with the value specified.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <param name="value">Value to fill in</param>
        /// <returns>The input text with the various items replaced</returns>
        public static string Replace(this string? input, StringFilter filter, string value = "")
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var FilterValue = BuildFilter(filter);
            return new Regex(FilterValue).Replace(input, value);
        }

        /// <summary>
        /// Replaces all keys in the replacements dictionary with the corresponding values.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>The resulting string.</returns>
        public static string ReplaceAll(this string? input, IEnumerable<KeyValuePair<string, string>>? replacements)
        {
            if (string.IsNullOrEmpty(input) || !(replacements?.Any() ?? false))
                return input ?? "";

            foreach (KeyValuePair<string, string> Item in replacements)
            {
                input = input.Replace(Item.Key, Item.Value);
            }
            return input;
        }

        /// <summary>
        /// Replaces all characters in the input array with the new character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="oldChars">The old characters.</param>
        /// <param name="newChar">The new character.</param>
        /// <returns>The resulting string.</returns>
        public static string ReplaceAll(this string? value, char[] oldChars, char newChar)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            ReadOnlySpan<char> ValueSpan = value.AsSpan();
            var Temp = new char[value.Length];
            value.CopyTo(0, Temp, 0, Temp.Length);
            Span<char> Result = Temp;
            Span<char> OldCharsSpan = (oldChars ?? Array.Empty<char>()).AsSpan();
            var CurrentIndex = 0;
            while (true)
            {
                var NextIndex = Result.IndexOfAny(OldCharsSpan);
                if (NextIndex == -1)
                    break;
                if (CurrentIndex != NextIndex)
                    ValueSpan[CurrentIndex..NextIndex].CopyTo(Result[CurrentIndex..NextIndex]);
                Result[NextIndex] = newChar;
            }
            return new string(Result);
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>The reverse of the input string</returns>
        public static string Reverse(this string? input) => string.IsNullOrEmpty(input) ? string.Empty : new string(input.ToCharArray().Reverse().ToArray());

        /// <summary>
        /// Gets the last x number of characters from the right hand side
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Right(this string? input, int length)
        {
            if (string.IsNullOrEmpty(input) || length <= 0)
                return string.Empty;

            length = input.Length > length ? length : input.Length;
            return input.Substring(input.Length - length, length);
        }

        /// <summary>
        /// Removes HTML elements from a string
        /// </summary>
        /// <param name="html">HTML laiden string</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML(this string? html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            foreach (KeyValuePair<string, string> Item in HtmlReplacements)
            {
                // Replace opening tags
                html = Regex.Replace(html, $@"<\s*{Item.Key}[^>]*>", Item.Value, RegexOptions.IgnoreCase);
                // Replace closing tags
                html = Regex.Replace(html, $@"<\s*/\s*{Item.Key}\s*>", "\n", RegexOptions.IgnoreCase);
            }
            // remove all other html tags
            html = STRIP_HTML_REGEX.Replace(html, " ");
            // replace &nbsp; with space
            html = html.Replace("&nbsp;", " ", StringComparison.OrdinalIgnoreCase);
            // replace &#160; with space
            html = html.Replace("&#160;", "", StringComparison.Ordinal);
            // replace all other html entities with their character
            html = WebUtility.HtmlDecode(html);
            // Convert tabs to spaces
            html = html.Replace("\t", " ", StringComparison.Ordinal);
            // replace multiple spaces with single space
            while (html.Contains("  "))
                html = html.Replace("  ", " ", StringComparison.Ordinal);
            // Normalize new lines
            html = html.Replace("\r", "\n", StringComparison.Ordinal);

            // Preserve double new lines for paragraphs
            html = Regex.Replace(html, @"\n\s*\n", "\n\n", RegexOptions.Multiline);

            // Trim spaces at the beginning and end of lines
            html = Regex.Replace(html, @"^[ ]+|[ ]+$", string.Empty, RegexOptions.Multiline);

            return html.Trim();
        }

        /// <summary>
        /// Strips illegal characters for XML content
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>The stripped string</returns>
        public static string StripIllegalXML(this string? content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var Builder = new StringBuilder();
            for (int x = 0, contentLength = content.Length; x < contentLength; x++)
            {
                var Char = content[x];
                if (Char is (char)0x9
                    or (char)0xA
                    or (char)0xD
                    or (>= (char)0x20 and <= (char)0xD7FF)
                    or (>= (char)0xE000 and <= (char)0xFFFD))
                {
                    _ = Builder.Append(Char);
                }
            }

            return Builder.ToString().ReplaceAll(new char[] { '\u2013', '\u2014', '\u2015' }, '-')
                .Replace('\u2017', '_')
                .ReplaceAll(new char[] { '\u2018', '\u2019', '\u201b', '\u2032', '`' }, '\'')
                .Replace('\u201a', ',')
                .ReplaceAll(new char[] { '\u201c', '\u201d', '\u201e', '\u2033' }, '\"')
                .Replace("\u2026", "...", StringComparison.Ordinal)
                .Replace("&", "&amp;", StringComparison.Ordinal).Replace("<", "&lt;", StringComparison.Ordinal)
                .Replace(">", "&gt;", StringComparison.Ordinal)
                .Replace("\"", "&quot;", StringComparison.Ordinal).Replace("\'", "&apos;", StringComparison.Ordinal);
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the left side of the input string
        /// (stops when a character not in the list is found)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string? StripLeft(this string? input, string characters = " ")
        {
            return string.IsNullOrEmpty(input) || string.IsNullOrEmpty(characters)
                ? input
                : input.ToCharArray().SkipWhile(x => characters.ToCharArray().Contains(x)).ToString(x => x.ToString(CultureInfo.InvariantCulture), "");
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the right side of the input
        /// string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string? StripRight(this string? input, string characters = " ")
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(characters))
                return input;

            var Position = input.Length - 1;
            for (var x = input.Length - 1; x >= 0; --x)
            {
                if (!characters.ToCharArray().Contains(input[x]))
                {
                    Position = x + 1;
                    break;
                }
            }
            return input.Left(Position);
        }

        /// <summary>
        /// Converts from the specified encoding to a base 64 string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="originalEncodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <returns>Bas64 string</returns>
        public static string ToBase64String(this string? input, Encoding? originalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            originalEncodingUsing ??= Encoding.UTF8;
            var TempArray = originalEncodingUsing.GetBytes(input);
            return Convert.ToBase64String(TempArray);
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="encodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>the byte array representing the string</returns>
        public static byte[] ToByteArray(this string? input, Encoding? encodingUsing = null)
        {
            encodingUsing ??= Encoding.UTF8;
            return string.IsNullOrEmpty(input) ? Array.Empty<byte>() : encodingUsing.GetBytes(input);
        }

        /// <summary>
        /// Formats the string based on the capitalization specified
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="caseOfString">Capitalization type to use</param>
        /// <param name="provider">CultureInfo to use. Defaults to InvariantCulture.</param>
        /// <returns>Capitalizes the string based on the case specified</returns>
        public static string ToString(this string? input, StringCase caseOfString, CultureInfo? provider = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            provider ??= CultureInfo.InvariantCulture;
            if (caseOfString == StringCase.FirstCharacterUpperCase)
            {
                var InputChars = input.ToCharArray();
                for (var x = 0; x < InputChars.Length; ++x)
                {
                    if (InputChars[x] is not ' ' and not '\t')
                    {
                        InputChars[x] = provider.TextInfo.ToUpper(InputChars[x]);
                        break;
                    }
                }
                return new string(InputChars);
            }
            if (caseOfString == StringCase.SentenceCapitalize)
            {
                string[] Seperator = { ".", "?", "!" };
                var InputStrings = input.Split(Seperator, StringSplitOptions.None);
                for (var x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x]))
                    {
                        var TempRegex = new Regex(InputStrings[x]);
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        input = TempRegex.Replace(input, InputStrings[x]);
                    }
                }
                return input;
            }
            if (caseOfString == StringCase.TitleCase)
            {
                string[] Seperator = { " ", ".", "\t", Environment.NewLine, "!", "?" };
                var InputStrings = input.Split(Seperator, StringSplitOptions.None);
                for (var x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x])
                        && InputStrings[x].Length > 3)
                    {
                        var TempRegex = new Regex(InputStrings[x]
                            .Replace(")", @"\)", StringComparison.Ordinal)
                            .Replace("(", @"\(", StringComparison.Ordinal)
                            .Replace("*", @"\*", StringComparison.Ordinal));
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        input = TempRegex.Replace(input, InputStrings[x]);
                    }
                }
                return input;
            }
            if (caseOfString == StringCase.CamelCase)
            {
                input = input.Replace(" ", "", StringComparison.Ordinal);
                return char.ToLower(input[0], CultureInfo.InvariantCulture) + input.Remove(0, 1);
            }
            return input;
        }

        /// <summary>
        /// Formats a string based on a format string passed in. The default formatter uses the
        /// following format: # = digits @ = alpha characters \ = escape char
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="format">Format of the output string</param>
        /// <param name="provider">String formatter provider (defaults to GenericStringFormatter)</param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string? input, string format, IStringFormatter? provider = null)
        {
            provider ??= new GenericStringFormatter();
            return provider.Format(input, format);
        }

        /// <summary>
        /// Formats a string based on the object's properties
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="inputObject">Object to use to format the string</param>
        /// <param name="startSeperator">
        /// Seperator character/string to use to describe the start of the property name
        /// </param>
        /// <param name="endSeperator">
        /// Seperator character/string to use to describe the end of the property name
        /// </param>
        /// <returns>The formatted string</returns>
        public static string? ToString(this string? input, object inputObject, string startSeperator = "{", string endSeperator = "}")
        {
            if (inputObject is null)
                return input;

            _ = inputObject.GetType()
                .GetProperties()
                .Where(x => x.CanRead)
                .ForEach(x =>
                {
                    var Value = x.GetValue(inputObject, null);
                    input = input?.Replace(startSeperator + x.Name + endSeperator, Value is null ? "" : Value.ToString(), StringComparison.Ordinal);
                });
            return input;
        }

        /// <summary>
        /// Formats a string based on the key/value pairs that are sent in
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="pairs">Key/value pairs. Replaces the key with the corresponding value.</param>
        /// <returns>The string after the changes have been made</returns>
        public static string? ToString(this string? input, params KeyValuePair<string, string>[] pairs)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            foreach (KeyValuePair<string, string> Pair in pairs)
            {
                input = input.Replace(Pair.Key, Pair.Value, StringComparison.Ordinal);
            }
            return input;
        }

        /// <summary>
        /// Uses a regex to format the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="format">Regex string used to</param>
        /// <param name="outputFormat">Output format</param>
        /// <param name="options">Regex options</param>
        /// <returns>The input string formatted by using the regex string</returns>
        public static string ToString(this string? input, string format, string outputFormat, RegexOptions options = RegexOptions.None)
        {
            return string.IsNullOrEmpty(input) || string.IsNullOrEmpty(format) || string.IsNullOrEmpty(outputFormat)
                ? string.Empty
                : Regex.Replace(input, format, outputFormat, options);
        }

        /// <summary>
        /// Builds the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private static string BuildFilter(StringFilter filter)
        {
            var FilterValue = "";
            var Separator = "";
            if ((filter & StringFilter.Alpha) != 0)
            {
                FilterValue += Separator + "[a-zA-Z]";
                Separator = "|";
            }
            if ((filter & StringFilter.Numeric) != 0)
            {
                FilterValue += Separator + "[0-9]";
                Separator = "|";
            }
            if ((filter & StringFilter.FloatNumeric) != 0)
            {
                FilterValue += Separator + @"[0-9\.]";
                Separator = "|";
            }
            if ((filter & StringFilter.ExtraSpaces) != 0)
                FilterValue += Separator + "[ ]{2,}";
            return FilterValue;
        }

        /// <summary>
        /// Minifies CSS
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The resulting value.</returns>
        private static string CSSMinify(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = Regex.Replace(input, @"(/\*\*/)|(/\*[^!][\s\S]*?\*/)", string.Empty);
            input = Regex.Replace(input, @"\s+", " ");
            input = Regex.Replace(input, @"(\s([\{:,;\}\(\)]))", "$2");
            input = Regex.Replace(input, @"(([\{:,;\}\(\)])\s)", "$2");
            input = Regex.Replace(input, ":0 0 0 0;", ":0;");
            input = Regex.Replace(input, ":0 0 0;", ":0;");
            input = Regex.Replace(input, ":0 0;", ":0;");
            input = Regex.Replace(input, ";}", "}");
            input = Regex.Replace(input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&nbsp;)\s{2,}(?=[<])", string.Empty);
            input = Regex.Replace(input, @"([!{}:;>+([,])\s+", "$1");
            input = Regex.Replace(input, @"([\s:])(0)(px|em|%|in|cm|mm|pc|pt|ex)", "$1$2");
            input = Regex.Replace(input, "background-position:0", "background-position:0 0");
            input = Regex.Replace(input, @"(:|\s)0+\.(\d+)", "$1.$2");
            return Regex.Replace(input, @"[^\}]+\{;\}", "");
        }

        /// <summary>
        /// Evaluates the specified matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        /// <returns>The resulting value.</returns>
        private static string Evaluate(Match matcher)
        {
            if (matcher is null)
                return string.Empty;

            var MyString = matcher.ToString();
            return string.IsNullOrEmpty(MyString) ? string.Empty : Regex.Replace(MyString, @"[\r\n\s]*", "");
        }

        /// <summary>
        /// Minifies HTML
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The resulting value</returns>
        private static string HTMLMinify(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = Regex.Replace(input, "/// <.+>", "");
            return string.IsNullOrEmpty(input) ? string.Empty : Regex.Replace(input, @">[\s\S]*?<", new MatchEvaluator(Evaluate));
        }

        /// <summary>
        /// Minifies javascript.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The resulting value.</returns>
        private static string JavaScriptMinify(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var CodeLines = input.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var Builder = new StringBuilder();
            for (int x = 0, CodeLinesLength = CodeLines.Length; x < CodeLinesLength; x++)
            {
                var Line = CodeLines[x];
                var Temp = Line.Trim();
                if (Temp.Length > 0 && !Temp.StartsWith("//", StringComparison.Ordinal))
                    _ = Builder.AppendLine(Temp);
            }

            input = Builder.ToString();
            input = Regex.Replace(input, @"(/\*\*/)|(/\*[^!][\s\S]*?\*/)", string.Empty);
            input = Regex.Replace(input, @"^[\s]+|[ \f\r\t\v]+$", string.Empty);
            input = Regex.Replace(input, @"^[\s]+|[ \f\r\t\v]+$", string.Empty);
            input = Regex.Replace(input, @"([+-])\n\1", "$1 $1");
            input = Regex.Replace(input, @"([^+-][+-])\n", "$1");
            input = Regex.Replace(input, @"([^+]) ?(\+)", "$1$2");
            input = Regex.Replace(input, @"(\+) ?([^+])", "$1$2");
            input = Regex.Replace(input, @"([^-]) ?(\-)", "$1$2");
            input = Regex.Replace(input, @"(\-) ?([^-])", "$1$2");
            input = Regex.Replace(input, @"\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1");
            input = Regex.Replace(input, @"(\W(if|while|for)\([^{]*?\))\n", "$1");
            input = Regex.Replace(input, @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3");
            input = Regex.Replace(input, @"([;}]else)\n", "$1 ");
            return Regex.Replace(input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&nbsp;)\s{2,}(?=[<])", string.Empty);
        }
    }

    /// <summary>
    /// Minification type
    /// </summary>
    public enum MinificationType
    {
        /// <summary>
        /// The HTML
        /// </summary>
        HTML = 1,

        /// <summary>
        /// The java script
        /// </summary>
        JavaScript = 2,

        /// <summary>
        /// The CSS
        /// </summary>
        CSS = 3
    }

    /// <summary>
    /// What sort of string capitalization should be used?
    /// </summary>
    public enum StringCase
    {
        /// <summary>
        /// Sentence capitalization
        /// </summary>
        SentenceCapitalize,

        /// <summary>
        /// First character upper case
        /// </summary>
        FirstCharacterUpperCase,

        /// <summary>
        /// Title case
        /// </summary>
        TitleCase,

        /// <summary>
        /// Camel case
        /// </summary>
        CamelCase
    }

    /// <summary>
    /// What type of string comparison are we doing?
    /// </summary>
    public enum StringCompare
    {
        /// <summary>
        /// Is this a credit card number?
        /// </summary>
        CreditCard,

        /// <summary>
        /// Is this an anagram?
        /// </summary>
        Anagram,

        /// <summary>
        /// Is this Unicode
        /// </summary>
        Unicode
    }

    /// <summary>
    /// Predefined filters
    /// </summary>
    [Flags]
    public enum StringFilter
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// Alpha characters
        /// </summary>
        Alpha = 1,

        /// <summary>
        /// Numeric characters
        /// </summary>
        Numeric = 2,

        /// <summary>
        /// Numbers with period, basically allows for decimal point
        /// </summary>
        FloatNumeric = 4,

        /// <summary>
        /// Multiple spaces
        /// </summary>
        ExtraSpaces = 8
    }
}