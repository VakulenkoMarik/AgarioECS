using Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct EatingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            var eatersEntities = SystemAPI.QueryBuilder()
                .WithAll<Hungry, Fat, LocalTransform>()
                .Build()
                .ToEntityArray(Allocator.Temp);

            var foodEntities = SystemAPI.QueryBuilder()
                .WithAll<Fat, LocalTransform>()
                .Build()
                .ToEntityArray(Allocator.Temp);

            foreach (var eater in eatersEntities) {
                ProcessEating(ref ecb, ref state, eater, foodEntities);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            eatersEntities.Dispose();
            foodEntities.Dispose();
        }

        private void ProcessEating(ref EntityCommandBuffer ecb, ref SystemState state, Entity eater, NativeArray<Entity> foodEntities) {
            var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(eater);
            var playerFat =  SystemAPI.GetComponentRW<Fat>(eater);
            var playerHungry =  SystemAPI.GetComponentRO<Hungry>(eater);
            
            foreach (var foodEntity in foodEntities) {
                if (foodEntity == eater)
                    continue;
                
                var foodTransform = SystemAPI.GetComponentRO<LocalTransform>(foodEntity);
                var foodFat = SystemAPI.GetComponentRO<Fat>(foodEntity);

                if (IsWithinEatingRange(playerTransform.ValueRO.Position, playerHungry.ValueRO.EatingRange, foodTransform.ValueRO.Position)) {
                    EatFood(ref ecb, foodEntity, foodFat, playerFat);
                }
            }
        }

        private bool IsWithinEatingRange(float3 playerPos, float radius, float3 foodPos) {
            float distanceSq = math.distancesq(playerPos, foodPos);
            return distanceSq <= radius * radius;
        }

        private void EatFood(ref EntityCommandBuffer ecb, Entity foodEntity, RefRO<Fat> foodFat, RefRW<Fat> eaterFat) {
            eaterFat.ValueRW.CurrentKilogramsValue += foodFat.ValueRO.CurrentKilogramsValue;
            ecb.DestroyEntity(foodEntity);
        }
    }
}
