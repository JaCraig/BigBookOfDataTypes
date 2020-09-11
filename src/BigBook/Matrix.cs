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
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Matrix used in linear algebra
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width of the matrix</param>
        /// <param name="height">Height of the matrix</param>
        /// <param name="values">Values to use in the matrix</param>
        public Matrix(int width, int height, double[,]? values = null)
        {
            Width = width > -1 ? width : 0;
            Height = height > -1 ? height : 0;
            Values = values ?? new double[width, height];
        }

        /// <summary>
        /// Height of the matrix
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Values for the matrix
        /// </summary>
        public double[,] Values { get; }

        /// <summary>
        /// Width of the matrix
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Sets the values of the matrix
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>the value at a point in the matrix</returns>
        public double this[int x, int y]
        {
            get
            {
                x = x.Clamp(Width - 1, 0);
                y = y.Clamp(Height - 1, 0);

                return Values[x, y];
            }

            set
            {
                x = x.Clamp(Width - 1, 0);
                y = y.Clamp(Height - 1, 0);

                Values[x, y] = value;
            }
        }

        /// <summary>
        /// Adds the specified values.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The results</returns>
        public static Matrix Add(Matrix left, Matrix right) => left + right;

        /// <summary>
        /// Divides the specified values.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result</returns>
        public static Matrix Divide(Matrix left, double right) => left / right;

        /// <summary>
        /// Multiplies the specified values.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The results</returns>
        public static Matrix Multiply(Matrix left, Matrix right) => left * right;

        /// <summary>
        /// Negates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The result</returns>
        public static Matrix Negate(Matrix item) => -item;

        /// <summary>
        /// Subtracts two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            m1 ??= new Matrix(0, 0);
            m2 ??= new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
            {
                throw new ArgumentException(Properties.Resources.MatrixNotSameSizeError);
            }

            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (var x = 0; x < m1.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = m1[x, y] - m2[x, y];
                }
            }

            return TempMatrix;
        }

        /// <summary>
        /// Negates a matrix
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix m1)
        {
            m1 ??= new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (var x = 0; x < m1.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = -m1[x, y];
                }
            }

            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are unequal
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(Matrix? m1, Matrix? m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            m1 ??= new Matrix(0, 0);
            m2 ??= new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
            {
                throw new ArgumentException(Properties.Resources.MatrixNotSameSizeError);
            }

            var TempMatrix = new Matrix(m2.Width, m1.Height);
            for (var x = 0; x < m2.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = 0.0;
                    for (var i = 0; i < m1.Width; ++i)
                    {
                        for (var j = 0; j < m2.Height; ++j)
                        {
                            TempMatrix[x, y] += (m1[i, y] * m2[x, j]);
                        }
                    }
                }
            }
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix m1, double d)
        {
            m1 ??= new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (var x = 0; x < m1.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = m1[x, y] * d;
                }
            }

            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="d">Value to multiply by</param>
        /// <param name="m1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator *(double d, Matrix m1)
        {
            m1 ??= new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (var x = 0; x < m1.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = m1[x, y] * d;
                }
            }

            return TempMatrix;
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(Matrix m1, double d)
        {
            m1 ??= new Matrix(0, 0);
            return m1 * (1 / d);
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="d">Value to divide by</param>
        /// <param name="m1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator /(double d, Matrix m1)
        {
            m1 ??= new Matrix(0, 0);
            return m1 * (1 / d);
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            m1 ??= new Matrix(0, 0);
            m2 ??= new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
            {
                throw new ArgumentException(Properties.Resources.MatrixNotSameSizeError);
            }

            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (var x = 0; x < m1.Width; ++x)
            {
                for (var y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = m1[x, y] + m2[x, y];
                }
            }

            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are equal
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(Matrix? m1, Matrix? m2)
        {
            if (m1 is null && m2 is null)
            {
                return true;
            }

            if (m1 is null)
            {
                return false;
            }

            if (m2 is null)
            {
                return false;
            }

            if (m1.Width != m2.Width || m1.Height != m2.Height)
            {
                return false;
            }

            for (var x = 0; x <= m1.Width; ++x)
            {
                for (var y = 0; y <= m1.Height; ++y)
                {
                    if (m1[x, y] != m2[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Subtracts the specified values.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The results</returns>
        public static Matrix Subtract(Matrix left, Matrix right) => left - right;

        /// <summary>
        /// Gets the determinant of a square matrix
        /// </summary>
        /// <returns>The determinant of a square matrix</returns>
        public double Determinant()
        {
            if (Width != Height)
            {
                throw new InvalidOperationException(Properties.Resources.MatrixNotSquareError);
            }

            if (Width == 2)
            {
                return (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
            }

            var Answer = 0.0;
            for (var x = 0; x < Width; ++x)
            {
                var TempMatrix = new Matrix(Width - 1, Height - 1);
                var WidthCounter = 0;
                for (var y = 0; y < Width; ++y)
                {
                    if (y != x)
                    {
                        for (var z = 1; z < Height; ++z)
                        {
                            TempMatrix[WidthCounter, z - 1] = this[y, z];
                        }

                        ++WidthCounter;
                    }
                }
                if (x % 2 == 0)
                {
                    Answer += TempMatrix.Determinant();
                }
                else
                {
                    Answer -= TempMatrix.Determinant();
                }
            }
            return Answer;
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj) => obj is Matrix Tempobj && this == Tempobj;

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            double Hash = 0;
            for (var x = 0; x < Width; ++x)
            {
                for (var y = 0; y < Height; ++y)
                {
                    Hash += this[x, y];
                }
            }

            return (int)Hash;
        }

        /// <summary>
        /// Gets the string representation of the matrix
        /// </summary>
        /// <returns>The matrix as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            var Seperator = "";
            Builder.Append('{').Append(Environment.NewLine);
            for (var x = 0; x < Width; ++x)
            {
                Builder.Append('{');
                for (var y = 0; y < Height; ++y)
                {
                    Builder.Append(Seperator).Append(this[x, y]);
                    Seperator = ",";
                }
                Builder.Append('}').Append(Environment.NewLine);
                Seperator = "";
            }
            Builder.Append('}');
            return Builder.ToString();
        }

        /// <summary>
        /// Transposes the matrix
        /// </summary>
        /// <returns>Returns a new transposed matrix</returns>
        public Matrix Transpose()
        {
            var TempValues = new Matrix(Height, Width);
            for (var x = 0; x < Width; ++x)
            {
                for (var y = 0; y < Height; ++y)
                {
                    TempValues[y, x] = Values[x, y];
                }
            }

            return TempValues;
        }
    }
}