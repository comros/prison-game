using UnityEngine;

namespace sknco.prisongame
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerBody;

        [Header("Settings")]
        [SerializeField] private float mouseSensitivity = 3.0f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        private PlayerInputHandler input;
        private float pitch;

        private void Awake()
        {
            // Szukamy inputu w rodzicu (Player)
            input = GetComponentInParent<PlayerInputHandler>();

            if (input == null)
            {
                Debug.LogError("PlayerInputHandler not found in parent!");
                enabled = false;
            }
        }

        private void Start()
        {
            // Kamera działa tylko lokalnie
            if (!input.isLocalPlayer)
            {
                enabled = false;
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector2 mouseDelta = input.Look;
            if (mouseDelta == Vector2.zero) return;

            mouseDelta *= mouseSensitivity;

            pitch -= mouseDelta.y;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // góra/dół — pivot
            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            // lewo/prawo — ciało gracza
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}