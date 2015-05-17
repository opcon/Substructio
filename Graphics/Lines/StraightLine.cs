//
// Port to C# and OpenTK of code from http://www.codeproject.com/Articles/199525/Drawing-nearly-perfect-D-line-segments-in-OpenGL
// 
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Substructio.Graphics.Lines
{
    public class StraightLine
    {
        public Vector2[] line_vertex;
        public Color4[] line_colour;
        public Vector2[] line_cap_vertex;
        public Color4[] line_cap_colour;

        public StraightLine()
        {
            line_vertex = new Vector2[8];
            line_colour = new Color4[8];
            line_cap_vertex = new Vector2[12];
            line_cap_colour = new Color4[12];
        }

        public void Line(Vector2 p1, Vector2 p2, float width, Color4 colour, Color4 backgroundColour, bool alphaBlend)
        {
            float t;
            float R;
            float f = width - (int)width;
            float A;

            if (alphaBlend)
                A = colour.A;
            else
                A = 1.0f;

            //Determine parameters t,R
            if (width >= 0.0 && width < 1.0)
            {
                t = 0.05f;
                R = 0.48f + 0.32f * f;
                if (!alphaBlend)
                {
                    colour.R += 0.88f * (1 - f);
                    colour.G += 0.88f * (1 - f);
                    colour.B += 0.88f * (1 - f);
                    if (colour.R > 1.0)
                        colour.R = 1.0f;
                    if (colour.G > 1.0)
                        colour.G = 1.0f;
                    if (colour.B > 1.0)
                        colour.B = 1.0f;
                }
                else
                {
                    A *= f;
                }
            }
            else if (width >= 1.0 && width < 2.0)
            {
                t = 0.05f + f * 0.33f;
                R = 0.768f + 0.312f * f;
            }
            else if (width >= 2.0 && width < 3.0)
            {
                t = 0.38f + f * 0.58f;
                R = 1.08f;
            }
            else if (width >= 3.0 && width < 4.0)
            {
                t = 0.96f + f * 0.48f;
                R = 1.08f;
            }
            else if (width >= 4.0 && width < 5.0)
            {
                t = 1.44f + f * 0.46f;
                R = 1.08f;
            }
            else if (width >= 5.0 && width < 6.0)
            {
                t = 1.9f + f * 0.6f;
                R = 1.08f;
            }
            else if (width >= 6.0)
            {
                float ff = width - 6.0f;
                t = 2.5f + ff * 0.50f;
                R = 1.08f;
            }
            else
            {
                t = 0;
                R = 0;
            }

            //determine angle of the line to horizontal
            float tx=0,ty=0; //core thinkness of a line
            float Rx=0,Ry=0; //fading edge of a line
            float cx=0,cy=0; //cap of a line
            float ALW=0.01f;
            float dx=p2.X - p1.X;
            float dy=p2.Y - p1.Y;
            if (Math.Abs(dx) < ALW) {
                //vertical
                tx=t; ty=0;
                Rx=R; Ry=0;
                if ( width>0.0 && width<=1.0) {
                    tx = 0.5f; Rx=0.0f;
                }
            } else if (Math.Abs(dy) < ALW) {
                //horizontal
                tx=0; ty=t;
                Rx=0; Ry=R;
                if ( width>0.0 && width<=1.0) {
                    ty = 0.5f; Ry=0.0f;
                }
            } else {
                if ( width < 3) { //approximate to make things even faster
                    double m=dy/dx;
                    //and calculate tx,ty,Rx,Ry
                    if ( m>-0.4142 && m<=0.4142) {
                        // -22.5< angle <= 22.5, approximate to 0 (degree)
                        tx=t*0.1f; ty=t;
                        Rx=R*0.6f; Ry=R;
                    } else if ( m>0.4142 && m<=2.4142) {
                        // 22.5< angle <= 67.5, approximate to 45 (degree)
                        tx=t*-0.7071f; ty=t*0.7071f;
                        Rx=R*-0.7071f; Ry=R*0.7071f;
                    } else if ( m>2.4142 || m<=-2.4142) {
                        // 67.5 < angle <=112.5, approximate to 90 (degree)
                        tx=t; ty=t*0.1f;
                        Rx=R; Ry=R*0.6f;
                    } else if ( m>-2.4142 && m<-0.4142) {
                        // 112.5 < angle < 157.5, approximate to 135 (degree)
                        tx=t*0.7071f; ty=t*0.7071f;
                        Rx=R*0.7071f; Ry=R*0.7071f;
                    } else {
                        // error in determining angle
                        //printf( "error in determining angle: m=%.4f\n",m);
                    }
                } else { //calculate to exact
                    //SHOULD THESE BE THE OTHER WAY AROUND?
                    dx = p1.Y - p2.Y;
                    dy = p2.X - p1.X;

                    float L=(float)Math.Sqrt(dx*dx+dy*dy);
                    dx/=L;
                    dy/=L;
                    cx=-dy; cy=dx;
                    tx=t*dx; ty=t*dy;
                    Rx=R*dx; Ry=R*dy;
                }
            }

            p1.X +=cx*0.5f; p1.Y+=cy*0.5f;
            p2.X-=cx*0.5f; p2.Y-=cy*0.5f;

            //draw the line by triangle strip
            line_vertex = new Vector2[]
            {
                new Vector2(p1.X -tx - Rx - cx, p1.Y - ty - Ry - cy), //fading edge 1
                new Vector2(p2.X -tx -Rx + cx, p2.Y -ty -Ry + cy),
                new Vector2(p1.X - tx - cx, p1.Y - ty - cy), //core
                new Vector2(p2.X - tx + cx, p2.Y - ty + cy),
                new Vector2(p1.X + tx - cx, p1.Y + ty - cy),
                new Vector2(p2.X +tx + cx, p2.Y + ty + cy),
                new Vector2(p1.X + tx + Rx - cx, p1.Y + ty + Ry - cy), //fading edge 2
                new Vector2(p2.X + tx + Rx + cx, p2.Y + ty + Ry + cy)
            };

            if (!alphaBlend)
            {
                line_colour = new Color4[]
                {
                    backgroundColour,
                    backgroundColour,
                    colour,
                    colour,
                    colour,
                    colour,
                    backgroundColour,
                    backgroundColour
                };
            }
            else
            {
                line_colour = new Color4[]
                {
                    new Color4(colour.R, colour.G, colour.B, 0),
                    new Color4(colour.R, colour.G, colour.B, 0),
                    new Color4(colour.R, colour.G, colour.B, A),
                    new Color4(colour.R, colour.G, colour.B, A),
                    new Color4(colour.R, colour.G, colour.B, A),
                    new Color4(colour.R, colour.G, colour.B, A),
                    new Color4(colour.R, colour.G, colour.B, 0),
                    new Color4(colour.R, colour.G, colour.B, 0)
                }; 
            }

            if (width < 3)
            {
                //don't draw a cap
            }
            else
            {
                //draw a cap
                line_cap_vertex = new Vector2[]
                {
                    new Vector2(p1.X - tx - Rx - cx, p1.Y - ty - Ry - cy), //cap1
                    new Vector2(p1.X - tx - Rx, p1.Y - ty - Ry),
                    new Vector2(p1.X - tx - cx, p1.Y - ty - cy),
                    new Vector2(p1.X + tx + Rx, p1.Y + ty + Ry),
                    new Vector2(p1.X + tx - cx, p2.Y + ty - cy),
                    new Vector2(p1.X + tx + Rx - cx, p1.Y + ty + Ry - cy),
                    new Vector2(p2.X - tx - Rx + cx, p2.Y - ty - Ry + cy), //cap2
                    new Vector2(p2.X - tx - Rx, p2.Y - ty - Ry),
                    new Vector2(p2.X - tx - cx, p2.Y - ty + cy),
                    new Vector2(p2.X + tx + Rx, p2.Y + ty + Ry),
                    new Vector2(p2.X + tx + cx, p2.Y + ty + cy),
                    new Vector2(p2.X + tx + Rx + cx, p2.Y + ty + Ry + cy)
                };

                if (!alphaBlend)
                {
                    line_cap_colour = new Color4[]
                    {
                        backgroundColour, //cap1
                        backgroundColour,
                        colour,
                        backgroundColour,
                        colour,
                        backgroundColour,
                        backgroundColour, //cap2
                        backgroundColour,
                        colour,
                        backgroundColour,
                        colour,
                        backgroundColour
                    };
                }
                else
                {
                    line_cap_colour = new Color4[]
                    {
                        new Color4(colour.R, colour.G, colour.B, 0), //cap1
                        new Color4(colour.R, colour.G, colour.B, 0),
                        new Color4(colour.R, colour.G, colour.B, A),
                        new Color4(colour.R, colour.G, colour.B, 0),
                        new Color4(colour.R, colour.G, colour.B, A),
                        new Color4(colour.R, colour.G, colour.B, 0),
                        new Color4(colour.R, colour.G, colour.B, 0), //cap2
                        new Color4(colour.R, colour.G, colour.B, 0),
                        new Color4(colour.R, colour.G, colour.B, A),
                        new Color4(colour.R, colour.G, colour.B, 0),
                        new Color4(colour.R, colour.G, colour.B, A),
                        new Color4(colour.R, colour.G, colour.B, 0),
                    };
                }
            }

        }
    }
}

