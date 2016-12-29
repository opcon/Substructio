using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Graphics.OpenGL
{

    [Serializable]
    public class ShaderProgramLinkException : Exception
    {
        public string ProgramLog
        {
            get
            {
                return (string)this.Data["ProgramLog"];
            }
        }

        public string AttachedShaders
        {
            get
            {
                return (string)this.Data["AttachedShaders"];
            }
        }

        public ShaderProgramLinkException() { }
        public ShaderProgramLinkException(string message, string log, string attachedShaders) : base(message)
        {
            this.Data.Add("ProgramLog", log);
            this.Data.Add("AttachedShaders", attachedShaders);
        }
        public ShaderProgramLinkException(string message, string log, string attachedShaders, Exception inner) : base(message, inner)
        {
            this.Data.Add("ProgramLog", log);
            this.Data.Add("AttachedShaders", attachedShaders);
        }
        protected ShaderProgramLinkException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class ShaderTypeException : Exception
    {
        public ShaderTypeException() { }
        public ShaderTypeException(string message) : base(message) { }
        public ShaderTypeException(string message, Exception inner) : base(message, inner) { }
        protected ShaderTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class ShaderCompilationException : Exception
    {
        public string ShaderLog
        {
            get
            {
                return (string)this.Data["ShaderLog"];
            }
        }

        public string ShaderType
        {
            get
            {
                return (string)this.Data["ShaderType"];
            }
        }

        public string ShaderSource
        {
            get
            {
                return (string)this.Data["ShaderSource"];
            }
        }

        public ShaderCompilationException() { }

        public ShaderCompilationException(string message, string shaderLog, OpenTK.Graphics.OpenGL4.ShaderType shaderType, string shaderSource) : base(message)
        {
            Data.Add("ShaderLog", shaderLog);
            Data.Add("ShaderType", shaderType.ToString());
            Data.Add("ShaderSource", shaderSource);
        }
        public ShaderCompilationException(string message, string shaderLog, OpenTK.Graphics.OpenGL4.ShaderType shaderType, string shaderSource, Exception inner) : base(message, inner)
        {
            Data.Add("ShaderLog", shaderLog);
            Data.Add("ShaderType", shaderType.ToString());
            Data.Add("ShaderSource", shaderSource);
        }
        protected ShaderCompilationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
