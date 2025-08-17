using Authoring;
using Authoring.Controllers;
using MonoBehaviourCustom;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Systems
{
    [BurstCompile]
    public partial struct HumanControllerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;

            float3 mouseWorldPos = MouseWorldPosition.Instance.GetPosition();

            foreach (var humanController in SystemAPI.Query<RefRO<HumanController>>())
            {
                if (humanController.ValueRO.PlayerEntity != Entity.Null &&
                    state.EntityManager.HasComponent<PlayerMove>(humanController.ValueRO.PlayerEntity))
                {
                    var mover = state.EntityManager.GetComponentData<PlayerMove>(humanController.ValueRO.PlayerEntity);
                    mover.TargetPosition = mouseWorldPos;
                    state.EntityManager.SetComponentData(humanController.ValueRO.PlayerEntity, mover);
                }
            }
        }
    }
}
