using Unity.Entities;
using UnityEngine;

namespace Authoring.Controllers
{
    public class AIControllerAuthoring : MonoBehaviour
    {
        public GameObject player;

        public class Baker : Baker<AIControllerAuthoring>
        {
            public override void Bake(AIControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new AIController
                {
                    PlayerEntity = authoring.player != null ? GetEntity(authoring.player, TransformUsageFlags.Dynamic) : Entity.Null
                });
            }
        }
    }
    
    public struct AIController : IComponentData
    {
        public Entity PlayerEntity;
    }
}