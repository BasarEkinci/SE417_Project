using DG.Tweening;
using UnityEngine;

namespace Objects
{
    public class Medkit : MonoBehaviour
    {
        [SerializeField] private int healAmount;
        private Tween _tween;
        private void OnEnable()
        {
            _tween = transform.DORotate(Vector3.up * 360, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player.HealthController>().Heal(healAmount);
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
    }
}
