using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private MovementController _movementController;
        private HealthController _healthController;
        private Animator _animator;

        private static readonly int IsInjured = Animator.StringToHash("IsInjured");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _animator = GetComponentInChildren<Animator>();
            _healthController = GetComponent<HealthController>();
        }

        private void Update()
        {
            SetAnimationParameters();
        }

        private void SetAnimationParameters()
        {
            _animator.SetBool(IsMoving, _movementController.IsMoving);
            _animator.SetBool(IsJumping, _movementController.IsJumping);
            _animator.SetBool(IsInjured, _healthController.IsInjured);
        }
    }
}
