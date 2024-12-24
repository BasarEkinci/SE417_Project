using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerInputs _playerInputs;
        
        private void Awake()
        {
            _playerInputs = new PlayerInputs();
        }
        
        private void OnEnable()
        {
            _playerInputs.Player.Enable();
        }

        public bool GetJumpInput()
        {
            return _playerInputs.Player.Jump.triggered;
        }
        private void OnDisable()
        {
            _playerInputs.Player.Disable();
        }
        
        
        public Vector2 GetMoveInput()
        {
            return _playerInputs.Player.Move.ReadValue<Vector2>();
        }
    }
}