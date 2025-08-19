using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class MoveAuthoring : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        public class Baker : Baker<MoveAuthoring>
        {
            public override void Bake(MoveAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new Move {
                    MoveSpeed = authoring.moveSpeed,
                    TargetPosition = new Vector3(0, 0, 0)
                });
            }
        }
    }

    public struct Move : IComponentData
    {
        public float MoveSpeed;
        public Vector3 TargetPosition;
    }
}
