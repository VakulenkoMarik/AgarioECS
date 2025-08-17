using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class CameraFollowTargetAuthoring : MonoBehaviour
    {
        public class Baker : Baker<CameraFollowTargetAuthoring>
        {
            public override void Bake(CameraFollowTargetAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new CameraFollowTarget());
                SetComponentEnabled<CameraFollowTarget>(entity, false);
            }
        }
    }

    public struct CameraFollowTarget : IComponentData, IEnableableComponent { }
}