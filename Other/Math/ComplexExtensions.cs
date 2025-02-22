﻿// <copyright file="ComplexExtensions.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2010 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace MathNet.Numerics
{
    using System;
    using System.Collections.Generic;

#if !PORTABLE
    using System.Runtime;
#endif

#if !NOSYSNUMERICS
    using Complex = System.Numerics.Complex;
#endif

    /// <summary>
    /// Extension methods for the Complex type provided by System.Numerics
    /// </summary>
    public static class ComplexExtensions
    {
        /// <summary>
        /// Gets the squared magnitude of the <c>Complex</c> number.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>The squared magnitude of the <c>Complex</c> number.</returns>
        public static double MagnitudeSquared(this Complex complex)
        {
            return (complex.Real * complex.Real) + (complex.Imaginary * complex.Imaginary);
        }

        /// <summary>
        /// Gets the conjugate of the <c>Complex</c> number.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <remarks>
        /// The semantic of <i>setting the conjugate</i> is such that
        /// <code>
        /// // a, b of type Complex32
        /// a.Conjugate = b;
        /// </code>
        /// is equivalent to
        /// <code>
        /// // a, b of type Complex32
        /// a = b.Conjugate
        /// </code>
        /// </remarks>
        /// <returns>The conjugate of the <see cref="Complex"/> number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Conjugate(this Complex complex)
        {
            return Complex.Conjugate(complex);
        }

        /// <summary>
        /// Returns the multiplicative inverse of a complex number.
        /// </summary>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Reciprocal(this Complex complex)
        {
            return Complex.Reciprocal(complex);
        }

        /// <summary>
        /// Exponential of this <c>Complex</c> (exp(x), E^x).
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        /// The exponential of this complex number.
        /// </returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Exp(this Complex complex)
        {
            return Complex.Exp(complex);
        }

        /// <summary>
        /// Natural Logarithm of this <c>Complex</c> (Base E).
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        /// The natural logarithm of this complex number.
        /// </returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Ln(this Complex complex)
        {
            return Complex.Log(complex);
        }

        /// <summary>
        /// Common Logarithm of this <c>Complex</c> (Base 10).
        /// </summary>
        /// <returns>The common logarithm of this complex number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Log10(this Complex complex)
        {
            return Complex.Log10(complex);
        }

        /// <summary>
        /// Logarithm of this <c>Complex</c> with custom base.
        /// </summary>
        /// <returns>The logarithm of this complex number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex Log(this Complex complex, double baseValue)
        {
            return Complex.Log(complex, baseValue);
        }

        /// <summary>
        /// Raise this <c>Complex</c> to the given value.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <param name="exponent">
        /// The exponent.
        /// </param>
        /// <returns>
        /// The complex number raised to the given exponent.
        /// </returns>
        public static Complex Power(this Complex complex, Complex exponent)
        {
            if (complex.IsZero())
            {
                if (exponent.IsZero())
                {
                    return Complex.One;
                }

                if (exponent.Real > 0d)
                {
                    return Complex.Zero;
                }

                if (exponent.Real < 0d)
                {
                    return exponent.Imaginary == 0d
                        ? new Complex(double.PositiveInfinity, 0d)
                        : new Complex(double.PositiveInfinity, double.PositiveInfinity);
                }

                return new Complex(double.NaN, double.NaN);
            }

            return Complex.Pow(complex, exponent);
        }

        /// <summary>
        /// Raise this <c>Complex</c> to the inverse of the given value.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <param name="rootExponent">
        /// The root exponent.
        /// </param>
        /// <returns>
        /// The complex raised to the inverse of the given exponent.
        /// </returns>
        public static Complex Root(this Complex complex, Complex rootExponent)
        {
            return Complex.Pow(complex, 1 / rootExponent);
        }

        /// <summary>
        /// The Square (power 2) of this <c>Complex</c>
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        /// The square of this complex number.
        /// </returns>
        public static Complex Square(this Complex complex)
        {
            if (complex.IsReal())
            {
                return new Complex(complex.Real * complex.Real, 0.0);
            }

            return new Complex((complex.Real * complex.Real) - (complex.Imaginary * complex.Imaginary), 2 * complex.Real * complex.Imaginary);
        }

        /// <summary>
        /// The Square Root (power 1/2) of this <c>Complex</c>
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        /// The square root of this complex number.
        /// </returns>
        public static Complex SquareRoot(this Complex complex)
        {
            // Note: the following code should be equivalent to Complex.Sqrt(complex),
            // but it turns out that is implemented poorly in System.Numerics,
            // hence we provide our own implementation here. Do not replace.

            if (complex.IsRealNonNegative())
            {
                return new Complex(Math.Sqrt(complex.Real), 0.0);
            }

            Complex result;

            var absReal = Math.Abs(complex.Real);
            var absImag = Math.Abs(complex.Imaginary);
            double w;
            if (absReal >= absImag)
            {
                var ratio = complex.Imaginary / complex.Real;
                w = Math.Sqrt(absReal) * Math.Sqrt(0.5 * (1.0 + Math.Sqrt(1.0 + (ratio * ratio))));
            }
            else
            {
                var ratio = complex.Real / complex.Imaginary;
                w = Math.Sqrt(absImag) * Math.Sqrt(0.5 * (Math.Abs(ratio) + Math.Sqrt(1.0 + (ratio * ratio))));
            }

            if (complex.Real >= 0.0)
            {
                result = new Complex(w, complex.Imaginary / (2.0 * w));
            }
            else if (complex.Imaginary >= 0.0)
            {
                result = new Complex(absImag / (2.0 * w), w);
            }
            else
            {
                result = new Complex(absImag / (2.0 * w), -w);
            }

            return result;
        }

        /// <summary>
        /// Evaluate all square roots of this <c>Complex</c>.
        /// </summary>
        public static Tuple<Complex, Complex> SquareRoots(this Complex complex)
        {
            var principal = SquareRoot(complex);
            return new Tuple<Complex, Complex>(principal, -principal);
        }

        /// <summary>
        /// Evaluate all cubic roots of this <c>Complex</c>.
        /// </summary>
        public static Tuple<Complex, Complex, Complex> CubicRoots(this Complex complex)
        {
            var r = Math.Pow(complex.Magnitude, 1d/3d);
            var theta = complex.Phase/3;
            const double shift = Constants.Pi2/3;
            return new Tuple<Complex, Complex, Complex>(
                Complex.FromPolarCoordinates(r, theta),
                Complex.FromPolarCoordinates(r, theta + shift),
                Complex.FromPolarCoordinates(r, theta - shift));
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is zero.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns><c>true</c> if this instance is zero; otherwise, <c>false</c>.</returns>
        public static bool IsZero(this Complex complex)
        {
            return complex.Real == 0.0 && complex.Imaginary == 0.0;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is one.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns><c>true</c> if this instance is one; otherwise, <c>false</c>.</returns>
        public static bool IsOne(this Complex complex)
        {
            return complex.Real == 1.0 && complex.Imaginary == 0.0;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is the imaginary unit.
        /// </summary>
        /// <returns><c>true</c> if this instance is ImaginaryOne; otherwise, <c>false</c>.</returns>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        public static bool IsImaginaryOne(this Complex complex)
        {
            return complex.Real == 0.0 && complex.Imaginary == 1.0;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c>evaluates
        /// to a value that is not a number.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        /// <c>true</c> if this instance is <c>NaN</c>; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsNaN(this Complex complex)
        {
            return double.IsNaN(complex.Real) || double.IsNaN(complex.Imaginary);
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> evaluates to an
        /// infinite value.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        ///     <c>true</c> if this instance is infinite; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// True if it either evaluates to a complex infinity
        /// or to a directed infinity.
        /// </remarks>
        public static bool IsInfinity(this Complex complex)
        {
            return double.IsInfinity(complex.Real) || double.IsInfinity(complex.Imaginary);
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns><c>true</c> if this instance is a real number; otherwise, <c>false</c>.</returns>
        public static bool IsReal(this Complex complex)
        {
            return complex.Imaginary == 0.0;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real and not negative, that is &gt;= 0.
        /// </summary>
        /// <param name="complex">The <see cref="Complex"/> number to perfom this operation on.</param>
        /// <returns>
        ///     <c>true</c> if this instance is real nonnegative number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRealNonNegative(this Complex complex)
        {
            return complex.Imaginary == 0.0f && complex.Real >= 0;
        }

        /// <summary>
        /// Returns a Norm of a value of this type, which is appropriate for measuring how
        /// close this value is to zero.
        /// </summary>
        public static double Norm(this Complex complex)
        {
            return complex.MagnitudeSquared();
        }

        /// <summary>
        /// Returns a Norm of the difference of two values of this type, which is
        /// appropriate for measuring how close together these two values are.
        /// </summary>
        public static double NormOfDifference(this Complex complex, Complex otherValue)
        {
            return (complex - otherValue).MagnitudeSquared();
        }
    }
}