using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Substructio.GUI
{
    public struct GUIComponentContainer
    {
        public Gwen.Renderer.OpenTK Renderer { get; private set; }
        public Gwen.Skin.Base Skin { get; private set; } 

        public GUIComponentContainer(Gwen.Renderer.OpenTK renderer, Gwen.Skin.Base skin) : this()
        {
            Renderer = renderer;
            Skin = skin;
        }

        public void Resize(Matrix4 projMatrix, int width, int height)
        {
            var renderMatrix = Matrix4.CreateTranslation(-width / 2.0f, -height / 2.0f, 0) * Matrix4.CreateScale(1, -1, 1) * projMatrix;
            Renderer.Resize(ref renderMatrix, width, height);
        }

    }
}
