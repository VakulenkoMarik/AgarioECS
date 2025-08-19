using Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    partial struct CameraFollowSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<RoundInitializationData>();
        }

        public void OnUpdate(ref SystemState state) {
            foreach ((
                RefRO<CameraFollowTarget> targetTag,
                Entity targetEntity) 
                in SystemAPI.Query<
                    RefRO<CameraFollowTarget>>()
                    .WithEntityAccess()) {

                DetermineCameraPosition(ref state, targetEntity);
            }
        }

        private void DetermineCameraPosition(ref SystemState state, Entity targetEntity) {
            Transform cameraTransform = Camera.main.transform;

            RefRO<LocalTransform> localTransform = SystemAPI.GetComponentRO<LocalTransform>(targetEntity);
            
            float3 targetPosition = localTransform.ValueRO.Position;
            float3 smoothedPosition = GetNextStepPosition(ref state, targetPosition, cameraTransform);

            cameraTransform.position = smoothedPosition;
            Camera.main.orthographicSize = GetNextCameraSize(ref state, targetEntity);
        }

        private float GetNextCameraSize(ref SystemState state, Entity targetEntity) {
            var roundData = SystemAPI.GetSingleton<RoundInitializationData>();
            float scaleMultiplier = 1;

            if (SystemAPI.HasComponent<Fat>(targetEntity)) {
                scaleMultiplier = SystemAPI.GetComponentRO<Fat>(targetEntity).ValueRO.CurrentKilogramsValue;
            }

            float targetSize = math.sqrt(scaleMultiplier) * roundData.CameraYOffset;
            float followSpeed = roundData.SpeedOfCameraFollow;
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            targetSize = math.clamp(targetSize, roundData.MinCameraYSize, 10000);

            return math.lerp(Camera.main.orthographicSize, targetSize, deltaTime * followSpeed);
        }

        private float3 GetNextStepPosition(ref SystemState state, float3 targetPosition, Transform currentCameraTransform) {
            var roundData = SystemAPI.GetSingleton<RoundInitializationData>();
            float3 currentPosition = currentCameraTransform.position;

            targetPosition.y = currentPosition.y;

            float followSpeed = roundData.SpeedOfCameraFollow;
            float deltaTime = SystemAPI.Time.DeltaTime;

            return math.lerp(currentPosition, targetPosition, deltaTime * followSpeed);
        }
    }
}