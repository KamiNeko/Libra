using LibraCore.Components;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraCore.Systems
{
    public class DoorSystem : EntityProcessingSystem
    {
        public DoorSystem() : base(new Matcher().all(typeof(DoorComponent)))
        {
        }
        
        public override void process(Entity entity)
        {
            var doorComponent = entity.getComponent<DoorComponent>();
            var targetEntity = scene.Entities.findEntity(doorComponent.TargetEntityName);
            
            if (targetEntity == null)
            {
                entity.detachFromScene();
            }
        }
    }
}
