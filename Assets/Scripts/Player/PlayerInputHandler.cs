using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

namespace sknco.prisongame
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool Sprint { get; private set; }

        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference look;
        [SerializeField] InputActionReference jump;
        [SerializeField] InputActionReference sprint;

        public override void OnStartAuthority()
        {
            EnableInput(true);
        }

        public override void OnStopAuthority()
        {
            EnableInput(false);
        }

        private void EnableInput(bool enable)
        {
            if (enable)
            {
                move.action.Enable();
                look.action.Enable();
                jump.action.Enable();
                sprint.action.Enable();
            }
            else
            {
                move.action.Disable();
                look.action.Disable();
                jump.action.Disable();
                sprint.action.Disable();
            }
        }

        void Update()
        {
            if (!isOwned) return;

            Move = move.action.ReadValue<Vector2>();
            Look = look.action.ReadValue<Vector2>();
            JumpPressed = jump.action.WasPressedThisFrame();
            Sprint = sprint.action.IsPressed();
        }
    }
}