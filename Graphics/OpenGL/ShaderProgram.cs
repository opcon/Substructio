using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class ShaderProgram : BindeableObject
    {
        public Dictionary<string, int> Attributes { get; private set; }
        public Dictionary<string, int> Uniforms { get; private set; } 
        public ShaderProgram()
        {
            Attributes = new Dictionary<string, int>();
            Uniforms = new Dictionary<string, int>();
            Create();
        }

        public void Load(params Shader[] shaders)
        {
            Load((IEnumerable<Shader>)shaders);
        }

        public void Load(IEnumerable<Shader> shaders)
        {
            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }
            GL.LinkProgram(ID);
        }


        public int AttributeLocation(string name)
        {
            if (!Attributes.ContainsKey(name))
                Attributes.Add(name, GL.GetAttribLocation(ID, name));
            return Attributes[name];
        }

        public void AttachShader(Shader shader)
        {
            GL.AttachShader(ID, shader.ID);
        }

        public override void Bind()
        {
            GL.UseProgram(ID);
        }

        public override void UnBind()
        {
            GL.UseProgram(0);
        }

        public override void Dispose()
        {
            UnBind();
            GL.DeleteProgram(ID);
        }

        public override void Create()
        {
            ID = GL.CreateProgram();
        }
    }
}
