using engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Components
{
    abstract class ComponentShape : IComponent
    {
        public abstract ComponentTypes ComponentType { get; }
        public int VertexBuffer => shapeInfo.vertexBuffer;
        public int ElementBuffer => shapeInfo.elementBuffer;
        public int ElementCount => shapeInfo.elementCount;

        protected ShapeManager.PolyMeta shapeInfo = new ShapeManager.PolyMeta();
    }
}
