using System;
using Substructio.Core;

namespace Substructio.GUI
{
    public abstract class Scene : IDisposable
    {
        public bool Exclusive = false;
        public bool Loaded = false;
        public SceneManager SceneManager;
        public Scene ParentScene;
        public bool Visible = true;
        public bool Removed = false;
        public bool Focused = false;

        public int WindowWidth {get { return SceneManager.Width; }}
        public int WindowHeight {get { return SceneManager.Height; }}

        public bool NeedsDisposing = false; 

        /// <summary>
        /// Load resources here
        /// Make sure to set Loaded to true after it, or you'll fuck things up
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// The regular callback that shall be used to process events from awesomium
        /// </summary>
        /// <param name="e">The callback arguments</param>
        public abstract void CallBack(GUICallbackEventArgs e);

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        public abstract void Resize(EventArgs e);

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="time">Contains timing information for framerate independent logic.</param>
        /// <param name="b"></param>
        public abstract void Update(double time, bool focused = false);

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="time">Contains timing information.</param>
        public abstract void Draw(double time);

        /// <summary>
        /// Unload and shutdown screen here
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// This method is called when the scene enters focus
        /// </summary>
        public abstract void EnterFocus();

        /// <summary>
        /// This method is called when the scene exits focus
        /// </summary>
        public abstract void ExitFocus();
    }
}