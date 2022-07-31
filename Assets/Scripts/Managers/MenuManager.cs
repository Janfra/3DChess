using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private Text victoryText;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStart;
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

    {
        startScreen.SetActive(state == GameState.Start);
    }
    
    private void WhiteSideVictory()
    {
        victoryText.text = "WHITE VICTORY";
    }

    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameState.GenerateBoard);
    }

    public void Rematch()
    {
        GameManager.Instance.UpdateGameState(GameState.GenerateBoard);
    }
}