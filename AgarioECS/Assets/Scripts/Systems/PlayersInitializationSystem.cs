using Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    partial struct PlayersInitializationSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<RoundInitializationData>();
        }
        
        public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            if (SystemAPI.TryGetSingleton<RoundInitializationData>(out var roundData)) {
                SpawnEnemies(ref ecb, ref roundData);
            }
            
            Dispose(ref ecb, ref state);
        }

        private void SpawnEnemies(ref EntityCommandBuffer ecb, ref RoundInitializationData roundData) {
            for (int i = 0; i < roundData.EnemiesAmount; i++) {
                CreateEntity(ref ecb, ref roundData);
            }
        }

        private Entity CreateEntity(ref EntityCommandBuffer ecb, ref RoundInitializationData roundData) {
            Entity newEntity = ecb.Instantiate(roundData.EnemyPrefab);
                
            float2 randomOffset2D = Random.CreateFromIndex((uint)UnityEngine.Random.Range(1, 100000))
                .NextFloat2Direction() * UnityEngine.Random.Range(0f, roundData.SpawnRadius);

            float3 randomPosition = new float3(
                roundData.SpawnCenter.x + randomOffset2D.x,
                roundData.SpawnCenter.y,
                roundData.SpawnCenter.z + randomOffset2D.y
            );

            ecb.SetComponent(newEntity, new LocalTransform {
                Position = randomPosition,
                Rotation = quaternion.identity,
                Scale = 0.5f
            });

            return newEntity;
        }

        private void Dispose(ref EntityCommandBuffer ecb, ref SystemState state) {
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
            state.Enabled = false;
        }
    }
}
