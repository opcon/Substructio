using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class Buffer : BindeableObject
    {
        public BufferTarget BufferType { get; protected set; }

        public BufferUsageHint BufferUsage { get; set; }

        public int MaxSize { get; set; }

        public bool Initialised { get; protected set; }

        public List<BufferDataSpecification> DataSpecifications { get; private set; }

        public Buffer()
        {
            Create();
            DataSpecifications = new List<BufferDataSpecification>();
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferType, ID);
        }

        public override void UnBind()
        {
            GL.BindBuffer(BufferType, 0);
        }

        public void Initialise()
        {
            Initialise(MaxSize);
        }

        public void Initialise(int size)
        {
            GL.BufferData(BufferType, (IntPtr)size, IntPtr.Zero, BufferUsage);
            Initialised = true;
        }

        public void AddSpec(BufferDataSpecification spec)
        {
            if (!DataSpecifications.Contains(spec))
            {
                DataSpecifications.Add(spec);
            }
        }

        //public virtual void AddData<T>(IEnumerable<T> data, BufferDataSpecification spec) where T : struct
        //{
        //    if (!DataSpecifications.Contains(spec))
        //    {
        //        SetData(data, spec.Offset);
        //        DataSpecifications.Add(spec);
        //    }
        //}

        public void SetData<T>(IEnumerable<T> data, BufferDataSpecification spec) where T : struct
        {
            SetData(data, spec.Offset);
        }

        public void SetData<T>(IEnumerable<T> data, int offset) where T : struct
        {
            var t = default(T);
            var size = Marshal.SizeOf(t) * data.Count();
            //var test = data.ToArray();
            SetData(data, offset, size);
        }

        public void SetData<T>(IEnumerable<T> data, int offset, int size) where T : struct
        {
            GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)size, data.ToArray());
        }

        public override void Dispose()
        {
            UnBind();
            GL.DeleteBuffer(ID);
        }

        public override sealed void Create()
        {
            ID = GL.GenBuffer();
        }

        public virtual void CalculateMaxSize()
        {
            MaxSize = 0;
        }

    }

    public struct BufferDataSpecification
    {
        public string Name;
        public int Offset;
        public int Count;
        public VertexAttribPointerType Type;
        public bool ShouldBeNormalised;
        public int Stride;
        public int SizeInBytes;
    }
}
