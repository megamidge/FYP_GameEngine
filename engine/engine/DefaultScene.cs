using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
namespace Engine
{
    class DefaultScene : Scene
    {

        private float clearRed, clearGreen, clearBlue;
        public DefaultScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneType = SceneType.INIT;

            sceneManager.RenderEvent = Render;
            sceneManager.UpdateEvent = Update;

            clearRed = 1f;
            clearBlue = 0f;
            clearGreen = 0f;
        }
        public override void Close()
        {
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);

            GL.ClearColor(clearRed, clearGreen, clearBlue, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

        }

        private float time = 0;
        public override void Update(FrameEventArgs e)
        {
            time += (float)e.Time;
            clearRed = 0.2f + (0.2f * (float)(Math.Sin(time * 0.5f)));
            clearBlue = 0.2f + (0.2f * (float)(Math.Sin(time * 0.8f)));
            clearGreen = 0.2f + (0.2f * (float)(Math.Sin(time * 1.1f)));
        }
    }
}
