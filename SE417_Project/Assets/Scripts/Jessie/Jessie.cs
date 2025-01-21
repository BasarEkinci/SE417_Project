using System;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;

namespace Jessie
{
    public class Jessie : MonoBehaviour
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CoreGameSignals.Instance.OnCompleteLevel?.Invoke();
                PlayVictoryAnimationAsync().Forget();
            }
        }
        private async UniTaskVoid PlayVictoryAnimationAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _animator.Play("Celebrate");
        }
    }
}
