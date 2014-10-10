using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class VertexBuffer : Buffer
    {
        public int DrawableIndices { get; set; }

        public VertexBuffer()
        {
            BufferType = BufferTarget.ArrayBuffer;
        }

        new public void Initialise()
        {
            if (MaxSize == 0)
                DataSpecifications.ForEach(d => MaxSize += d.Count * DrawableIndices * 4);
            Initialise(MaxSize);
        }
    }
}
