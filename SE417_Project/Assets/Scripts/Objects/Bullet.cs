using Player;
using UnityEngine;

namespace Objects
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Instantiate(hitEffect,other.transform.position,Quaternion.identity);
                other.gameObject.GetComponent<HealthController>().Damage(1);
            }
            Destroy(gameObject,5f);
        }
    }
}
