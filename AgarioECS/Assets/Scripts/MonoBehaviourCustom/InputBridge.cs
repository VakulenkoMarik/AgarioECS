using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehaviourCustom
{
    public class InputBridge : MonoBehaviour
    {
        private InputSystem_Actions _inputSystemActions;
        private EntityManager _entityManager;
        private Entity _inputEntity;

        private void Awake() {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Player.Enable();

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _inputEntity = _entityManager.CreateEntity(typeof(PlayerInputData));
        }

        private void OnEnable() {
            _inputSystemActions.Player.Interact.performed += OnInteract;
        }

        private void OnDisable() {
            _inputSystemActions.Player.Interact.performed -= OnInteract;
        }

        private void OnInteract(InputAction.CallbackContext obj) {
            float3 mouseWorldPos = MouseWorldPosition.Instance.GetPosition();

            _entityManager.SetComponentData(_inputEntity, new PlayerInputData {
                InteractPressed = true,
                MouseWorldPosition = mouseWorldPos
            });
        }

        private void LateUpdate() {
            if (_entityManager.Exists(_inputEntity)) {
                var data = _entityManager.GetComponentData<PlayerInputData>(_inputEntity);
                data.InteractPressed = false;
                _entityManager.SetComponentData(_inputEntity, data);
            }
        }
    }
    
    public struct PlayerInputData : IComponentData
    {
        public bool InteractPressed;
        public float3 MouseWorldPosition;
    }
}