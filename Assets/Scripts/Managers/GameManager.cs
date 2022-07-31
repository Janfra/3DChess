using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public Faction FactionTurn { get; private set; }

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
                if (!GridManager.Instance.AreTilesGenerated())
                {
                    GridManager.Instance.GenerateGrid();
                } 
                else
                {
                    GridManager.Instance.ResetGrid();
                }
                break;
            case GameState.WhiteTurn:
                FactionTurn = Faction.White;
                break;
            case GameState.BlackTurn:
                FactionTurn = Faction.Black;
                break;
            case GameState.Victory:
                break;
            case GameState.Pause:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public GameState TurnUpdate()
    {
        if (FactionTurn == Faction.White) return GameState.BlackTurn; else return GameState.WhiteTurn;
    }
}

public enum GameState
{
    Start,
    GenerateBoard,
    WhiteTurn,
    BlackTurn,
    Victory,
    Pause,
}