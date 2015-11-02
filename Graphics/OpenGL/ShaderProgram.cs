using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
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
            string info;
            GL.GetProgramInfoLog(ID, out info);
            if (!String.IsNullOrWhiteSpace(info)) throw new Exception(info);
        }


        public int AttributeLocation(string name)
        {
            if (!Attributes.ContainsKey(name))
                Attributes.Add(name, GL.GetAttribLocation(ID, name));
            return Attributes[name];
        }

        public int UniformLocation(string name)
        {
            if (!Uniforms.ContainsKey(name))
                Uniforms.Add(name, GL.GetUniformLocation(ID, name));
            return Uniforms[name];
        }

        #region SetUniformFunctions
        public void SetUniform(string name, float value)
        {
            GL.Uniform1(UniformLocation(name), value);
        }

        public void SetUniform(string name, params float[] values)
        {
            SetUniform(name, (IEnumerable<float>)values);
        }

        public void SetUniform(string name, IEnumerable<float> values)
        {
            var enumerable = values as float[] ?? values.ToArray();
            GL.Uniform1(UniformLocation(name), enumerable.Count(), enumerable.ToArray());
        }

        public void SetUniform(string name, params Vector2[] values)
        {
            SetUniform(name, (IEnumerable<Vector2>)values);
        }

        public void SetUniform(string name, IEnumerable<Vector2> values)
        {
            var enumerable = values as Vector2[] ?? values.ToArray();
            if (enumerable.Length == 1)
                GL.Uniform2(UniformLocation(name), enumerable[0]);
            else
            {
                var inArray = enumerable.SelectMany((v => new[] {v.X, v.Y})).ToArray();
                GL.Uniform2(UniformLocation(name), inArray.Length, inArray);
            }
        }

        public void SetUniform(string name, params Vector3[] values)
        {
            SetUniform(name, (IEnumerable<Vector3>)values);
        }

        public void SetUniform(string name, IEnumerable<Vector3> values)
        {
            var enumerable = values as Vector3[] ?? values.ToArray();
            if (enumerable.Length == 1)
                GL.Uniform3(UniformLocation(name), enumerable[0]);
            else
            {
                var inArray = enumerable.SelectMany((v => new[] {v.X, v.Y, v.Z})).ToArray();
                GL.Uniform3(UniformLocation(name), inArray.Length, inArray);
            }
        }

        public void SetUniform(string name, params Vector4[] values)
        {
            SetUniform(name, (IEnumerable<Vector4>)values);
        }

        public void SetUniform(string name, IEnumerable<Vector4> values)
        {
            var enumerable = values as Vector4[] ?? values.ToArray();
            if (enumerable.Length == 1)
                GL.Uniform4(UniformLocation(name), enumerable[0]);
            else
            {
                var inArray = enumerable.SelectMany(v => new[] {v.X, v.Y, v.Z, v.W}).ToArray();
                GL.Uniform4(UniformLocation(name), inArray.Length, inArray);
            }
        }

        public void SetUniform(string name, Quaternion value)
        {
            SetUniform(name, new Vector4(value.Xyz, value.W));
        }

        public void SetUniform(string name, Color4 value)
        {
            SetUniform(name, new Vector4(value.R, value.G, value.B, value.A));
        }

        public void SetUniform(string name, params Matrix2[] values)
        {
            SetUniform(name, (IEnumerable<Matrix2>)values);
        }

        public void SetUniform(string name, IEnumerable<Matrix2> values)
        {
            var enumerables = values as Matrix2[] ?? values.ToArray();
            if (enumerables.Length == 1)
                GL.UniformMatrix2(UniformLocation(name), false, ref enumerables[0]);
            else
            {
                var inArray = enumerables.SelectMany(m => new[] {m.M11, m.M21, m.M12, m.M22}).ToArray();
                GL.UniformMatrix2(UniformLocation(name), enumerables.Length, false, inArray);
            }
        }

        public void SetUniform(string name, params Matrix3[] values)
        {
            SetUniform(name, (IEnumerable<Matrix3>)values);
        }

        public void SetUniform(string name, IEnumerable<Matrix3> values)
        {
            var enumerables = values as Matrix3[] ?? values.ToArray();
            if (enumerables.Length == 1)
                GL.UniformMatrix3(UniformLocation(name), false, ref enumerables[0]);
            else
            {
                var inArray =
                    enumerables.SelectMany(m => new[] {m.M11, m.M21, m.M31, m.M12, m.M22, m.M32, m.M13, m.M23, m.M33})
                        .ToArray();
                GL.UniformMatrix3(UniformLocation(name), enumerables.Length, false, inArray);
            }
        }

        public void SetUniform(string name, params Matrix4[] values)
        {
            SetUniform(name, (IEnumerable<Matrix4>)values);
        }

        public void SetUniform(string name, IEnumerable<Matrix4> values)
        {
            var enumerables = values as Matrix4[] ?? values.ToArray();
            if (enumerables.Length == 1)
                GL.UniformMatrix4(UniformLocation(name), false, ref enumerables[0]);
            else
            {
                var inArray =
                    enumerables.SelectMany(
                        m =>
                            new[]
                            {
                                m.M11, m.M21, m.M31, m.M41, m.M12, m.M22, m.M32, m.M42, m.M13, m.M23, m.M33, m.M43, m.M14,
                                m.M24, m.M34, m.M44
                            }).ToArray();
                GL.UniformMatrix3(UniformLocation(name), enumerables.Length, false, inArray);
            }
        }

        #endregion

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
