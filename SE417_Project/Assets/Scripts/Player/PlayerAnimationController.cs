using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private PlayerMovementController _playerMovementController;
        private Animator _animator;

        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private void Awake()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            SetAnimationParameters();
        }

        private void SetAnimationParameters()
        {
            _animator.SetFloat(MoveSpeed, _playerMovementController.MoveSpeed);
            _animator.SetBool(IsMoving, _playerMovementController.IsMoving);
            _animator.SetBool(IsJumping, _playerMovementController.IsJumping);
        }
    }
}
