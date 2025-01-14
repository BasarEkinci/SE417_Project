using Extensions;
using UnityEngine;

namespace Inputs
{
    public class InputHandler : MonoSingleton<InputHandler>
    {
        public PlayerInputs PlayerInputs => _playerInputs;
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
        public Vector2 GetMoveInput()
        {
            return _playerInputs.Player.Move.ReadValue<Vector2>();
        }
    }
}