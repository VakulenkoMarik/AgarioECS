using Unity.Entities;
using UnityEngine;

namespace Authoring.Player
{
    public class PlayerFeederAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerFeederAuthoring>
        {
            public override void Bake(PlayerFeederAuthoring authoring) {
                Entity entity = GetEntity(authoring, TransformUsageFlags.NonUniformScale);
                
                AddComponent(entity, new PlayerFat());
            }
        }
    }

    public struct PlayerFat : IComponentData
    {
        public int CurrentKilogramsValue;
        public int LastKilogramsValue;
    }
}
