using Authoring;
using Authoring.Controllers;
using MonoBehaviourCustom;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [BurstCompile]
    public partial struct HumanControllerSystem : ISystem
    {
        private float3 _lastPosition;
        
        public void OnUpdate(ref SystemState state) {
            if (NeedChangePlayerPositions(out var inputData)) {
                SetUpNewPosition(inputData);
                
                ChangePlayersPositions(ref state);
            }
        }

        private bool NeedChangePlayerPositions(out PlayerInputData inputData) {
            return SystemAPI.TryGetSingleton(out inputData) && inputData.InteractPressed;
        }

        private void SetUpNewPosition(PlayerInputData inputData) {
            _lastPosition = inputData.MouseWorldPosition;
        }

        private void ChangePlayersPositions(ref SystemState state) {
            foreach (var humanController in SystemAPI.Query<RefRO<HumanController>>()) {
                NewPositionForPlayerProcess(ref state, humanController);
            }
        }

        private void NewPositionForPlayerProcess(ref SystemState state, RefRO<HumanController> humanController) {
            if (IsValidController(ref state, humanController)) {
                var mover = state.EntityManager.GetComponentData<Move>(humanController.ValueRO.PlayerEntity);
                mover.TargetPosition = _lastPosition;
                state.EntityManager.SetComponentData(humanController.ValueRO.PlayerEntity, mover);
            }
        }

        private bool IsValidController(ref SystemState state, RefRO<HumanController> humanController) {
            return humanController.ValueRO.PlayerEntity != Entity.Null &&
                   state.EntityManager.HasComponent<Move>(humanController.ValueRO.PlayerEntity);
        }
    }
}
