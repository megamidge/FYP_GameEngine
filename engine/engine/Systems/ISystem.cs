using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Systems
{
    interface ISystem
    {
        void Action(Entity entity);
        /// <summary>
        /// To identify system at runtime
        /// </summary>
        string Name { get; }
    }
}
