using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        #region Public Properties
        public float MoveSpeed => moveVector.magnitude;
        public bool IsMoving => moveVector.magnitude > 0;
        #endregion

        #region Class References
        private PlayerInputs _playerInputs;
        private Rigidbody _rigidbody;
        #endregion

        #region Variables
        private Vector2 moveVector;
        #endregion

        #region Unity Functions

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnEnable()
        {
            _playerInputs.Player.Enable();
            _playerInputs.Player.Run.started += OnRun;
            _playerInputs.Player.Run.canceled += OnRun;
        }

        private void Update()
        {
            // Rotate player to face movement direction
            if (moveVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }


        private void OnDisable()
        {
            _playerInputs.Player.Run.started -= OnRun;
            _playerInputs.Player.Run.canceled -= OnRun;
            _playerInputs.Disable();
        }
        #endregion

        #region Custom Functions
        //This function controls the run key pressed or released : "Shift"
        private void OnRun(InputAction.CallbackContext context)
        {
        }

        //This function controls the movement of the player. It takes the input and sets the velocity of the rigidbody.
        private void MovePlayer()
        {
            moveVector = _playerInputs.Player.Move.ReadValue<Vector2>();
            //_rigidbody.linearVelocity = new Vector3(moveVector.x, _rigidbody.linearVelocity.y, moveVector.y) * moveSpeed;
        }
        #endregion
    }
}