using UnityEngine;
using UnityEngine.InputSystem;

namespace sknco.prisongame
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool Sprint { get; private set; }

        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference jump;
        [SerializeField] InputActionReference sprint;

        void OnEnable()
        {
            move.action.Enable();
            jump.action.Enable();
            sprint.action.Enable();
        }

        void OnDisable()
        {
            move.action.Disable();
            jump.action.Disable();
            sprint.action.Disable();
        }

        void Update()
        {
            Move = move.action.ReadValue<Vector2>();
            JumpPressed = jump.action.WasPressedThisFrame();
            Sprint = sprint.action.IsPressed();
        }
    }
}