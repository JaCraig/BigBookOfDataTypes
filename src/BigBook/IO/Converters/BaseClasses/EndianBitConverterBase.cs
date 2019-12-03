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

using BigBook.IO.Converters.Structs;
using System;

namespace BigBook.IO.Converters.BaseClasses
{
    /// <summary>
    /// Endian bit converter base class
    /// </summary>
    public abstract class EndianBitConverterBase
    {
        /// <summary>
        /// Gets the big endian bit converter.
        /// </summary>
        /// <value>The big endian bit converter.</value>
        public static EndianBitConverterBase BigEndian { get; } = new BigEndianBitConverter();

        /// <summary>
        /// Gets the little endian bit converter.
        /// </summary>
        /// <value>The little endian bit converter.</value>
        public static EndianBitConverterBase LittleEndian { get; } = new LittleEndianBitConverter();

        /// <summary>
        /// Gets a value indicating whether this instance is little endian.
        /// </summary>
        /// <value><c>true</c> if this instance is little endian; otherwise, <c>false</c>.</value>
        public abstract bool IsLittleEndian { get; }

        /// <summary>
        /// Converts a double to a long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting long.</returns>
        public static long DoubleToLong(double value) => BitConverter.DoubleToInt64Bits(value);

        /// <summary>
        /// Converts a float to an integer
        /// </summary>
        /// <param name="value">Float value</param>
        /// <returns>The integer equivalent.</returns>
        public static int FloatToInt(float value) => new IntFloatUnion(value).IntegerValue;

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>Gets the resulting byte array.</returns>
        public static byte[] GetBytes(bool value) => BitConverter.GetBytes(value);

        /// <summary>
        /// Converts an integer to a float.
        /// </summary>
        /// <param name="value">The integer value</param>
        /// <returns>The float value.</returns>
        public static float IntToFloat(int value) => new IntFloatUnion(value).FloatValue;

        /// <summary>
        /// Converts a long to a double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting double.</returns>
        public static double LongToDouble(long value) => BitConverter.Int64BitsToDouble(value);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public static string ToString(byte[] value) => BitConverter.ToString(value);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public static string ToString(byte[] value, int startIndex) => BitConverter.ToString(value, startIndex);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public static string ToString(byte[] value, int startIndex, int length) => BitConverter.ToString(value, startIndex, length);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(decimal value, byte[] buffer, int index)
        {
            var parts = decimal.GetBits(value);
            for (var i = 0; i < 4; i++)
            {
                CopyBytesImpl(parts[i], 4, buffer, (i * 4) + index);
            }
        }

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(bool value, byte[] buffer, int index) => CopyBytes(value ? 1 : 0, 1, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(char value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(double value, byte[] buffer, int index) => CopyBytes(DoubleToLong(value), 8, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(short value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(int value, byte[] buffer, int index) => CopyBytes(value, 4, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(long value, byte[] buffer, int index) => CopyBytes(value, 8, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(float value, byte[] buffer, int index) => CopyBytes(FloatToInt(value), 4, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(ushort value, byte[] buffer, int index) => CopyBytes(value, 2, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(uint value, byte[] buffer, int index) => CopyBytes(value, 4, buffer, index);

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        public void CopyBytes(ulong value, byte[] buffer, int index) => CopyBytes(unchecked((long)value), 8, buffer, index);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(decimal value)
        {
            var bytes = new byte[16];
            var parts = decimal.GetBits(value);
            for (var i = 0; i < 4; i++)
            {
                CopyBytesImpl(parts[i], 4, bytes, i * 4);
            }

            return bytes;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(char value) => GetBytes(value, 2);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(double value) => GetBytes(DoubleToLong(value), 8);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(short value) => GetBytes(value, 2);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(int value) => GetBytes(value, 4);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(long value) => GetBytes(value, 8);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(float value) => GetBytes(FloatToInt(value), 4);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(ushort value) => GetBytes(value, 2);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(uint value) => GetBytes(value, 4);

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] GetBytes(ulong value) => GetBytes(unchecked((long)value), 8);

        /// <summary>
        /// To the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting boolean.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public bool ToBoolean(byte[] value, int startIndex)
        {
            value ??= Array.Empty<byte>();
            if (value.Length - 1 < startIndex || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            return BitConverter.ToBoolean(value, startIndex);
        }

        /// <summary>
        /// To the character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting character.</returns>
        public char ToChar(byte[] value, int startIndex) => unchecked((char)CheckedFromBytes(value, startIndex, 2));

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting decimal value</returns>
        public decimal ToDecimal(byte[] value, int startIndex)
        {
            var parts = new int[4];
            for (var i = 0; i < 4; i++)
            {
                parts[i] = ToInt(value, startIndex + (i * 4));
            }

            return new decimal(parts);
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting double.</returns>
        public double ToDouble(byte[] value, int startIndex) => LongToDouble(ToLong(value, startIndex));

        /// <summary>
        /// To the float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting float.</returns>
        public float ToFloat(byte[] value, int startIndex) => IntToFloat(ToInt(value, startIndex));

        /// <summary>
        /// Converts the string representation of a number to an integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting integer.</returns>
        public int ToInt(byte[] value, int startIndex) => unchecked((int)CheckedFromBytes(value, startIndex, 4));

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting long.</returns>
        public long ToLong(byte[] value, int startIndex) => CheckedFromBytes(value, startIndex, 8);

        /// <summary>
        /// To the short.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting short.</returns>
        public short ToShort(byte[] value, int startIndex) => unchecked((short)CheckedFromBytes(value, startIndex, 2));

        /// <summary>
        /// To the unsigned integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting unsigned integer</returns>
        public uint ToUnsignedInteger(byte[] value, int startIndex) => unchecked((uint)CheckedFromBytes(value, startIndex, 4));

        /// <summary>
        /// To the unsigned long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting unsigned long.</returns>
        public ulong ToUnsignedLong(byte[] value, int startIndex) => unchecked((ulong)CheckedFromBytes(value, startIndex, 8));

        /// <summary>
        /// To the unsigned short.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The resulting unsigned short.</returns>
        public ushort ToUnsignedShort(byte[] value, int startIndex) => unchecked((ushort)CheckedFromBytes(value, startIndex, 2));

        /// <summary>
        /// Copies the bytes implementation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected abstract void CopyBytesImpl(long value, int bytes, byte[] buffer, int index);

        /// <summary>
        /// Converts a byte array to a long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesToConvert">The bytes to convert.</param>
        /// <returns>The resulting long.</returns>
        protected abstract long FromBytes(byte[] value, int startIndex, int bytesToConvert);

        /// <summary>
        /// Checkeds from bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesToConvert">The bytes to convert.</param>
        /// <returns>The resulting long</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private long CheckedFromBytes(byte[] value, int startIndex, int bytesToConvert)
        {
            value ??= Array.Empty<byte>();
            if (value.Length - bytesToConvert < startIndex || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            return FromBytes(value, startIndex, bytesToConvert);
        }

        /// <summary>
        /// Copies the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void CopyBytes(long value, int bytes, byte[] buffer, int index)
        {
            buffer ??= Array.Empty<byte>();
            if (buffer.Length - bytes < index || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            CopyBytesImpl(value, bytes, buffer, index);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Gets the resulting byte array.</returns>
        private byte[] GetBytes(long value, int bytes)
        {
            var buffer = new byte[bytes];
            CopyBytes(value, bytes, buffer, 0);
            return buffer;
        }
    }
}