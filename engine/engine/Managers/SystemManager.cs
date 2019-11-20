using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using engine.Systems;

namespace engine.Managers
{
    class SystemManager
    {
        private List<ISystem> systemManifold = new List<ISystem>();
        public SystemManager()
        {

        }
        public void ActionSystems(EntityManager entityManager)
        {
            List<Entity> entitiesToAction = entityManager.Entities;
            foreach(ISystem system in systemManifold)
            {
                foreach (Entity entity in entitiesToAction)
                    system.Action(entity);
            }
        }

        public void AddSystem(ISystem system)
        {
            systemManifold.Add(system);
        }
    }

}
