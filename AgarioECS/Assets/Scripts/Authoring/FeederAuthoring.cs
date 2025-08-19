using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class FeederAuthoring : MonoBehaviour
    {
        [SerializeField] private int currentKilogram;
        
        public class Baker : Baker<FeederAuthoring>
        {
            public override void Bake(FeederAuthoring authoring) {
                Entity entity = GetEntity(authoring, TransformUsageFlags.NonUniformScale);
                
                AddComponent(entity, new Fat {
                    CurrentKilogramsValue = authoring.currentKilogram
                });
            }
        }
    }

    public struct Fat : IComponentData
    {
        public int CurrentKilogramsValue;
        public int LastKilogramsValue;
    }
}
