using engine.Managers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Components
{
    /// <summary>
    /// For computed 3D geometry. Use [TO-DO: Geometry from file] component for loaded geometry.
    /// </summary>
    class ComponentShape3D : ComponentShape
    {
        public override ComponentTypes ComponentType => ComponentTypes.COMP_GEOMETRY_3D;
        public ComponentShape3D(Vector3 size)
        {
            shapeInfo = ShapeManager.MakeCube(size);
        }
    }
}
