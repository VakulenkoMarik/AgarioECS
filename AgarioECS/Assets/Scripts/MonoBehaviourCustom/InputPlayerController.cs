using Authoring;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehaviourCustom
{
    public class InputPlayerController : MonoBehaviour
    {
        private InputSystem_Actions _inputSystemActions;

        private void Awake() {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Player.Enable();
        }

        private void OnEnable() {
            _inputSystemActions.Player.Interact.performed += UpdateTargetPositions;
        }

        private void UpdateTargetPositions(InputAction.CallbackContext obj) {
            Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();

            EntityManager entityManager = World. DefaultGameObjectInjectionWorld. EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PlayerMover>().Build(entityManager);

            NativeArray<PlayerMover> unitMoverArray = entityQuery.ToComponentDataArray<PlayerMover>(Allocator.Temp);

            for (int i = 0; i < unitMoverArray.Length; i++) {
                PlayerMover unitMover = unitMoverArray[i];
                
                unitMover.TargetPosition = mouseWorldPosition;
                
                unitMoverArray[i] = unitMover;
            }
            
            entityQuery.CopyFromComponentDataArray(unitMoverArray);
        }
    }
}