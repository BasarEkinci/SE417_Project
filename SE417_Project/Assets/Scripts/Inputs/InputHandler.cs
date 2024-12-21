using System;
using UnityEngine;

namespace Inputs
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerInputs _playerInputs;

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Player.Enable();
            _playerInputs.Player.Run.started += _ => IsRunning(true);
            _playerInputs.Player.Run.canceled += _ => IsRunning(false);
        }

        private void OnDisable()
        {
            _playerInputs.Player.Disable();
            _playerInputs.Player.Run.Disable();
        }

        public bool IsRunning(bool state)
        {
            return state;
        }

        public Vector2 GetMovementInput()
        {
            return _playerInputs.Player.Move.ReadValue<Vector2>();
        }
    }
}
