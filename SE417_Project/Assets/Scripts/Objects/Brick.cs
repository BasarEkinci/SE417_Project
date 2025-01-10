using DG.Tweening;
using Signals;
using UnityEngine;

namespace Objects
{
    public class Brick : MonoBehaviour
    {
        private Tween _tween;
        private void OnEnable()
        {
            _tween = transform.DORotate(Vector3.up * 360, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            _tween.Kill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerSignals.Instance.OnCollectObject?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
