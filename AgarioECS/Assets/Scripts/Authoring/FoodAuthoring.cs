using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class FoodAuthoring : MonoBehaviour
    {
        public float kilogram;
        
        public class Baker : Baker<FoodAuthoring>
        {
            public override void Bake(FoodAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new Food {
                    Kilogram = authoring.kilogram
                });
            }
        }
    }

    public struct Food : IComponentData
    {
        public float Kilogram;
    }
}