using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine
{
    [FlagsAttribute]
    enum ComponentTypes
    {
        COMP_NONE               = 0,
        COMP_TRANSFORM          = 1,
        COMP_GEOMETRY_2D        = 1 << 1,
        COMP_GEOMETRY_3D        = 1 << 2,
        COMP_COLOUR             = 1 << 3,
    }
    interface IComponent
    {
        ComponentTypes ComponentType { get; }
    }
}
