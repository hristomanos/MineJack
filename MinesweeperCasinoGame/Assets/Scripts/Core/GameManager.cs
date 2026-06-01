using System;
using Grid;
using UI;
using UnityEngine;
using WebGL;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameHudPresenter gameHudPresenter;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private DifficultyManager difficultyManager;
        
        //Owns the game state
        public GameState CurrentGameState { get; private set; } = GameState.WaitingToStart;
        
        //Starts the game
        //Ends the game
        //Determines if the game is over
        
        private void Start()
        {
            gameHudPresenter.OnBetButtonClickedEvent.AddListener(StartGame);
            gameHudPresenter.OnDifficultyChangedEvent.AddListener(OnDifficultyChanged);
            
            difficultyManager.Initialize();
            gridManager.Initialize(OnPlayerCompletedGrid,OnPlayerHitBomb,difficultyManager.CurrentDifficultyConfig,difficultyManager.CurrentGridLayoutGroup);
        }

        private void StartGame()
        {
            if (CurrentGameState == GameState.Playing)
                return;

            if (CurrentGameState == GameState.GameOver) 
                gridManager.ResetGrid();
            
            CurrentGameState = GameState.Playing;
            
            gridManager.EnableCurrentRow();
            gameHudPresenter.SetInteractable(false);
            WebGLBridge.NotifyGameStarted();
        }
        
        private void OnDifficultyChanged(int difficultyIndex)
        {
            if (difficultyManager.TrySelectDifficulty(difficultyIndex) == false)
                return;
            
            gridManager.OnDifficultyChanged(difficultyManager);
        }
        
        
        private void OnPlayerHitBomb()
        {
            Debug.Log("Player Lost!");
            EndGame(false);
        }

        private void OnPlayerCompletedGrid()
        {
            Debug.Log("Player Wins!");
            EndGame(true);
        }
    
        private void EndGame(bool playerWon)
        {
            gridManager.RevealGrid();
            gridManager.SetGridInteractable(false);

            CurrentGameState = GameState.GameOver;
            gameHudPresenter.SetInteractable(true);
        }

        private void OnDestroy()
        {
            if (gameHudPresenter == null)
                return;
            
            gameHudPresenter.OnBetButtonClickedEvent.RemoveListener(StartGame);
            gameHudPresenter.OnDifficultyChangedEvent.RemoveListener(OnDifficultyChanged);
        }
    }
}
