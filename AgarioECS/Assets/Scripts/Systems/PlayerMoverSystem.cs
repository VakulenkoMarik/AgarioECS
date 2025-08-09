using Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    partial struct PlayerMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((
                RefRW<LocalTransform> localTransform, 
                RefRO<PlayerMover> playerMoverAuthoring) 
                in SystemAPI.Query<
                    RefRW<LocalTransform>, 
                    RefRO<PlayerMover>>()) {

                Vector3 playerTargetPosition = playerMoverAuthoring.ValueRO.TargetPosition;
                
                float3 newTargetPosition = new float3(playerTargetPosition.x, playerTargetPosition.y, playerTargetPosition.z);
                float3 moveDirection = newTargetPosition - localTransform.ValueRO.Position;
                
                float distance = math.length(moveDirection);

                if (distance <= PlayerMoverAuthoring.STOPPING_DISTANCE) {
                    localTransform.ValueRW.Position = newTargetPosition;
                    continue; 
                }

                if (!moveDirection.Equals(float3.zero))
                {
                    moveDirection = math.normalize(moveDirection);
                    localTransform.ValueRW.Position += moveDirection * playerMoverAuthoring.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
                }
            }
        }
    }
}
