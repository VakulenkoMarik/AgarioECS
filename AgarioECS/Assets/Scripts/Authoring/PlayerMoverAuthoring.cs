using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerMoverAuthoring : MonoBehaviour
    {
        public float moveSpeed;
        
        public class Baker : Baker<PlayerMoverAuthoring>
        {
            public override void Bake(PlayerMoverAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerMover {
                    MoveSpeed = authoring.moveSpeed,
                    TargetPosition = new Vector3(0, 0, 0)
                });
            }
        }
    }

    public struct PlayerMover : IComponentData
    {
        public float MoveSpeed;
        public Vector3 TargetPosition;
    }
}
