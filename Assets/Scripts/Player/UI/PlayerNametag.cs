using UnityEngine;
using TMPro;
using Mirror;

public class PlayerNametag : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private TextMeshPro nameText;

    void Update() // Zmieniamy na Update/LateUpdate
    {
        if (playerState.isLocalPlayer)
        {
            if (nameText.gameObject.activeSelf) nameText.gameObject.SetActive(false);
            return;
        }
    }
    
    // Dodatkowy check na starcie
    private void Start()
    {
        if(playerState) UpdateText(playerState.playerName);
    }

    void UpdateText(string s) => nameText.text = s;
}