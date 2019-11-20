using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Components
{
    class ComponentColour : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMP_COLOUR;

        private Vector4 colour;
        public Vector4 Colour => colour;
        public ComponentColour(Vector4 colour)
        {
            this.colour = colour;
        }
    }
}
