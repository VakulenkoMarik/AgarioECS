using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class HungryAuthoring : MonoBehaviour
    {
        [SerializeField] private float eatingRadius;
        
        public class Baker : Baker<HungryAuthoring>
        {
            public override void Bake(HungryAuthoring authoring) {
                Entity entity = GetEntity(authoring, TransformUsageFlags.None);
                
                AddComponent(entity, new Hungry {
                    EatingRange = authoring.eatingRadius
                });
            }
        }
    }

    public struct Hungry : IComponentData
    {
        public float EatingRange;
    }
}