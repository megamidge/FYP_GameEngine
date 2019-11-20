using System;
using engine.Managers;
using OpenTK;

namespace engine
{
    abstract class Scene : IScene
    {
        protected SceneManager sceneManager;

        internal SceneType sceneType;
        public Scene(SceneManager sceneManager)
        {
            sceneType = SceneType.NONE;
            this.sceneManager = sceneManager;
        }

        public abstract void Close();

        public abstract void Render(FrameEventArgs e);

        public abstract void Update(FrameEventArgs e);
    }
}