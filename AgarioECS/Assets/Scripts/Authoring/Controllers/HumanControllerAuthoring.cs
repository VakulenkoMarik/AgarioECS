using Unity.Entities;
using UnityEngine;

namespace Authoring.Controllers
{
    public class HumanControllerAuthoring : MonoBehaviour
    {
        public GameObject player;

        public class Baker : Baker<HumanControllerAuthoring>
        {
            public override void Bake(HumanControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new HumanController
                {
                    PlayerEntity = authoring.player != null ? GetEntity(authoring.player, TransformUsageFlags.Dynamic) : Entity.Null
                });
            }
        }
    }
    
    public struct HumanController : IComponentData
    {
        public Entity PlayerEntity;
    }
}
