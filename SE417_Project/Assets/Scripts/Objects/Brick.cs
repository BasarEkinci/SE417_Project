using DG.Tweening;
using Signals;
using UnityEngine;

namespace Objects
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private GameObject collectEffect;
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
                Instantiate(collectEffect, transform.position, Quaternion.identity);
                CoreGameSignals.Instance.OnCollectObject?.Invoke();
                transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}
