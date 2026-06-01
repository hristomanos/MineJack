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
        
        public GameState CurrentGameState { get; private set; } = GameState.WaitingToStart;
        
        private void Start()
        {
            gameHudPresenter.OnBetButtonClickedEvent.AddListener(StartGame);
            gameHudPresenter.OnDifficultyChangedEvent.AddListener(OnDifficultyChanged);
            
            difficultyManager.Initialize();
            
            gameHudPresenter.Initialize(difficultyManager.CurrentDifficulty);
            gridManager.Initialize(
                OnPlayerCompletedGrid,
                OnPlayerHitBomb,
                difficultyManager.CurrentDifficultyConfig,
                difficultyManager.CurrentGridLayoutGroup);
        }
        
        private void OnDestroy()
        {
            if (gameHudPresenter == null)
                return;
            
            gameHudPresenter.OnBetButtonClickedEvent.RemoveListener(StartGame);
            gameHudPresenter.OnDifficultyChangedEvent.RemoveListener(OnDifficultyChanged);
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
            EndGame();
        }

        private void OnPlayerCompletedGrid()
        {
            Debug.Log("Player Wins!");
            EndGame();
        }
    
        private void EndGame()
        {
            gridManager.RevealGrid();
            gridManager.SetGridInteractable(false);

            CurrentGameState = GameState.GameOver;
            gameHudPresenter.SetInteractable(true);
        }
    }
}
