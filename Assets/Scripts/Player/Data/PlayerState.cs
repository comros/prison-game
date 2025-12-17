using Mirror;
using UnityEngine;
using Steamworks; // Wymagane dla FizzySteamworks/SteamManager
using System;

public class PlayerState : NetworkBehaviour
{
    // --- DANE SYNCHRONIZOWANE (SERVER -> CLIENTS) ---
    
    // Hook sprawia, że jak serwer zmieni wartość, u każdego klienta odpali się funkcja OnNameChanged
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnHealthChanged))]
    public int currentHealth = 100;

    [SyncVar]
    public ulong steamID; // Przechowujemy ID, żeby później np. pobrać avatar

    // --- EVENTY (Dla UI i innych skryptów) ---
    public event Action<string> ClientOnNameUpdated;
    public event Action<int> ClientOnHealthUpdated;

    // --- SETUP ---

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Odśwież UI przy wejściu (bo hook nie odpala się przy inicjalizacji zmiennej, tylko przy ZMIANIE)
        if (!string.IsNullOrEmpty(playerName))
            ClientOnNameUpdated?.Invoke(playerName);
            
        ClientOnHealthUpdated?.Invoke(currentHealth);
    }

    public override void OnStartLocalPlayer()
    {
        // To wykonuje się TYLKO u lokalnego gracza (tego przy klawiaturze)
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

        // Wysyłamy nasze dane do serwera
        CmdSetPlayerData(myName, myID);
    }

    // --- LOGIKA SERWERA ---

    [Command]
    private void CmdSetPlayerData(string name, ulong id)
    {
        // Tutaj serwer może dodać walidację (np. czy nick nie jest wulgarny)
        this.playerName = name;
        this.steamID = id;
    }

    [Server] // Metoda do wywołania np. przez system obrażeń
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
    }

    // --- HOOKS (Wywoływane na klientach) ---

    private void OnNameChanged(string oldName, string newName)
    {
        // Powiadamiamy Nametag, że nick się zmienił
        ClientOnNameUpdated?.Invoke(newName);
    }

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        // Powiadamiamy np. pasek zdrowia
        ClientOnHealthUpdated?.Invoke(newHealth);
    }
}