using UnityEngine;
using Mirror;

namespace sknco.prisongame
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementController : NetworkBehaviour
    {
        [Header("Movement")]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float sprintMultiplier = 1.5f;
        [SerializeField] float gravity = -9.81f;
        [SerializeField] float jumpForce = 5f;

        CharacterController controller;
        PlayerInputHandler input;

        float verticalVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            if (!isLocalPlayer) return;

            Vector2 move = input.Move;

            float speed = input.Sprint ? moveSpeed * sprintMultiplier : moveSpeed;
            Vector3 moveDir = transform.right * move.x + transform.forward * move.y;

            if (controller.isGrounded)
            {
                if (verticalVelocity < 0)
                    verticalVelocity = -2f;

                if (input.JumpPressed)
                    verticalVelocity = jumpForce;
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity;
            controller.Move(velocity * Time.deltaTime);
        }


    }
}