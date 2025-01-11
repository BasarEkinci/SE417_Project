using UnityEngine;

namespace Objects
{
    public class Medkit : MonoBehaviour
    {
        private void Update()
        {
            if (gameObject.activeSelf)
            {
                transform.Rotate(Vector3.up * (100 * Time.deltaTime));
            }
        }
    }
}
