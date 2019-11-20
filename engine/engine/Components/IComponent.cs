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
        COMP_GEOMETRY           = 1 << 1,
        COMP_COLOUR             = 1 << 2,
    }
    interface IComponent
    {
        ComponentTypes ComponentType { get; }
    }
}
