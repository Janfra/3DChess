using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        startScreen.SetActive(state == GameState.Start);
    }
    
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameState.GenerateBoard);
    }
}