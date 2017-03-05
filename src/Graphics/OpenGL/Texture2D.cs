using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class Texture2D : Texture
    {
        public Texture2D()
        {
            TextureType = TextureTarget.Texture2D;
        }

        public void LoadTexture(string imagePath)
        {
            using (var bmp = new System.Drawing.Bitmap(imagePath))
            {
                LoadTexture(bmp);
            }
        }

        public void LoadTexture(System.Drawing.Bitmap bitmap)
        {
            Bind();
            var rawData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexParameter(TextureType, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureType, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, rawData.Width, rawData.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, rawData.Scan0);

            bitmap.UnlockBits(rawData);
            UnBind();
        }
    }
}
