using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Substructio.Core.Math
{
    public class PolarVector
    {
        private const double AngleRangeStart = 0;
        private const double AngleRangeEnd = MathUtilities.TwoPI;

        public double Radius { get; set; }
        public double Azimuth { get; set; }

        public PolarVector(double azimuth, double radius)
        {
            Azimuth = azimuth;
            Radius = radius;
        }

        public PolarVector()
        {
            Azimuth = 0;
            Radius = 0;
        }

        public Vector2 ToCartesianCoordinates()
        {
            return PolarVector.ToCartesianCoordinates(this);
        }

        public static Vector2 ToCartesianCoordinates(PolarVector polarVector)
        {
            return ToCartesianCoordinates(polarVector, 0, 0);
        }

        public PolarVector Normalised()
        {
            return NormaliseAngle(this);
        }

        public static PolarVector NormaliseAngle(PolarVector p)
        {
            p.Azimuth = MathUtilities.Normalise(p.Azimuth, AngleRangeStart, AngleRangeEnd);
            return p;
        }

        public static Vector2 ToCartesianCoordinates(PolarVector polarVector, double dAzimuth, double dRadius)
        {
           return new Vector2((float)((polarVector.Radius+dRadius) * System.Math.Cos(polarVector.Azimuth+dAzimuth)), (float)((polarVector.Radius+dRadius) * System.Math.Sin(polarVector.Azimuth+dAzimuth))); 
        }
    }
}
