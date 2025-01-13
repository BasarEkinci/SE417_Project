using Extensions;
using UnityEngine;

namespace Inputs
{
    public class InputHandler : MonoSingleton<InputHandler>
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
        private void OnDisable()
        {
            _playerInputs.Player.Disable();
        }
        public bool GetJumpInput()
        {
            return _playerInputs.Player.Jump.triggered;
        }
        public bool GetHideInput()
        {
            return _playerInputs.Player.Hide.triggered;
        }
        
        public bool GetHealInput()
        {
            return _playerInputs.Player.Heal.triggered;
        }
        
        public bool GetCrouchInput()
        {
            return _playerInputs.Player.Crouch.triggered;
        }
        
        public Vector2 GetMoveInput()
        {
            return _playerInputs.Player.Move.ReadValue<Vector2>();
        }
    }
}