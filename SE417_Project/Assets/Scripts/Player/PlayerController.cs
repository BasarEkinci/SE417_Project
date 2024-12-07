using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private int health = 100;
        [SerializeField] private float damageInterval = 1f;
        
        [Header("Player Movement")]
        [SerializeField] private float moveSpeed;

        private Rigidbody _rigidbody;
        private Vector3 _moveVector;
        private float _horizontalInput;
        private float _verticalInput;
        
        private bool _isTouchingEnemy;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            GetDamageFromEnemy().Forget(); 
        }

        private void Update()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _moveVector = new Vector3(_horizontalInput, 0, _verticalInput);
            
            //To make the player move faster when holding shift key to decrease the linear damping
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _rigidbody.linearDamping = 5;
            }
            else
            {
                _rigidbody.linearDamping = 10;
            }
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isTouchingEnemy = true;

            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isTouchingEnemy = false;
            }
        }

        private async UniTaskVoid GetDamageFromEnemy()
        {
            while (true)
            {
                if (health > 0 && _isTouchingEnemy)
                {
                    health -= 10;
                    Debug.Log(health);
                      
                }
                await UniTask.Delay(TimeSpan.FromSeconds(damageInterval));  
            }
        }
        private void Move()
        {
            _rigidbody.linearVelocity += _moveVector * moveSpeed;
        }
    }
}
