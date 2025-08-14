using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ReferencesAuthoring : MonoBehaviour
    {
        public GameObject food;
        public float entitiesSpawnY;
        
        public class Baker : Baker<ReferencesAuthoring>
        {
            public override void Bake(ReferencesAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new References {
                    Food = GetEntity(authoring.food, TransformUsageFlags.None),
                    EntitiesSpawnY = authoring.entitiesSpawnY
                });
            }
        }
    }

    public struct References : IComponentData
    {
        public Entity Food;
        public float EntitiesSpawnY;
    }
}