using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class Texture : BindeableObject
    {
        public TextureTarget TextureType { get; protected set; }

        public Texture()
        {
            Create();
        }

        public override void Bind()
        {
            GL.BindTexture(TextureType, ID);
        }

        public override void UnBind()
        {
            GL.BindTexture(TextureType, 0);
        }

        public override void Create()
        {
            int texID;
            GL.GenTextures(1, out texID);
            ID = texID;
        }

        public override void Dispose()
        {
            GL.DeleteTexture(ID);
        }
    }
}
