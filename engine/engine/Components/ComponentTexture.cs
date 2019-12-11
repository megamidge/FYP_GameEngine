using engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Components
{
    internal class ComponentTexture : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMP_TEXTURE;

        internal int textureId;
        public ComponentTexture(string textureLocation)
        {
            textureId = TextureManager.LoadTexture(textureLocation);
        }
    }
}
