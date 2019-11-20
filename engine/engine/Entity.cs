using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine
{
    class Entity
    {
        public string Name { get; }
        public ComponentTypes ComponentMask { get; private set; }
        public List<IComponent> Components { get; } = new List<IComponent>();
        public Entity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds a component to the entity
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(IComponent component)
        {
            Components.Add(component);
            ComponentMask |= component.ComponentType;
        }


    }
}
