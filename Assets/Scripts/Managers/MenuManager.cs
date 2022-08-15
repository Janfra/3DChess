using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject pawnUpgradeScreen;
    [SerializeField] private Text victoryText;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStart;
        GameManager.OnGameStateChanged += OnGameVictory;
        GameManager.OnGameStateChanged += OnPawnUpgrade;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStart;
        GameManager.OnGameStateChanged -= OnGameVictory;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStart;
        GameManager.OnGameStateChanged -= OnGameVictory;
    }

    // Start Game
    private void OnGameStart(GameState state)
    {
        startScreen.SetActive(state == GameState.Start);
    }

    // Activate Victory screen
    private void OnGameVictory(GameState state)
    {
        if(state == GameState.Victory)
        {
            victoryScreen.SetActive(true);
            if(GameManager.Instance.FactionTurn == Faction.Black)
            {
                BlackSideVictory();
            } 
            else
            {
                WhiteSideVictory();
            }
            GameManager.OnGameStateChanged -= OnGameVictory;
        }
    }

    // Black win
    private void BlackSideVictory()
    {
        victoryText.text = "BLACK VICTORY";
    }

    // White win
    private void WhiteSideVictory()
    {
        victoryText.text = "WHITE VICTORY";
    }

    // Button reference to update state
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameState.GenerateBoard);
    }

    // Button reference to update state
    public void Rematch()
    {
        GameManager.Instance.UpdateGameState(GameState.GenerateBoard);
    }

    private void OnPawnUpgrade(GameState state)
    {
        if(state == GameState.PawnUpgrade)
        {
            pawnUpgradeScreen.SetActive(true);
        } 
        else if(pawnUpgradeScreen.activeSelf == true)
        {
            pawnUpgradeScreen.SetActive(false);
        }
    }
}