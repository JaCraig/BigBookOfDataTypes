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
using System.Globalization;

namespace BigBook
{
    /// <summary>
    /// Represents a fraction
    /// </summary>
    public class Fraction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        public Fraction(double numerator, double denominator)
        {
            while (Math.Abs(numerator - Math.Round(numerator, MidpointRounding.AwayFromZero)) > EPSILON
                || Math.Abs(denominator - Math.Round(denominator, MidpointRounding.AwayFromZero)) > EPSILON)
            {
                numerator *= 10;
                denominator *= 10;
            }
            Numerator = (int)numerator;
            Denominator = (int)denominator;
            if (Denominator == int.MinValue)
            {
                return;
            }

            Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        public Fraction(decimal numerator, decimal denominator)
        {
            while (numerator != Math.Round(numerator, MidpointRounding.AwayFromZero)
                || denominator != Math.Round(denominator, MidpointRounding.AwayFromZero))
            {
                numerator *= 10;
                denominator *= 10;
            }
            Numerator = (int)numerator;
            Denominator = (int)denominator;
            Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        /// <exception cref="ArgumentException">denominator</exception>
        public Fraction(float numerator, float denominator)
        {
            if (Math.Abs(denominator - int.MinValue) < EPSILON)
            {
                throw new ArgumentException(nameof(denominator) + " can't be int.MinValue");
            }

            while (Math.Abs(numerator - Math.Round(numerator, MidpointRounding.AwayFromZero)) > EPSILON
                || Math.Abs(denominator - Math.Round(denominator, MidpointRounding.AwayFromZero)) > EPSILON)
            {
                numerator *= 10;
                denominator *= 10;
            }
            Numerator = (int)numerator;
            Denominator = (int)denominator;
            if (Denominator == int.MinValue)
            {
                return;
            }

            Reduce();
        }

        /// <summary>
        /// Denominator of the fraction
        /// </summary>
        public int Denominator { get; set; }

        /// <summary>
        /// Numerator of the faction
        /// </summary>
        public int Numerator { get; set; }

        /// <summary>
        /// Gets the epsilon.
        /// </summary>
        /// <value>The epsilon.</value>
        private double EPSILON { get; } = 0.001d;

        /// <summary>
        /// Converts the fraction to a decimal
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The fraction as a decimal</returns>
        public static implicit operator decimal(Fraction fraction)
        {
            if (fraction == null!)
            {
                throw new ArgumentNullException(nameof(fraction));
            }

            return fraction.Numerator / (decimal)fraction.Denominator;
        }

        /// <summary>
        /// Converts the fraction to a double
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The fraction as a double</returns>
        public static implicit operator double(Fraction fraction)
        {
            if (fraction == null!)
            {
                throw new ArgumentNullException(nameof(fraction));
            }

            return fraction.Numerator / (double)fraction.Denominator;
        }

        /// <summary>
        /// Converts the fraction to a float
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The fraction as a float</returns>
        public static implicit operator float(Fraction fraction)
        {
            if (fraction == null!)
            {
                throw new ArgumentNullException(nameof(fraction));
            }

            return fraction.Numerator / (float)fraction.Denominator;
        }

        /// <summary>
        /// Converts the double to a fraction
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The double as a fraction</returns>
        public static implicit operator Fraction(double fraction)
        {
            return new Fraction(fraction, 1.0);
        }

        /// <summary>
        /// Converts the decimal to a fraction
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The decimal as a fraction</returns>
        public static implicit operator Fraction(decimal fraction)
        {
            return new Fraction(fraction, 1.0m);
        }

        /// <summary>
        /// Converts the float to a fraction
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The float as a fraction</returns>
        public static implicit operator Fraction(float fraction)
        {
            return new Fraction(fraction, 1.0);
        }

        /// <summary>
        /// Converts the int to a fraction
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The int as a fraction</returns>
        public static implicit operator Fraction(int fraction)
        {
            return new Fraction(fraction, 1);
        }

        /// <summary>
        /// Converts the uint to a fraction
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The uint as a fraction</returns>
        public static implicit operator Fraction(uint fraction)
        {
            return new Fraction((int)fraction, 1);
        }

        /// <summary>
        /// Converts the fraction to a string
        /// </summary>
        /// <param name="fraction">Fraction</param>
        /// <returns>The fraction as a string</returns>
        public static implicit operator string(Fraction fraction)
        {
            if (fraction == null!)
            {
                return "";
            }

            return fraction.ToString();
        }

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="first">First fraction</param>
        /// <param name="second">Second fraction</param>
        /// <returns>The subtracted fraction</returns>
        public static Fraction operator -(Fraction first, Fraction second)
        {
            if (first == null!)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null!)
            {
                throw new ArgumentNullException(nameof(second));
            }

            var Value1 = new Fraction(first.Numerator * second.Denominator, first.Denominator * second.Denominator);
            var Value2 = new Fraction(second.Numerator * first.Denominator, second.Denominator * first.Denominator);
            var Result = new Fraction(Value1.Numerator - Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Negation of the fraction
        /// </summary>
        /// <param name="first">Fraction to negate</param>
        /// <returns>The negated fraction</returns>
        public static Fraction operator -(Fraction first)
        {
            if (first == null!)
            {
                throw new ArgumentNullException(nameof(first));
            }

            return new Fraction(-first.Numerator, first.Denominator);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction first, Fraction second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction first, double second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(double first, Fraction second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="first">First fraction</param>
        /// <param name="second">Second fraction</param>
        /// <returns>The resulting fraction</returns>
        public static Fraction operator *(Fraction first, Fraction second)
        {
            if (first == null!)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null!)
            {
                throw new ArgumentNullException(nameof(second));
            }

            var Result = new Fraction(first.Numerator * second.Numerator, first.Denominator * second.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>The divided fraction</returns>
        public static Fraction operator /(Fraction first, Fraction second)
        {
            if (first == null!)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null!)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first * second.Inverse();
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="first">First fraction</param>
        /// <param name="second">Second fraction</param>
        /// <returns>The added fraction</returns>
        public static Fraction operator +(Fraction first, Fraction second)
        {
            if (first == null!)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null!)
            {
                throw new ArgumentNullException(nameof(second));
            }

            var Value1 = new Fraction(first.Numerator * second.Denominator, first.Denominator * second.Denominator);
            var Value2 = new Fraction(second.Numerator * first.Denominator, second.Denominator * first.Denominator);
            var Result = new Fraction(Value1.Numerator + Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction first, Fraction second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction first, double second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(double first, Fraction second)
        {
            return second.Equals(first);
        }

        /// <summary>
        /// Determines if the fractions are equal
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Fraction Other))
            {
                return false;
            }

            decimal Value1 = this;
            decimal Value2 = Other;
            return Value1 == Value2;
        }

        /// <summary>
        /// Gets the hash code of the fraction
        /// </summary>
        /// <returns>The hash code of the fraction</returns>
        public override int GetHashCode() => Numerator.GetHashCode() % Denominator.GetHashCode();

        /// <summary>
        /// Returns the inverse of the fraction
        /// </summary>
        /// <returns>The inverse</returns>
        public Fraction Inverse() => new Fraction(Denominator, Numerator);

        /// <summary>
        /// Reduces the fraction (finds the greatest common denominator and divides the
        /// numerator/denominator by it).
        /// </summary>
        public void Reduce()
        {
            if (Numerator == int.MinValue)
            {
                Numerator = int.MinValue + 1;
            }

            if (Denominator == int.MinValue)
            {
                Denominator = int.MinValue + 1;
            }

            var GCD = Numerator.GreatestCommonDenominator(Denominator);
            if (GCD != 0)
            {
                Numerator /= GCD;
                Denominator /= GCD;
            }
        }

        /// <summary>
        /// Displays the fraction as a string
        /// </summary>
        /// <returns>The fraction as a string</returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Numerator, Denominator);
    }
}