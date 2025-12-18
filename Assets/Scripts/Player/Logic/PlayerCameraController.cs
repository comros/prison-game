using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

namespace sknco.prisongame
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform playerBody;
        [SerializeField] Camera cam;
        public Camera PlayerCamera => cam;

        [Header("Settings")]
        [SerializeField] float mouseSensitivity = 3.0f;
        [SerializeField] float minPitch = -90f;
        [SerializeField] float maxPitch = 90f;

        [Header("Input")]
        [SerializeField] InputActionReference look;

        float pitch;
        NetworkIdentity identity;

        void Awake()
        {
            identity = GetComponentInParent<NetworkIdentity>();
        }

        void Start()
        {
            bool isLocal = identity != null && identity.isLocalPlayer;

            cam.enabled = isLocal;
            if (!isLocal) return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void OnEnable()
        {
            if (look != null)
                look.action.Enable();
        }

        void OnDisable()
        {
            if (look != null)
                look.action.Disable();
        }

        void Update()
        {
            if (identity == null || !identity.isLocalPlayer)
                return;

            Vector2 mouseDelta = look.action.ReadValue<Vector2>();

            if (mouseDelta == Vector2.zero)
                return;

            mouseDelta *= mouseSensitivity;

            pitch -= mouseDelta.y;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}