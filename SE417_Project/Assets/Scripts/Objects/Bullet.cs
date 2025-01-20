using Player;
using UnityEngine;

namespace Objects
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private int damage;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Instantiate(hitEffect,other.transform.position,Quaternion.identity);
                other.gameObject.GetComponent<HealthController>().Damage(damage);
            }
            Destroy(gameObject,5f);
        }
    }
}
