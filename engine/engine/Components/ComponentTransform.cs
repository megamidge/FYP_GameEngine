using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Components
{
    class ComponentTransform : IComponent
    {
        //Position, rotation and scale for all axis.
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public ComponentTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        public ComponentTransform(Vector3 position)
        {
            this.position = position;
            this.rotation = new Vector3(0, 0, 0);
            this.scale = new Vector3(1, 1, 1);
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public ComponentTypes ComponentType => ComponentTypes.COMP_TRANSFORM;
    }
}
