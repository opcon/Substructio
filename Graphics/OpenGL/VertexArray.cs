using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class VertexArray : BindeableObject
    {
        public ShaderProgram VAOProgram { get; private set; }
        public List<VertexBuffer> Buffers { get; private set; } 

        public VertexArray()
        {
            Create();
        }

        public void Load(ShaderProgram program, IEnumerable<VertexBuffer> buffers)
        {
            VAOProgram = program;
            Buffers = new List<VertexBuffer>(buffers);
            UpdateVertexAttributes();
        }

        public void UpdateVertexAttributes()
        {
            foreach (var vertexBuffer in Buffers)
            {
                vertexBuffer.Bind();
                foreach (var spec in vertexBuffer.DataSpecifications)
                {
                    GL.VertexAttribPointer(VAOProgram.AttributeLocation(spec.Name), spec.Count, spec.Type, spec.ShouldBeNormalised, spec.Stride, spec.Offset);
                    GL.EnableVertexAttribArray(VAOProgram.AttributeLocation(spec.Name));
                }
                //vertexBuffer.UnBind();
            }
            
        }

        public void Draw(double time)
        {
            Bind();

            foreach (var buffer in Buffers)
            {
                buffer.Bind();
            }
            GL.DrawArrays(PrimitiveType.Quads, 0, Buffers.Min(b => b.DrawableIndices));
            //GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 6);
            foreach (var buffer in Buffers)
            {
                buffer.UnBind();
            }

            UnBind();
        }

        public override void Bind()
        {
            GL.BindVertexArray(ID);
        }

        public override void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            UnBind();
            GL.DeleteVertexArray(ID);
        }

        public override void Create()
        {
            ID = GL.GenVertexArray();
        }

        public void Load(ShaderProgram program, params VertexBuffer[] buffers)
        {
            Load(program, (IEnumerable<VertexBuffer>)buffers);
        }
    }
}
