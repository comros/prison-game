using Mirror;
using Steamworks;
using UnityEngine;
using TMPro;

namespace sknco.prisongame
{
    public enum PlayerRole
    {
        Prisoner,
        Guard,
        Spectator
    }
    
    public class PlayerState : NetworkBehaviour
    {
        [Header("Identity")]
        [SyncVar(hook = nameof(OnNameChanged))]
        public string PlayerName;
        
        [SyncVar]
        public PlayerRole Role;
        
        [Header("Status")]
        [SyncVar]
        public bool IsAlive = true;
        
        [SyncVar]
        public int Health = 100;
        
        public bool IsLocal => isLocalPlayer;
        
        public event System.Action<string> NameChanged;
        public event System.Action<PlayerRole> RoleChanged;
        
        public override void OnStartClient()
        {
            NameChanged?.Invoke(PlayerName);
            RoleChanged?.Invoke(Role);
        }

        void OnNameChanged(string oldName, string newName)
        {
            NameChanged?.Invoke(newName);
        }
        
        public void SetLocalPlayerName()
        {
            if (!isLocalPlayer) return;

            string steamName = SteamFriends.GetPersonaName();
            
            PlayerName = steamName;
            NameChanged?.Invoke(PlayerName);
            
            CmdSetPlayerName(steamName);
        }
        
        [Command]
        void CmdSetPlayerName(string name)
        {
            PlayerName = name;
        }
        
        [Server]
        public void SetRole(PlayerRole role)
        {
            Role = role;
            RoleChanged?.Invoke(role);
        }
    }
}