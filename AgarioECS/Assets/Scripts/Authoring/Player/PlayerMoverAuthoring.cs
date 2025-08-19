using Unity.Entities;
using UnityEngine;

namespace Authoring.Player
{
    public class PlayerMoverAuthoring : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        public class Baker : Baker<PlayerMoverAuthoring>
        {
            public override void Bake(PlayerMoverAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerMove {
                    MoveSpeed = authoring.moveSpeed,
                    TargetPosition = new Vector3(0, 0, 0)
                });
            }
        }
    }

    public struct PlayerMove : IComponentData
    {
        public float MoveSpeed;
        public Vector3 TargetPosition;
    }
}
