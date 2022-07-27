using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public bool playerTurn;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Start:
                break;
            case GameState.GenerateBoard:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                OnEnemyTurn();
                break;
            case GameState.Victory:
                break;
            case GameState.Defeat:
                break;
            case GameState.Pause:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void OnEnemyTurn()
    {
        throw new NotImplementedException();
    }
}

public enum GameState
{
    Start,
    GenerateBoard,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat,
    Pause,
}