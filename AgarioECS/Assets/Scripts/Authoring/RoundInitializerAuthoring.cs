using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Authoring
{
    public class RoundInitializerAuthoring : MonoBehaviour
    {
        [Header("-- Players settings --")]
        [SerializeField] private int playersAmount;
        [SerializeField] private float speedOfOnePointPlayers;
        
        [Header("-- Spawn --")]
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float spawnRadius;
        
        [Header("-- Gameplay settings --")]
        [SerializeField] private int maxFoodAmount;
        
        public class Baker : Baker<RoundInitializerAuthoring>
        {
            public override void Bake(RoundInitializerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new RoundInitializationData {
                    PlayersAmount = authoring.playersAmount,
                    SpeedOfOnePointPlayers = authoring.speedOfOnePointPlayers,
                    SpawnCenter = authoring.spawnCenter.position,
                    SpawnRadius = authoring.spawnRadius,
                    MaxFoodAmount = authoring.maxFoodAmount
                });
            }
        }
    }

    public struct RoundInitializationData : IComponentData
    {
        public float3 SpawnCenter;
        public float SpawnRadius;
        
        public int PlayersAmount;
        public float SpeedOfOnePointPlayers;

        public int MaxFoodAmount;
    }
}
