using Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    partial struct PlayerGrowthSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((
                RefRW<Fat> playerFat,
                RefRW<PostTransformMatrix> postTransformMatrix)
                in SystemAPI.Query<
                    RefRW<Fat>,
                    RefRW<PostTransformMatrix>>()
                    .WithAll<Move>()) {

                if (NeedChangeScale(playerFat)) {
                    float radius = GetRadius(playerFat.ValueRO.CurrentKilogramsValue);

                    SetNewScale(postTransformMatrix, radius);
                    
                    UpdatePlayerFatValue(playerFat);
                }
            }
        }

        private bool NeedChangeScale(RefRW<Fat>  playerFat) {
            return playerFat.ValueRO.CurrentKilogramsValue != playerFat.ValueRO.LastKilogramsValue;
        }

        private float GetRadius(float mass, float scale = 0.5f) {
            return math.sqrt(mass / math.PI) * scale;
        }
        
        private void SetNewScale(RefRW<PostTransformMatrix> localTransform, float radius) {
            localTransform.ValueRW.Value = float4x4.TRS(
                float3.zero,
                quaternion.identity,
                new float3(radius, 0.5f, radius)
            );
        }

        private void UpdatePlayerFatValue(RefRW<Fat> playerFat) {
            playerFat.ValueRW.LastKilogramsValue = playerFat.ValueRO.CurrentKilogramsValue;
        }
    }
}
