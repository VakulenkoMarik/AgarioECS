using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ReferencesAuthoring : MonoBehaviour
    {
        [Header("-- Controllers --")]
        public GameObject humanController;
        public GameObject aiController;
        
        [Header("-- Players --")]
        public GameObject inputPlayer;
        public GameObject aiPlayer;
        
        [Header("-- Gameplay objects --")]
        public GameObject food;
        
        [Header("-- Global settings --")]
        public float entitiesSpawnY;
        
        public class Baker : Baker<ReferencesAuthoring>
        {
            public override void Bake(ReferencesAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new References {
                    Food = GetEntity(authoring.food, TransformUsageFlags.None),
                    InputPlayer = GetEntity(authoring.inputPlayer, TransformUsageFlags.None),
                    AIPlayer = GetEntity(authoring.aiPlayer, TransformUsageFlags.None),
                    HumanController = GetEntity(authoring.humanController, TransformUsageFlags.None),
                    AIController = GetEntity(authoring.aiController, TransformUsageFlags.None),
                    EntitiesSpawnY = authoring.entitiesSpawnY
                });
            }
        }
    }

    public struct References : IComponentData
    {
        public Entity Food;
        
        public Entity InputPlayer;
        public Entity AIPlayer;

        public Entity HumanController;
        public Entity AIController;
        
        public float EntitiesSpawnY;
    }
}