using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Managers
{
    class EntityManager
    {
        private List<Entity> entityManifold;
        public List<Entity> Entities => entityManifold;
        public EntityManager()
        {
            entityManifold = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entityManifold.Add(entity);
        }                
    }
}
