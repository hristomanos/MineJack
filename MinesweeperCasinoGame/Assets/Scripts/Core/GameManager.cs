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
        
        //Starts the game
        //Ends the game
        //Determines if the game is over

        private void Start()
        {
            gameHudPresenter.OnBetButtonClickedEvent.AddListener(StartGame);
            gameHudPresenter.OnDifficultyChangedEvent.AddListener(OnDifficultyChanged);
            
            difficultyManager.Initialize();
            gridManager.Initialize(difficultyManager.CurrentDifficultyConfig,difficultyManager.CurrentGridLayoutGroup);
        }

        private void StartGame()
        {
            gridManager.OnBetButtonClicked();
            WebGLBridge.NotifyGameStarted();
        }
        
        private void OnDifficultyChanged(int difficultyIndex)
        {
            difficultyManager.SelectDifficulty(difficultyIndex);
            gridManager.OnDifficultyChanged(difficultyManager);
        }
    }
}
