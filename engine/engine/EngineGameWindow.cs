using engine.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Engine
{
    internal class EngineGameWindow : GameWindow
    {
        internal SceneManager sceneManager;
        public EngineGameWindow(int width, int height) : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 16))
        {
            sceneManager = new SceneManager();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Cull back faces
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            //Enable depth testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            sceneManager.Width = Width;
            sceneManager.Height = Height;

            sceneManager.Start();

            GL.PointSize(4);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            sceneManager.Update(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            sceneManager.Render(e);

            GL.Flush();
            SwapBuffers();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            sceneManager.Width = Width;
            sceneManager.Height = Height;

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            //Cycle poly modes
            if(e.Key == Key.F1)
            {
                int polyModeInt;
                GL.GetInteger(GetPName.PolygonMode, out polyModeInt);
                Array polyVals = Enum.GetValues(typeof(PolygonMode));
                int min = (int)polyVals.GetValue(0);
                int max = (int)polyVals.GetValue(polyVals.Length-1);
                polyModeInt++;
                if (polyModeInt > max)
                    polyModeInt = min;
                GL.PolygonMode(MaterialFace.FrontAndBack, (PolygonMode)polyModeInt);
            }
        }

    }
}