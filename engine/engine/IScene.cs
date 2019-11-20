using OpenTK;

namespace engine
{
    //Scene interface that all scenes must follow
    internal interface IScene
    {
        void Update(FrameEventArgs e);
        void Render(FrameEventArgs e);
        void Close();
    }

    public enum SceneType
    {
        NONE,
        INIT,
    }
}