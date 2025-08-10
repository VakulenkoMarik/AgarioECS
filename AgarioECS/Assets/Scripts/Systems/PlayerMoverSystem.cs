using Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems
{
    partial struct PlayerMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((
                RefRO<LocalTransform> localTransform, 
                RefRO<PlayerMover> playerMoverAuthoring,
                RefRW<PhysicsVelocity> physicsVelocity) 
                in SystemAPI.Query<
                    RefRO<LocalTransform>, 
                    RefRO<PlayerMover>,
                    RefRW<PhysicsVelocity>>()) {

                float3 targetPosition = playerMoverAuthoring.ValueRO.TargetPosition;
                float3 moveDirection = GetNormalizedMoveDirection(localTransform, targetPosition);

                physicsVelocity.ValueRW.Linear = moveDirection * playerMoverAuthoring.ValueRO.MoveSpeed;
                physicsVelocity.ValueRW.Angular = float3.zero;
            }
        }

        private float3 GetNormalizedMoveDirection(RefRO<LocalTransform> transform, float3 targetPosition) {
            float3 moveDirection = targetPosition - transform.ValueRO.Position;

            moveDirection = math.normalize(moveDirection);
            moveDirection.y = 0;

            return moveDirection;
        }
    }
}
