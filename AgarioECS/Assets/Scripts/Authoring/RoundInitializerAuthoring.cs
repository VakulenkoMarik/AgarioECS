using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Authoring
{
    public class RoundInitializerAuthoring : MonoBehaviour
    {
        [Header("-- Players settings --")]
        [SerializeField] private int playersAmount;
        [SerializeField] private float speedOfOnePointPlayers;
        [SerializeField] private int startInputPlayerKilogramValue;
        [SerializeField] private int2 minMaxStartAiKilogramValue;
        
        [Header("-- Spawn --")]
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float spawnRadius;
        
        [Header("-- Gameplay settings --")]
        [SerializeField] private int maxFoodAmount;
        [SerializeField] private float entitiesSpawnY;
        [SerializeField] private float minInitialDistanceBetweenHumanAndAi;
        
        [Header("-- Camera --")]
        [SerializeField] private float speedOfCameraFollow;
        [SerializeField] private float cameraYOffset;
        [SerializeField] private float minCameraYSize;
        
        public class Baker : Baker<RoundInitializerAuthoring>
        {
            public override void Bake(RoundInitializerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new RoundInitializationData {
                    PlayersAmount = authoring.playersAmount,
                    SpeedOfOnePointPlayers = authoring.speedOfOnePointPlayers,
                    SpawnCenter = authoring.spawnCenter.position,
                    SpawnRadius = authoring.spawnRadius,
                    MaxFoodAmount = authoring.maxFoodAmount,
                    EntitiesSpawnY = authoring.entitiesSpawnY,
                    MinInitialDistanceBetweenHumanAndAi = authoring.minInitialDistanceBetweenHumanAndAi,
                    SpeedOfCameraFollow = authoring.speedOfCameraFollow,
                    StartInputPlayerKilogramValue = authoring.startInputPlayerKilogramValue,
                    MinMaxStartAiKilogramValue = authoring.minMaxStartAiKilogramValue,
                    CameraYOffset = authoring.cameraYOffset,
                    MinCameraYSize = authoring.minCameraYSize,
                });
            }
        }
    }

    public struct RoundInitializationData : IComponentData
    {
        public float3 SpawnCenter;
        public float SpawnRadius;
        public float EntitiesSpawnY;
        
        public int PlayersAmount;
        public int StartInputPlayerKilogramValue;
        public int2 MinMaxStartAiKilogramValue;
        public float SpeedOfOnePointPlayers;
        public float MinInitialDistanceBetweenHumanAndAi;

        public int MaxFoodAmount;
        
        public float SpeedOfCameraFollow;
        public float CameraYOffset;
        public float MinCameraYSize;
    }
}
