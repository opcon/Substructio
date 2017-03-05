using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Substructio.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Substructio.Core
{
    public class Sprite
    {
        public Texture2D SpriteTexture { get; protected set; }
        public Vector2 Size
        {
            get
            {
                return _originalSize * Scale;
            }
        }

        public ShaderProgram Program { get; protected set; }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _invalidated = true;
            }
        }

        public float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                _invalidated = true;
            }
        }

        protected Vector2 _position;
        protected Vector2 _originalSize;
        protected float _scale = 1.0f;
        protected VertexArray _vertexArray;
        protected VertexBuffer _positionBuffer;
        protected VertexBuffer _textureBuffer;

        private bool _invalidated = false;
        private List<Vector2> _vertices;

        public Sprite(ShaderProgram program)
        {
            Program = program;
            Position = Vector2.Zero;
            _vertices = new List<Vector2>();
        }

        public void Initialise(string imagePath)
        {
            using (var bmp = new System.Drawing.Bitmap(imagePath))
            {
                Initialise(bmp);
            }
        }

        public void Initialise(System.Drawing.Bitmap imageBitmap)
        {
            SpriteTexture = new Texture2D();
            SpriteTexture.LoadTexture(imageBitmap);

            _originalSize = new Vector2(imageBitmap.Width, imageBitmap.Height);

            var posDS = new BufferDataSpecification
            {
                Count = 2,
                Name = "in_position",
                Offset = 0,
                ShouldBeNormalised = false,
                Stride = 0,
                Type = VertexAttribPointerType.Float,
                SizeInBytes = sizeof(float)
            };

            var tcDS = new BufferDataSpecification
            {
                Count = 2,
                Name = "in_tc",
                Offset = 0,
                ShouldBeNormalised = false,
                Stride = 0,
                Type = VertexAttribPointerType.Float,
                SizeInBytes = sizeof(float)
            };

            _vertexArray = new VertexArray { DrawPrimitiveType = PrimitiveType.Triangles };
            _vertexArray.Bind();

            _positionBuffer = new VertexBuffer
            {
                BufferUsage = BufferUsageHint.StreamDraw,
                DrawableIndices = 6,
                MaxDrawableIndices = 6
            };

            _textureBuffer = new VertexBuffer
            {
                BufferUsage = BufferUsageHint.StaticDraw,
                DrawableIndices = 6,
                MaxDrawableIndices = 6
            };

            _positionBuffer.AddSpec(posDS);
            _positionBuffer.CalculateMaxSize();
            _positionBuffer.Bind();
            _positionBuffer.Initialise();

            List<Vector2> textures = new List<Vector2>();
            textures.Add(new Vector2(0f, 1.0f));
            textures.Add(new Vector2(1.0f, 1.0f));
            textures.Add(new Vector2(1.0f, 0.0f));
            textures.Add(new Vector2(1.0f, 0.0f));
            textures.Add(new Vector2(0.0f, 0.0f));
            textures.Add(new Vector2(0f, 1.0f));

            _textureBuffer.AddSpec(tcDS);
            _textureBuffer.CalculateMaxSize();
            _textureBuffer.Bind();
            _textureBuffer.Initialise();
            _textureBuffer.SetData(textures.SelectMany(v => new[] { v.X, v.Y }), tcDS);

            _vertexArray.Load(Program, new[] { _positionBuffer, _textureBuffer });
            _vertexArray.UnBind();
        }

        public void Update(double time)
        {
            if (_invalidated)
            {
                _invalidated = false;
                _vertices.Clear();
                _vertices.Add(Position);
                _vertices.Add(new Vector2(Position.X + Size.X * Scale, Position.Y));
                _vertices.Add(Position + Size * Scale);
                _vertices.Add(Position + Size * Scale);
                _vertices.Add(new Vector2(Position.X, Position.Y + Size.Y * Scale));
                _vertices.Add(Position);

                _positionBuffer.Bind();
                _positionBuffer.SetData(_vertices.SelectMany(v => new[] { v.X, v.Y }), 0);
            }
        }

        public void Draw(double time)
        {
            SpriteTexture.Bind();
            _vertexArray.Draw(time);
        }

        public void Dispose()
        {
            if (SpriteTexture != null)
            {
                SpriteTexture.Dispose();
                SpriteTexture = null;
            }
        }
    }
}
