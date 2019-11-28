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
    class ComponentShape2D : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMP_GEOMETRY;

        private bool centerIsZero;

        private float[] vertices;
        private uint[] indices;

        private int[] mVertexBufferObjectIDArray = new int[2];
        public int VertexBuffer => shapeInfo.vertexBuffer;
        public int ElementBuffer => shapeInfo.elementBuffer;
        public int ElementCount => shapeInfo.elementCount;

        private Shape2DManager.PolyMeta shapeInfo = new Shape2DManager.PolyMeta();
        public ComponentShape2D(int sides, Vector2 size, bool centerIsZero = true)
        {
            this.centerIsZero = centerIsZero;
            shapeInfo = Shape2DManager.MakeRegularPolygon(sides, size, centerIsZero);
        }        
    }
}
