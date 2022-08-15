using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public Faction FactionTurn { get; private set; }

    public static event Action<GameState> OnGameStateChanged;


    // Singleton
    private void Awake()
    {
        Instance = this;
        Castling.Castled += Castled;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    // Update the game state and run the logic for the current state, start event.
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Start:
                break;

                // If the board was already generated just reuse it
            case GameState.GenerateBoard:
                if (!GridManager.Instance.BoardGenCheck())
                {
                    GridManager.Instance.GenerateGrid();
                } 
                else
                {
                    GridManager.Instance.ResetGrid();
                }
                break;

                // Change the 'FactionTurn' to set which pieces can be used
            case GameState.WhiteTurn:
                FactionTurn = Faction.White;
                break;
            case GameState.BlackTurn:
                FactionTurn = Faction.Black;
                break;

            case GameState.PawnUpgrade:
                Debug.Log("UpgradePawn");
                GridManager.Instance.UnhighlightMoveTiles();
                break;
            case GameState.Victory:
                break;
            case GameState.Pause:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    // Returns who's side turn it is
    public GameState TurnUpdate()
    {
        if (FactionTurn == Faction.White) return GameState.BlackTurn; else return GameState.WhiteTurn;
    }

    public void NextTurn()
    {
        UnitManager.Instance.SelectedPiece = null;
        GridManager.Instance.UnhighlightMoveTiles();
        if (Instance.State == GameState.BlackTurn || Instance.State == GameState.WhiteTurn || Instance.State == GameState.PawnUpgrade) Instance.UpdateGameState(Instance.TurnUpdate());
    }

    private void Castled()
    {
        NextTurn();
    }
}

public enum GameState
{
    Start,
    GenerateBoard,
    WhiteTurn,
    BlackTurn,
    PawnUpgrade,
    Victory,
    Pause,
}