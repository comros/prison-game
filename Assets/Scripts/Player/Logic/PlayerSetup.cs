using Mirror;
using UnityEngine;
using TMPro;

public class PlayerSetup : NetworkBehaviour
{
    [Header("Komponenty do zarządzania")]
    [SerializeField] private GameObject cameraRoot; // Obiekt z Kamerą wewnątrz gracza
    [SerializeField] private Behaviour[] componentsToDisableOnRemote; // Np. PlayerMovement, AudioListener
    [SerializeField] private PlayerState playerState;
    [SerializeField] private TextMeshPro nameText;

    public override void OnStartLocalPlayer()
    {
        // 1. To jestem JA (Lokalny Gracz). Włączamy kamerę.
        if (cameraRoot != null)
        {
            cameraRoot.SetActive(true);
            
            // WAŻNE: Nadajemy tag MainCamera, żeby skrypty UI wiedziały, gdzie patrzeć
            Camera cam = cameraRoot.GetComponent<Camera>();
            if (cam != null)
            {
                cam.tag = "MainCamera";
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // 2. To jest KTOŚ INNY (Remote Player). Wyłączamy mu sterowanie i kamerę.
        if (!isLocalPlayer)
        {
            if (nameText != null)
            {
                nameText.text = playerState.playerName;
            }
            
            if (cameraRoot != null)
            {
                cameraRoot.SetActive(false);
            }

            foreach (var comp in componentsToDisableOnRemote)
            {
                comp.enabled = false;
            }
        }
    }
}