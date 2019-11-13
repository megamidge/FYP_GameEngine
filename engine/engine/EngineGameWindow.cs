using OpenTK;
using OpenTK.Graphics.OpenGL;
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

            //Enable depth testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            sceneManager.Start();
            sceneManager.Width = Width;
            sceneManager.Height = Height;
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
        }
    }
}