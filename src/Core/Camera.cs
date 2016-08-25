using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Substructio.Core
{
    //TODO Refactor this class. Seriously this is atrocious.
    public class Camera
    {
        #region Member Variables

        private Vector2 m_OldMousePosition;
        private float m_OldMouseWheel;

        #endregion

        #region Properties

        public Polygon CameraBounds;
        public Polygon CameraBox;

        public Vector2 Center;
        public bool EditMode;
        public Vector2 InitialTranslation;

        public Vector2 MaximumScale;
        public Vector2 MinimumScale;
        public MouseDevice Mouse;
        public Vector2 MouseScreenDelta;

        public Vector2 MouseScreenPosition;
        public float MouseWheelDelta;
        public Vector2 MouseWorldDelta;
        public Vector2 MouseWorldPosition;
        public Polygon OriginalBounds;
        public Vector2 Scale;
        public Vector2 ScreenScale;
        public Matrix4 ScreenModelViewMatrix;
        public Matrix4 ScreenProjectionMatrix;
        public Vector2 ScreenSpaceMax;
        public Vector2 ScreenSpaceMin;
        public bool Split;
        public Vector2 TargetScale;
        public Vector2 TargetWorldTranslation;
        public Matrix4 WorldModelViewMatrix;
        public Matrix4 WorldProjectionMatrix;
        public Vector2 WorldTranslation;

        public float WorldZOffset = 0;

        public float PreferredWidth;
        public float PreferredHeight;
        public float WindowWidth;
        public float WindowHeight;

        public bool SplitScreen;

        public float ScaleDelta = 0.01f;
        public float ScaleChangeMultiplier = 10;
        public float ExtraScale = 0.0f;
        public Vector2 ScreenShake = Vector2.Zero;

        public bool HandleInput;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Camera(float prefWidth, float prefHeight, float windowWidth, float windowHeight, MouseDevice m)
        {
            OriginalBounds = CameraBounds = new Polygon();
            TargetScale = Scale = new Vector2(1f, 1f);
            MinimumScale = new Vector2(0.5f, 0.5f);
            MaximumScale = new Vector2(20, 20);
            CameraBox = new Polygon();
            Mouse = m;
            PreferredWidth = prefWidth;
            PreferredHeight = prefHeight;
            UpdateResize(windowWidth, windowHeight);


            UpdateProjectionMatrix();
        }

        public Matrix4 WorldModelViewProjection
        {
            get { return Matrix4.Mult(WorldModelViewMatrix, WorldProjectionMatrix); }
        }

        public Matrix4 ScreenModelViewProjection
        {
            get { return Matrix4.Mult(ScreenModelViewMatrix, ScreenProjectionMatrix); }
        }

        public void UpdateResize(float wWidth, float wHeight)
        {
            //ScreenSpaceMax = new Vector2(pWidth, pHeight);
            //PreferredWidth = pWidth;
            //PreferredHeight = pHeight;
            WindowWidth = wWidth;
            WindowHeight = wHeight;
            ScreenScale = new Vector2(WindowWidth/PreferredWidth, WindowHeight/PreferredHeight);
            //ScreenScale = new Vector2(PreferredWidth/WindowWidth, PreferredHeight/WindowHeight);
            //PreferredWidth = wWidth;
            //PreferredHeight = wHeight;

            UpdateProjectionMatrix();
        }

        #endregion

        #region Public Methods

        public void UpdateProjectionMatrix()
        {
            //WorldProjectionMatrix = Matrix4.CreateOrthographic(PreferredWidth * (Scale.X), PreferredHeight * (Scale.Y),
                                                               //-1000.0f, 1000.0f);
            WorldZOffset = -1000 - (float)(System.Math.Exp(Scale.X * 10) * 0.01);
            WorldProjectionMatrix = Matrix4.Mult(Matrix4.CreateTranslation(0, 0, WorldZOffset), Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, WindowWidth/WindowHeight, 0.1f, 1000000f));
            ScreenProjectionMatrix = Matrix4.CreateOrthographic(WindowWidth, WindowHeight, -10.0f, 10.0f);
        }

        public void UpdateModelViewMatrix()
        {
            ScreenShake = Vector2.Zero;
            if (System.Math.Abs(ExtraScale) > 0.15)
            {
                Vector2 direction = new Vector2(Utilities.RandomGenerator.Next(2) == 0 ? -1 : 1, Utilities.RandomGenerator.Next(2) == 0 ? -1 : 1);
                ScreenShake = Vector2.Multiply(direction, 1f);
            }
            Matrix4 trans = Matrix4.CreateTranslation(WorldTranslation.X + ScreenShake.X + 0.375f, WorldTranslation.Y + ScreenShake.Y + 0.375f, 0);
            WorldModelViewMatrix = Matrix4.Mult(Matrix4.Identity, trans);
            

            trans = Matrix4.CreateTranslation(0.375f, 0.375f, 0);
            ScreenModelViewMatrix = Matrix4.Mult(Matrix4.Identity, trans);
        }

        public void RotateX(float amount)
        {
            WorldModelViewMatrix = Matrix4.Mult(Matrix4.CreateRotationX(amount), WorldModelViewMatrix);
        }

        public void UpdateTargetTranslation()
        {
            //Initial translation
            InitialTranslation.Y = -(PreferredHeight / 2);
            InitialTranslation.X = -(PreferredWidth / 2);

            //Center the player in X axis
            TargetWorldTranslation.X = ((PreferredWidth/2) - Center.X);

            //Center the player in Y axis
            TargetWorldTranslation.Y = ((PreferredHeight/2) - Center.Y);

            TargetWorldTranslation += InitialTranslation;
        }

        private void ClampTranslations()
        {
            GenerateCameraBoundingBox();

            var bbox = new Polygon(CameraBox);
            bbox.Scale(TargetScale);
            bbox.BuildEdges();
            bbox.CalculateMaxMin();

            if (bbox.Width > CameraBounds.Width && CameraBounds.Width >= CameraBounds.Height)
            {
                TargetScale.X = CameraBounds.Width/(PreferredWidth);
            }
            if (bbox.Height > CameraBounds.Height && CameraBounds.Height >= CameraBounds.Width)
            {
                TargetScale.Y = CameraBounds.Height/(PreferredHeight);
            }

            if (TargetScale.Y > TargetScale.X)
            {
                TargetScale.Y = TargetScale.X;
            }
            else if (TargetScale.X > TargetScale.Y)
            {
                TargetScale.X = TargetScale.Y;
            }

            CameraBox.Scale(Scale);

            CameraBox.BuildEdges();
            CameraBox.CalculateMaxMin();

            //Clamp translation values
            if (CameraBox.Max.X > CameraBounds.Max.X)
            {
                TargetWorldTranslation.X += ((CameraBox.Max.X) - CameraBounds.Max.X);
            }
            else if (CameraBox.Min.X < CameraBounds.Min.X)
            {
                TargetWorldTranslation.X += ((CameraBox.Min.X) - CameraBounds.Min.X);
            }

            //Clamp translation values
            if (CameraBox.Max.Y > CameraBounds.Max.Y)
            {
                TargetWorldTranslation.Y += (CameraBox.Max.Y - CameraBounds.Max.Y);
            }
            else if (CameraBox.Min.Y < CameraBounds.Min.Y)
            {
                TargetWorldTranslation.Y += (CameraBox.Min.Y - CameraBounds.Min.Y);
            }
        }

        private void GenerateCameraBoundingBox()
        {
            CameraBox.Clear();

            CameraBox.Points.Add(new Vector2(-(PreferredWidth/2) - TargetWorldTranslation.X,
                                             -(PreferredHeight/2) - TargetWorldTranslation.Y));
            CameraBox.Points.Add(new Vector2((PreferredWidth/2) - TargetWorldTranslation.X,
                                             -(PreferredHeight/2) - TargetWorldTranslation.Y));
            CameraBox.Points.Add(new Vector2((PreferredWidth/2) - TargetWorldTranslation.X,
                                             (PreferredHeight/2) - TargetWorldTranslation.Y));
            CameraBox.Points.Add(new Vector2(-(PreferredWidth/2) - TargetWorldTranslation.X,
                                             (PreferredHeight/2) - TargetWorldTranslation.Y));
        }

        public void SnapToCenter()
        {
            UpdateTargetTranslation();
            ClampTranslations();
            WorldTranslation = TargetWorldTranslation;
        }

        public Vector2 UnProject(Vector2 vec)
        {
            var ret = UnProject(new Vector3(vec.X, vec.Y, WorldZOffset));
            return ret.Xy;
        }

        public Vector3 UnProject(Vector3 vec, bool world = true)
        {
            //Retrieve the inverse of the modelview and projection matrices
            var mat = new Matrix4();
            if (world)
            {
                mat = WorldModelViewProjection;
            }
            else
            {
                mat = ScreenModelViewProjection;
            }

            try
            {
                mat = Matrix4.Invert(mat);
            }
            catch (Exception)
            {
            }

            //Transform the mouse position, and return it as vector 2, no need for the z component
            vec = Vector3.TransformPosition(vec, mat);
            return vec;
        }

        public Vector2 UnProjectMouse(bool world = true)
        {
            //Retrive mouse position
            var mousePos = (new Vector3(Mouse.X, WindowHeight - Mouse.Y, 0));

            // Turn it into 0-1 screen coords 
            mousePos = Vector3.Multiply(mousePos, new Vector3(1.0f/WindowWidth, 1.0f/WindowHeight, 0));

            //And change that into -1 to 1 
            mousePos = (mousePos*2) - new Vector3(1, 1, 0);

            return UnProject(mousePos, world).Xy;
        }

        #endregion

        #region Private Methods

        #endregion

        public void Update(double time, bool editMode = false)
        {
            EditMode = editMode;
            UpdateMouse();

            ClampCenter();

            UpdateTargetTranslation();
            ClampScale();

            Scale = Vector2.Lerp(Scale, TargetScale, (float) time*ScaleChangeMultiplier);

            ClampTranslations();

            WorldTranslation = Vector2.Lerp(WorldTranslation, TargetWorldTranslation, (float) time*7.5f);

            if (System.Math.Abs(WorldTranslation.X) < 1)
            {
                WorldTranslation.X = 0;
            }
            if (System.Math.Abs(WorldTranslation.Y) < 1)
            {
                WorldTranslation.Y = 0;
            }

            //MouseWorldPosition = UnProjectMouse();
        }

        public void DrawBounds()
        {
            OriginalBounds.Draw();
        }


        private void ClampScale()
        {
            //Vector2 Max = Vector2.Max(new Vector2(MaximumScale.X, 0), new Vector2(0, MaximumScale.Y));
            Vector2.Clamp(ref TargetScale, ref MinimumScale, ref MaximumScale, out TargetScale);
        }

        private void ClampCenter()
        {
            //Clamp the center values
            if (Center.X > OriginalBounds.Max.X)
            {
                Center.X = OriginalBounds.Max.X;
            }
            else if (Center.X < OriginalBounds.Min.X)
            {
                Center.X = OriginalBounds.Min.X;
            }

            if (Center.Y > OriginalBounds.Max.Y)
            {
                Center.Y = OriginalBounds.Max.Y;
            }
            else if (Center.Y < OriginalBounds.Min.Y)
            {
                Center.Y = OriginalBounds.Min.Y;
            }
        }

        private void UpdateMouse()
        {
            Vector2 newMWorld = UnProjectMouse();
            MouseWorldDelta = newMWorld - MouseWorldPosition;
            if (EditMode && InputSystem.CurrentButtons.Contains(MouseButton.Right))
            {
                Center -= Vector2.Multiply(MouseWorldDelta, 3.5f);
            }
            MouseWorldPosition = newMWorld;

            Vector2 newMScreen = UnProjectMouse(false);
            MouseScreenDelta = newMScreen - MouseScreenPosition;
            MouseScreenPosition = newMScreen;

            if (HandleInput)
            {
                float wheel = Mouse.WheelPrecise;
                MouseWheelDelta = m_OldMouseWheel - wheel;

                if (InputSystem.CurrentKeys.Contains(Key.Down))
                {
                    TargetScale.X += ScaleDelta*0.5f;
                    TargetScale.Y += ScaleDelta*0.5f;
                    SnapToCenter();
                }
                else if (InputSystem.CurrentKeys.Contains(Key.Up))
                {
                    TargetScale.X -= ScaleDelta;
                    TargetScale.Y -= ScaleDelta;
                    SnapToCenter();
                }

                if (MouseWheelDelta != 0)
                {
                    //TargetScale.X += MouseWheelDelta * 0.1f;
                    //TargetScale.Y += MouseWheelDelta * 0.1f;
                    TargetScale.X += InputSystem.MouseWheelDelta*ScaleDelta;
                    TargetScale.Y += InputSystem.MouseWheelDelta*ScaleDelta;
                    SnapToCenter();
                }

                m_OldMouseWheel = wheel;
            }

            //var s = (float)PreferredWidth / (float)GameWindow.WindowWidth;
            //MouseWorldDelta = Vector2.Multiply(InputSystem.MouseDelta, s);
            //m_OldMousePosition = MouseWorldPosition;
        }

        public void UpdateCenter(Vector2 center)
        {
            Center = center;
        }
    }
}