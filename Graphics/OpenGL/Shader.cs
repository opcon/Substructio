using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Substructio.Graphics.OpenGL
{
    public class Shader : BindeableObject
    {
        public ShaderType ShaderType { get; private set; }
        public string Source { get; private set; }
        public string Name { get; private set; }

        public Shader(string path)
        {
            Load(path);
        }

        public Shader(string path, ShaderType type)
        {
            Load(path, type);
        }

        public Shader(string source, string name, ShaderType type)
        {
            Load(source, name, type);
        }

        public void Load(string path, ShaderType type)
        {
            var source = IO.ASCIIFileHelper.ReadFileToEnd(path);
            var name = Path.GetFileNameWithoutExtension(path);
            Load(source, name, type);
        }

        public void Load(string path)
        {
            var ext = Path.GetExtension(path);
            ShaderType type;
            switch (ext)
            {
                case ".fs":
                    type = ShaderType.FragmentShader;
                    break;
                case ".vs":
                    type = ShaderType.VertexShader;
                    break;
                case ".gs":
                    type = ShaderType.GeometryShader;
                    break;
                default:
                    throw new Exception("Unknown shader file specified");
            }

            Load(path, type);
        }

        public void Load(string source, string name, ShaderType type)
        {
            Source = source;
            ShaderType = type;
            Name = name;

            Create();
            GL.ShaderSource(ID, Source);
            GL.CompileShader(ID);
        }

        public override void Bind()
        {
            throw new Exception("Shaders can not be bound");
        }

        public override void UnBind()
        {
            throw new Exception("Shaders can not be bound");
        }

        public override void Dispose()
        {
            GL.DeleteShader(ID);
        }

        public override void Create()
        {
            ID = GL.CreateShader(ShaderType);
        }
    }
}
