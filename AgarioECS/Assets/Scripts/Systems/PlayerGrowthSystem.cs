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
                RefRW<Hungry> playerHungry,
                RefRW<PostTransformMatrix> postTransformMatrix)
                in SystemAPI.Query<
                    RefRW<Fat>,
                    RefRW<Hungry>,
                    RefRW<PostTransformMatrix>>()
                    .WithAll<Move>()) {

                if (NeedChangeScale(playerFat)) {
                    float diameter = GetDiameter(playerFat.ValueRO.CurrentKilogramsValue);

                    SetNewScale(postTransformMatrix, diameter);
                    SetNewEatingRadius(playerHungry, diameter);
                    
                    UpdatePlayerFatValue(playerFat);
                }
            }
        }

        private bool NeedChangeScale(RefRW<Fat>  playerFat) {
            return playerFat.ValueRO.CurrentKilogramsValue != playerFat.ValueRO.LastKilogramsValue;
        }

        private float GetDiameter(float mass, float scale = 0.5f) {
            return math.sqrt(mass / math.PI) * scale;
        }
        
        private void SetNewScale(RefRW<PostTransformMatrix> localTransform, float diameter) {
            localTransform.ValueRW.Value = float4x4.TRS(
                float3.zero,
                quaternion.identity,
                new float3(diameter, 0.5f, diameter)
            );
        }
        
        private void SetNewEatingRadius(RefRW<Hungry> hungryComponent, float diameter) {
            hungryComponent.ValueRW.EatingRange = diameter / 2;
        }

        private void UpdatePlayerFatValue(RefRW<Fat> playerFat) {
            playerFat.ValueRW.LastKilogramsValue = playerFat.ValueRO.CurrentKilogramsValue;
        }
    }
}
