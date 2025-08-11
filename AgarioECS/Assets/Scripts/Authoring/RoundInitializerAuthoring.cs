using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Authoring
{
    public class RoundInitializerAuthoring : MonoBehaviour
    {
        [SerializeField] private int enemiesAmount;
        [SerializeField] private float speedOfOnePointPlayers;
        [SerializeField] private GameObject enemyPrefab;
        
        [SerializeField] private Transform playersSpawnCenter;
        [SerializeField] private float playersSpawnRadius;
        
        public class Baker : Baker<RoundInitializerAuthoring>
        {
            public override void Bake(RoundInitializerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new RoundInitializationData {
                    EnemiesAmount = authoring.enemiesAmount,
                    SpeedOfOnePointPlayers = authoring.speedOfOnePointPlayers,
                    EnemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                    SpawnCenter = authoring.playersSpawnCenter.position,
                    SpawnRadius = authoring.playersSpawnRadius
                });
            }
        }
    }

    public struct RoundInitializationData : IComponentData
    {
        public int EnemiesAmount;
        public float SpeedOfOnePointPlayers;
        public Entity EnemyPrefab;
        public float3 SpawnCenter;
        public float SpawnRadius;
    }
}
