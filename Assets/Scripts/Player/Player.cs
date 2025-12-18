using Mirror;
using UnityEngine;
using Steamworks; // Wymagane dla FizzySteamworks/SteamManager
using System;

namespace sknco.prisongame
{
    public class Player : NetworkBehaviour
    {
        // --- SYNCHRONIZED DATA (SERVER -> CLIENTS) ---
    
        [SyncVar(hook = nameof(OnNameChanged))]
        public string Name;

        [SyncVar(hook = nameof(OnHealthChanged))]
        public int currentHealth = 100;

        [SyncVar]
        public ulong steamID;

        // --- EVENTS ---
        public event Action<string> ClientOnNameUpdated;
        public event Action<int> ClientOnHealthUpdated;

        // --- SETUP ---

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!string.IsNullOrEmpty(Name))
                ClientOnNameUpdated?.Invoke(Name);
            
            ClientOnHealthUpdated?.Invoke(currentHealth);
        }

        public override void OnStartLocalPlayer()
        {
            string myName = "Player";
            ulong myID = 0;

            if (SteamManager.Initialized) 
            {
                myName = SteamFriends.GetPersonaName();
                myID = SteamUser.GetSteamID().m_SteamID;
            }
            else
            {
                myName = "NoSteamUser_" + UnityEngine.Random.Range(100, 999);
            }
        
            CmdSetPlayerData(myName, myID);
        }

        // --- LOGIKA SERWERA ---

        [Command]
        private void CmdSetPlayerData(string name, ulong id)
        {
            // Note: add nickname validation
            this.Name = name;
            this.steamID = id;
        }

        [Server]
        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
        }

        // --- Client Hooks ---

        private void OnNameChanged(string oldName, string newName)
        {
            ClientOnNameUpdated?.Invoke(newName);
        }

        private void OnHealthChanged(int oldHealth, int newHealth)
        {
            ClientOnHealthUpdated?.Invoke(newHealth);
        }
    }
}
