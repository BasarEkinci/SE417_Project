using UnityEngine;

namespace AI
{
    public class ZurgBot : AIController
    {
        private Animator _animator;
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        protected override void ActivateAI()
        {
            base.ActivateAI();
            _animator.enabled = true;
        }
        protected override void DeactivateAI()
        {
            base.DeactivateAI();
            _animator.enabled = false;
        }
        
    }
}