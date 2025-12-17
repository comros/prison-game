using Mirror;
using UnityEngine;

public enum GameState
{
    Lobby,
    Starting,
    InProgress,
    Ended
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SyncVar]
    public GameState currentState = GameState.Lobby;

    [SyncVar]
    public float matchTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Ważne: Jeśli to obiekt sieciowy (NetworkIdentity), nie używaj DontDestroyOnLoad bez ostrożności w Mirrorze.
        // W Mirrorze NetworkManager zarządza cyklem życia obiektów sceny.
    }

    // Logika uruchamiana tylko na serwerze
    [ServerCallback]
    private void Update()
    {
        if (currentState == GameState.InProgress)
        {
            matchTimer -= Time.deltaTime;
            if (matchTimer <= 0)
            {
                EndMatch();
            }
        }
    }

    [Server]
    public void StartMatch()
    {
        currentState = GameState.InProgress;
        matchTimer = 300f; // 5 minut
    }

    [Server]
    public void EndMatch()
    {
        currentState = GameState.Ended;
        // Logika końca gry, np. respawn wszystkich w celach
        RpcMatchEnded();
    }

    [ClientRpc]
    private void RpcMatchEnded()
    {
        Debug.Log("Mecz zakończony!");
        // Tutaj pokaż tabelę wyników
    }
}