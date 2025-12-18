using Mirror;
using UnityEngine;
using TMPro;

namespace sknco.prisongame
{
    public class PlayerSetup : NetworkBehaviour
    {
        [Header("Managable Components")]
        [SerializeField] private Player player;
        [SerializeField] private TextMeshPro nameText;

        public override void OnStartClient()
        {
            if(isLocalPlayer) nameText.gameObject.SetActive(false);

            if (player != null)
                player.ClientOnNameUpdated += UpdateNameText;

            if (!string.IsNullOrEmpty(player.Name))
                UpdateNameText(player.Name);
        }
        
        private void UpdateNameText(string newName)
        {
            if (nameText != null)
                nameText.text = newName;
        }
        
        /*
        public override void OnStopClient()
        {
            if (player != null)
                player.ClientOnNameUpdated -= UpdateNameText;
        }*/
    }
}
