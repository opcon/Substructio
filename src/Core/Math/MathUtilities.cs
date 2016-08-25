using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Core.Math
{
    public static class MathUtilities
    {
        public const double TwoPI = 2*System.Math.PI;

        /// <summary>
        /// Normalizes any number to an arbitrary range 
        /// by assuming the range wraps around when going below min or above max.
        /// adapted from http://stackoverflow.com/a/2021986
        /// </summary>
        /// <param name="value">The value to normalise</param>
        /// <param name="start">The start of the range</param>
        /// <param name="end">The end of the range</param>
        /// <returns>The normalised value.</returns>
        public static double Normalise(double value, double start = 0, double end = TwoPI)
        {
            var width = end - start;
            var offsetValue = value - start;

            return (offsetValue - (System.Math.Floor(offsetValue/width)*width)) + start;
        }
    }
}
