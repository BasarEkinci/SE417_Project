using DG.Tweening;
using Player;
using UnityEngine;

namespace Objects
{
    public class Medkit : MonoBehaviour
    {
        [SerializeField] private int healAmount;
        private Tween _tween;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<HealthController>().Heal(healAmount);
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                transform.Rotate(Vector3.up * (100 * Time.deltaTime));
            }
        }
    }
}
