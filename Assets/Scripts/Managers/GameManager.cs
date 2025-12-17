using Mirror;
using UnityEngine;

namespace sknco.prisongame
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        [Server]
        public void StartRound()
        {
            // logika startu rundy np. zmiana fazy
        }

        [Server]
        public void EndRound()
        {
            // logika końca rundy
        }

        [Server]
        public PlayerState[] GetAllPlayers()
        {
            return FindObjectsOfType<PlayerState>();
        }

        [Server]
        public string[] GetAllPlayerNames()
        {
            var players = GetAllPlayers();
            string[] names = new string[players.Length];
            for (int i = 0; i < players.Length; i++)
                names[i] = players[i].PlayerName;
            return names;
        }
    }
}