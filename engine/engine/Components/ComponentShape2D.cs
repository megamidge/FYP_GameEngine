using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using engine.Managers;

namespace engine.Components
{
    class ComponentShape2D : ComponentShape
    {
        public override ComponentTypes ComponentType => ComponentTypes.COMP_GEOMETRY_2D;
        public ComponentShape2D(int sides, Vector2 size, bool centerIsZero = true)
        {
            shapeInfo = ShapeManager.MakeRegularPolygon(sides, size, centerIsZero);
        }        
    }
}
