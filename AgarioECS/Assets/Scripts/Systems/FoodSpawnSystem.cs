using Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    partial struct FoodSpawnSystem : ISystem
    {
        private Random _random;
        private int _currentFoodsAmount;

        public void OnCreate(ref SystemState state) {
            InitRequiresForUpdate(ref state);
            InitRandom();
        }

        private void InitRequiresForUpdate(ref SystemState state) {
            state.RequireForUpdate<References>();
        }

        private void InitRandom() {
            uint seedRandom = (uint)UnityEngine.Random.Range(0, 1000);
            _random = Random.CreateFromIndex(seedRandom);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            if (!SystemAPI.TryGetSingleton<RoundInitializationData>(out var roundData))
                return;

            CheckCurrentFoodsAmount(ref state);

            if (NeedToSpawnFoods(roundData.MaxFoodAmount)) {
                SpawnFoods(ref state, roundData);
            }
        }

        private void CheckCurrentFoodsAmount(ref SystemState state) {
            _currentFoodsAmount = SystemAPI.QueryBuilder().WithAll<Food>().Build().CalculateEntityCount();
        }

        private bool NeedToSpawnFoods(int maxFoodsAmount) {
            return _currentFoodsAmount < maxFoodsAmount;
        }

        private void SpawnFoods(ref SystemState state, RoundInitializationData roundData) {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var refs = SystemAPI.GetSingleton<References>();

            for (int i = _currentFoodsAmount; i < roundData.MaxFoodAmount; i++) {
                Entity newFood = ecb.Instantiate(refs.Food);
                float3 spawnPos = GetSpawnPosition(roundData.SpawnRadius, refs.EntitiesSpawnY);

                ecb.SetComponent(newFood, LocalTransform.FromPosition(spawnPos));
            }
                
            EcbDispose(ref ecb, ref state);
        }

        private float3 GetSpawnPosition(float spawnRadius, float spawnY) {
            float2 offset = _random.NextFloat2Direction() * _random.NextFloat(0f, spawnRadius);
            
            float3 spawnPos = new float3(offset.x, spawnY, offset.y);

            return spawnPos;
        }
        
        private void EcbDispose(ref EntityCommandBuffer ecb, ref SystemState state) {
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}