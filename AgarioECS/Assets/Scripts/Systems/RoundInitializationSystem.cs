using Authoring;
using Authoring.Controllers;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    partial struct RoundInitializationSystem : ISystem
    {
        private References _references;
        private RoundInitializationData _roundData;

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<References>();
            state.RequireForUpdate<RoundInitializationData>();
        }

        public void OnUpdate(ref SystemState state) {
            InitializeFields();

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            InitializeCompetitors(ref ecb);

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            state.Enabled = false;
        }

        private void InitializeFields() {
            _references = SystemAPI.GetSingleton<References>();
            _roundData = SystemAPI.GetSingleton<RoundInitializationData>();
        }

        private void InitializeCompetitors(ref EntityCommandBuffer ecb) {
            InitializeInputController(ref ecb);
            InitializeAiControllers(ref ecb);
        }

        private void InitializeInputController(ref EntityCommandBuffer ecb) {
            float3 pos = new float3(0, _roundData.EntitiesSpawnY, 0);
            
            Entity player = CreateEntity(ref ecb, _references.InputPlayer, pos);
            Entity controller = CreateEntity(ref ecb, _references.HumanController, float3.zero);

            ecb.SetComponent(controller, new HumanController { PlayerEntity = player });
            
            ecb.SetComponentEnabled<CameraFollowTarget>(player, true);
        }

        private void InitializeAiControllers(ref EntityCommandBuffer ecb) {
            for (int i = 1; i < _roundData.PlayersAmount; i++) {
                float3 pos = GenerateRandomPosition(_roundData.SpawnCenter, _roundData.SpawnRadius);
                
                Entity player = CreateEntity(ref ecb, _references.AIPlayer, pos);
                Entity controller = CreateEntity(ref ecb, _references.AIController, float3.zero);

                ecb.SetComponent(controller, new AIController { PlayerEntity = player });
            }
        }

        private Entity CreateEntity(ref EntityCommandBuffer ecb, Entity prefab, float3 position) {
            Entity entity = ecb.Instantiate(prefab);
            SetTransform(ref ecb, entity, position);
            return entity;
        }

        private void SetTransform(ref EntityCommandBuffer ecb, Entity entity, float3 position) {
            ecb.SetComponent(entity, new LocalTransform {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 0.5f
            });
        }

        private float3 GenerateRandomPosition(float3 center, float radius) {
            var rand = Random.CreateFromIndex((uint)UnityEngine.Random.Range(1, int.MaxValue));
            float2 offset2D = rand.NextFloat2Direction() * UnityEngine.Random.Range(_roundData.MinInitialDistanceBetweenHumanAndAi, radius);

            return new float3(center.x + offset2D.x, _roundData.EntitiesSpawnY, center.z + offset2D.y);
        }
    }
}
