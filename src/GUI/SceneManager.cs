using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using QuickFont;
using Substructio.Core;
using Substructio.Core.Settings;

namespace Substructio.GUI
{
    public class SceneManager : IDisposable
    {
        public ValueWrapper<bool> Debug;

        private readonly List<Scene> _scenesToAdd;
        private readonly List<Scene> _scenesToRemove;

        private bool InputSceneFound;
        public List<Scene> SceneList;

        public DirectoryHandler Directories;

        public Camera ScreenCamera { get; private set; }
        public GameFont DefaultFont { get; private set; }
        public QFontDrawing FontDrawing { get; private set; }
        public GameWindow GameWindow { get; private set; }
        public string FontPath { get; private set; }

        private float _rotation;
        private int _direction = 1;

        public int Width {get { return GameWindow.Width; }}
        public int Height {get { return GameWindow.Height; }}

        public IGameSettings GameSettings;

        public FontLibrary GameFontLibrary;

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public SceneManager(GameWindow gameWindow, Camera camera, FontLibrary fontLibrary, string fontPath, DirectoryHandler directoryHandler, IGameSettings gameSettings, ValueWrapper<bool> debug)
        {
            GameWindow = gameWindow;
            SceneList = new List<Scene>();
            _scenesToAdd = new List<Scene>();
            _scenesToRemove = new List<Scene>();

            Directories = directoryHandler;

            FontPath = fontPath;
            GameFontLibrary = fontLibrary;
            DefaultFont = GameFontLibrary.GetFirstOrDefault(GameFontType.Default);
            FontDrawing = new QFontDrawing();
            FontDrawing.ProjectionMatrix = camera.ScreenProjectionMatrix;

            ScreenCamera = camera;
            ScreenCamera.Center = Vector2.Zero;
            ScreenCamera.MaximumScale = new Vector2(10000, 10000);

            GameSettings = gameSettings;

            Debug = debug;
        }

        public void Draw(double time)
        {
            FontDrawing.DrawingPrimitives.Clear();
            Scene excl = SceneList.Where(scene => scene.Visible && !scene.Removed).FirstOrDefault(scene => scene.Exclusive);
            if (excl == null)
            {
                foreach (Scene scene in SceneList.Where(screen => screen.Visible && !screen.Removed))
                {
                    scene.Draw(time);
                }
            }
            else
            {
                excl.Draw(time);
            }
            FontDrawing.RefreshBuffers();
            FontDrawing.Draw();
        }

        public void Update(double time)
        {
            AddRemoveScenes();

            ScreenCamera.Update(time);
            ScreenCamera.SnapToCenter();
            ScreenCamera.UpdateProjectionMatrix();
            ScreenCamera.UpdateModelViewMatrix();

            var list = SceneList.Where(scene => !scene.Loaded).ToArray();
            foreach (var scene in list)
            {
                scene.Load();
            }
            Scene excl = SceneList.Where(scene => scene.Visible && !scene.Removed).FirstOrDefault(scene => scene.Exclusive);
            if (excl == null)
            {
                for (int i = SceneList.Count - 1; i >= 0; i--)
                {
                    if (SceneList[i].Removed) continue;
                    if (!InputSceneFound && SceneList[i].Visible)
                    {
                        SceneList[i].Update(time, true);
                        InputSceneFound = true;
                    }
                    else
                    {
                        SceneList[i].Update(time);
                    }
                }
            }
            else
                excl.Update(time, true);
            InputSceneFound = false;
        }

        public SizeF DrawTextLine(string text, Vector3 position, Color4 colour, QFontAlignment alignment = QFontAlignment.Centre, QFont font = null)
        {
            if (font == null) font = DefaultFont.Font;
            return FontDrawing.Print(font, text, position, alignment, (Color)colour);
        }

        public SizeF DrawProcessedText(ProcessedText pText, Vector3 position, Color4 colour)
        {
            return FontDrawing.Print(DefaultFont.Font, pText, position, (Color)colour);
        }

        private void AddRemoveScenes()
        {
            foreach (Scene scene in _scenesToRemove)
            {
                SceneList.Remove(scene);
                scene.Removed = true;
                if (scene.NeedsDisposing) scene.Dispose();
            }

            foreach (Scene scene in _scenesToAdd)
            {
                SceneList.Add(scene);
            }

            _scenesToAdd.Clear();
            _scenesToRemove.Clear();
        }

        public void Resize(EventArgs e)
        {
            ScreenCamera.UpdateResize(GameWindow.Width, GameWindow.Height);
            FontDrawing.ProjectionMatrix = ScreenCamera.ScreenProjectionMatrix;
            foreach (Scene scene in SceneList)
            {
                scene.Resize(e);
            }
        }

        public void AddScene(Scene s, Scene parent)
        {
            s.SceneManager = this;
            s.ParentScene = parent;
            _scenesToAdd.Add(s);
        }

        public void RemoveScene(Scene s, bool dispose=false)
        {
            s.Removed = true;
            s.NeedsDisposing = dispose;
            _scenesToRemove.Add(s);
        }

        public void Dispose()
        {
            foreach (Scene scene in SceneList)
            {
                scene.Dispose();
            }
            SceneList.Clear();
            DefaultFont.Font.Dispose();
            FontDrawing.Dispose();
        }
    }
}