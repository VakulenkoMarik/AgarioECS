using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Authoring
{
    public class RoundInitializerAuthoring : MonoBehaviour
    {
        [SerializeField] private int enemiesAmount;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float speedOfOnePointPlayers;
        
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float spawnRadius;

        [SerializeField] private int maxFoodAmount;
        
        public class Baker : Baker<RoundInitializerAuthoring>
        {
            public override void Bake(RoundInitializerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new RoundInitializationData {
                    EnemiesAmount = authoring.enemiesAmount,
                    SpeedOfOnePointPlayers = authoring.speedOfOnePointPlayers,
                    EnemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
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
        
        public int EnemiesAmount;
        public Entity EnemyPrefab;
        public float SpeedOfOnePointPlayers;

        public int MaxFoodAmount;
    }
}
