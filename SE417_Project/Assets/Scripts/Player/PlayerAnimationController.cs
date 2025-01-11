using Signals;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private PlayerController _playerController;
        private HealthController _healthController;
        private Animator _animator;

        private static readonly int IsInjured = Animator.StringToHash("IsInjured");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsStandingUp = Animator.StringToHash("IsStandingUp");
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _animator = GetComponentInChildren<Animator>();
            _healthController = GetComponent<HealthController>();
        }

        public void OnEnable()
        {
            CoreGameSignals.Instance.OnPlayerHide += Fall;
            CoreGameSignals.Instance.OnPlayerWakeUp += StandUp;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerHide -= Fall;
            CoreGameSignals.Instance.OnPlayerWakeUp -= StandUp;
        }

        private void Update()
        {
            SetAnimationParameters();
        }

        private void SetAnimationParameters()
        {
            _animator.SetBool(IsMoving, _playerController.IsMoving);
            _animator.SetBool(IsJumping, _playerController.IsJumping);
            _animator.SetBool(IsInjured, _healthController.IsInjured);
        }
        
        private void Fall()
        {
            _animator.Play("Fall");
            _animator.SetBool(IsStandingUp,false);
        }

        private void StandUp()
        {
            _animator.SetBool(IsStandingUp,true);
        }
    }
}
