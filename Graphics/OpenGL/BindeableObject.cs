using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Graphics.OpenGL
{
    public abstract class BindeableObject : IDisposable
    {
        public int ID { get; protected set; }

        public abstract void Bind();

        public abstract void UnBind();

        public abstract void Dispose();

        public abstract void Create();
    }
}
