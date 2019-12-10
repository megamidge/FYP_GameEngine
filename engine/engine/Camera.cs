using engine.Managers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine
{
    class Camera
    {
        private Vector3 cameraPosition;
        private Vector3 cameraDirection;
        private Vector3 cameraUp;
        private Vector3 targetPosition;
        public Matrix4 view;
        public Matrix4 projection;
        public Camera()
        {
            cameraPosition = new Vector3(0, 0, 800f);
            cameraDirection = new Vector3(0, 0, -1);
            cameraUp = new Vector3(0, 1, 0);

            UpdateView();

            UpdateProjection();            
        }

        private void UpdateView()
        {

            
            view = Matrix4.CreateTranslation(new Vector3(0,0, 0));
            targetPosition = cameraPosition + cameraDirection;
            view *= Matrix4.LookAt(cameraPosition, targetPosition, cameraUp);
        }
        public void UpdateProjection()
        {
            float aspect = Math.Max((float)SceneManager.Instance.Width, SceneManager.Instance.Height) / Math.Min((float)SceneManager.Instance.Width, (float)SceneManager.Instance.Height);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), aspect, 0.1f, 5000);
        }
    }
}
