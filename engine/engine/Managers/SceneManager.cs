using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Audio;

namespace engine.Managers
{
    class SceneManager
    {
        private Scene currentScene;

        internal delegate void SceneEventDel(FrameEventArgs e);
        internal SceneEventDel UpdateEvent;
        internal SceneEventDel RenderEvent;

        // TO-DO: Improve on system for things to fetch height/width.
        public static SceneManager Instance {get; private set;}
        public int Height { get; internal set; }
        public int Width { get; internal set; }

        internal SceneManager()
        {
            Instance = this;
        }                
        internal void Update(FrameEventArgs e)
        {
            UpdateEvent(e);
        }

        internal void Render(FrameEventArgs e)
        {
            RenderEvent(e);
        }

        internal void Start()
        {
            currentScene = new DefaultScene(this);
        }
    }
}
