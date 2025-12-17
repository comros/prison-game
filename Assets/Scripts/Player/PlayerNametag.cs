using System;
using TMPro;
using UnityEngine;

namespace sknco.prisongame
{
    public class PlayerNametag : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        Camera targetCamera;
        PlayerState state;
        private Camera _camera;

        void Awake()
        {
            state = GetComponentInParent<PlayerState>();
            
            if (state != null && state.isLocalPlayer)
            {
                gameObject.SetActive(false);
            }
        }
        
        void OnEnable()
        {
            if (state != null)
                state.NameChanged += UpdateName;
        }

        void OnDisable()
        {
            if (state != null)
                state.NameChanged -= UpdateName;
        }
        
        void UpdateName(string newName)
        {
            text.text = newName;
        }
        
        // Nametag rotation

        void LateUpdate()
        {
            if (targetCamera is null)
            {
                targetCamera = Camera.main;
                if (targetCamera is null) return;
            }

            transform.LookAt(
                transform.position + targetCamera.transform.forward,
                targetCamera.transform.up
            );
        }
    }
}