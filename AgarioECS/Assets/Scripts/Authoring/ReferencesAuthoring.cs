using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ReferencesAuthoring : MonoBehaviour
    {
        [Header("-- Controllers --")]
        [SerializeField] private GameObject humanController;
        [SerializeField] private GameObject aiController;
        
        [Header("-- Players --")]
        [SerializeField] private GameObject inputPlayer;
        [SerializeField] private GameObject aiPlayer;
        
        [Header("-- Gameplay objects --")]
        [SerializeField] private GameObject food;
        
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
    }
}