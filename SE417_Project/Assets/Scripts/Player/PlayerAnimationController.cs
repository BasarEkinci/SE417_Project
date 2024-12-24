using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private PlayerMovementController _playerMovementController;
        private Animator _animator;

        private static readonly int IsInjured = Animator.StringToHash("IsInjured");
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
            _animator.SetBool(IsMoving, _playerMovementController.IsMoving);
            _animator.SetBool(IsJumping, _playerMovementController.IsJumping);
            _animator.SetBool(IsInjured, _playerMovementController.IsInjured);
        }
    }
}
