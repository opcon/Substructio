using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class VertexBuffer : Buffer
    {
        public int DrawableIndices { get; set; }
        public int MaxDrawableIndices {get; set; }

        public VertexBuffer()
        {
            BufferType = BufferTarget.ArrayBuffer;
        }

        public override void CalculateMaxSize()
        {
            Debug.Assert(MaxDrawableIndices != 0);
            MaxSize = 0;
            DataSpecifications.ForEach(d => MaxSize += d.Count * MaxDrawableIndices * d.SizeInBytes);
        }

        //Recalculates max size whenever called - can potentially be very slow!
        public void CalculateDynamicMaxSize()
        {
            MaxSize = 0;
            DataSpecifications.ForEach(d => MaxSize += d.Count * DrawableIndices * d.SizeInBytes);
        }

        //Dyanmically recalculates the size needed for this vertex buffer. Can cause unnecessary resizing and can be slow!
        public void DynamicInitialise()
        {
            CalculateDynamicMaxSize();
            this.Initialise();
        }
    }
}
