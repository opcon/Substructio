using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using ColorMine.ColorSpaces;

namespace Substructio.Core
{
    public static class Utilities
    {
        private static Random _random;

        public static Random RandomGenerator
        {
            get { return _random ?? (_random = new Random()); }
        }

        public static List<Vector2> StringToVecList(IEnumerable<string> bcoords)
        {
            return bcoords.Select(s => Array.ConvertAll(s.Split(','), str => (Single.Parse(str)))).Select(
                point => new Vector2(point[0], point[1])).ToList();
        }

        public static string VecListToString(List<Vector2> positions)
        {
            string s = "";
            foreach (var p in positions)
            {
                s += String.Format("{0},{1} ", p.X, p.Y);
            }
            return s.Trim();
        }

        //public static void TranslateTo(Vector2 position, float width, float height)
        //{
        //    //GL.MatrixMode(MatrixMode.Modelview);
        //    //GL.LoadIdentity();

        //    //GL.Translate(-(width/2) + position.X, -(height/2) + position.Y, 0);

        //    //GL.Scale(1, -1, 1);
        //}

        // Handles IPv4 and IPv6 notation.
        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            IPAddress ip;
            if (ep.Length == 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid IP Address");
                }
            }
            else if (ep.Length ==1)
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid IP Address");
                }
                return new IPEndPoint(ip, 0);
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid IP Address");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid Port");
            }
            return new IPEndPoint(ip, port);
        }

        public static void TakeScreenShot(GameWindow g, string file)
        {
            Bitmap b = ScreenToBitmap(g);
            b.Save(file, ImageFormat.Png);
        }

        public static Bitmap ScreenToBitmap(GameWindow g)
        {
            Bitmap b = new Bitmap(g.Width, g.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            var bd = b.LockBits(new Rectangle(0, 0, g.Width, g.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, g.Width, g.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgr, PixelType.UnsignedByte, bd.Scan0);

            b.UnlockBits(bd);
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return b;
        }

        public static Color4 ColorSpaceToColor4(IColorSpace col)
        {
            var rgb = col.ToRgb();
            return new Color4((byte)rgb.R, (byte)rgb.G, (byte)rgb.B, 255);
        }

        public static IColorSpace Color4ToColorSpace(Color4 col)
        {
            return new Rgb() {B = col.B*255, G = col.G*255, R = col.R*255};
        }
    }
}