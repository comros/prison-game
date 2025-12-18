using System;
using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace sknco.prisongame
{
    public class ClientSetup : NetworkBehaviour
    {
        [Header("Managable Components")]
        [SerializeField] private Player player;
        [SerializeField] private PlayerNameTag nameTag;
        private Transform cameraPivot;
        
        private void Awake()
        {
            cameraPivot = transform.GetChild(0).GetComponent<Transform>();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            AttachCamera();
        }
        
        private void AttachCamera()
        {
            // 1. Znajdź główną kamerę na scenie
            Camera sceneCamera = Camera.main;

            if (sceneCamera != null)
            {
                // 2. Ustaw jej rodzica na nasz Pivot
                sceneCamera.transform.SetParent(cameraPivot);

                // 3. Wyzeruj pozycję i rotację, aby kamera "wskoczyła" idealnie w punkt
                sceneCamera.transform.localPosition = Vector3.zero;
                sceneCamera.transform.localRotation = Quaternion.identity;
            
                Debug.Log("Camera attached");
            }
            else
            {
                Debug.LogError("Main camera not found");
            }
        }
        


        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                nameTag.gameObject.SetActive(false);
            }
            
            player.ClientOnNameUpdated += nameTag.SetName;

            if (!string.IsNullOrEmpty(player.Name))
                nameTag.SetName(player.Name);
        }
        
        
        public override void OnStopClient()
        {
            if (player != null)
                player.ClientOnNameUpdated -= nameTag.SetName;
        }
    }
}
