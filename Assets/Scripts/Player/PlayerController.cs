using System;
using UnityEngine;
using Mirror;

namespace sknco.prisongame
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float sprintMultiplier = 1.5f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;
        public float lookSpeed = 1f;

        private CharacterController characterController;
        private PlayerInputHandler inputHandler;
        [SerializeField] Transform cameraPivot;

        private Vector3 velocity;
        private float xRotation = 0f;

        public override void OnStartLocalPlayer()
        {
            characterController = GetComponent<CharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();

            cameraPivot = GetComponentInChildren<Camera>(true).transform.parent;
            cameraPivot.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!isLocalPlayer)
            {
                // WYŁĄCZ kamerę i audio u zdalnych graczy
                Camera cam = GetComponentInChildren<Camera>(true);
                if (cam != null)
                    cam.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (!isLocalPlayer) return;

            HandleMovement();
            HandleLook();
        }

        void HandleMovement()
        {
            bool grounded = characterController.isGrounded;
            bool jumpPressed = inputHandler.JumpPressed;

            Vector2 moveInput = inputHandler.Move;
            bool sprint = inputHandler.Sprint;

            float speed = walkSpeed * (sprint ? sprintMultiplier : 1f);

            // poziomy ruch
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            characterController.Move(move * speed * Time.deltaTime);

            // reset prędkości Y gdy na ziemi
            if (grounded && velocity.y < 0f)
                velocity.y = -2f;

            // SKOK — TYLKO RAZ, PRZED GRAWITACJĄ
            if (jumpPressed && grounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // grawitacja
            velocity.y += gravity * Time.deltaTime;

            // pionowy ruch
            characterController.Move(Vector3.up * velocity.y * Time.deltaTime);
        }

        void HandleLook()
        {
            Vector2 lookInput = inputHandler.Look * lookSpeed;

            xRotation -= lookInput.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.Rotate(Vector3.up * lookInput.x);
            cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}
