using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

namespace sknco.prisongame
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool Sprint { get; private set; }

        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference look;
        [SerializeField] InputActionReference jump;
        [SerializeField] InputActionReference sprint;

        void OnEnable()
        {
            move.action.Enable();
            look.action.Enable();
            jump.action.Enable();
            sprint.action.Enable();
        }

        void OnDisable()
        {
            move.action.Disable();
            look.action.Disable();
            jump.action.Disable();
            sprint.action.Disable();
        }

        void Update()
        {
            Move = move.action.ReadValue<Vector2>();
            Look = look.action.ReadValue<Vector2>(); // musi być Vector2
            JumpPressed = jump.action.WasPressedThisFrame();
            Sprint = sprint.action.IsPressed();
        }

    }
}