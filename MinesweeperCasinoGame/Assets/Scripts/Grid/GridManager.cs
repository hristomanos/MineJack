using System.Collections.Generic;
using Config;
using Core;
using Grid.ButtonCell;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown difficultyDropdown;
        [SerializeField] private Button betButton;

        [SerializeField] private GameObject canvas;
        [SerializeField] DifficultyConfig[] difficultySettings;
        [SerializeField] private ButtonCellPresenter buttonCellPrefab;
        
        private GridLayoutGroup currentGridLayoutGroup;

        private int bombsPerRow = 1;

        private Difficulty currentDifficulty = Difficulty.Easy;

        private int width;
        private int height;

        private ButtonCellPresenter[,] grid;

        private int currentRow;

        private GameState currentGameState = GameState.WaitingToStart;

        public void Initialize(DifficultyConfig difficultyConfig, GridLayoutGroup gridLayoutGroup)
        {
            ApplyDifficultySettings(difficultyConfig, gridLayoutGroup);
            GenerateGrid();
        }
    
        private void GenerateGrid()
        {
            for(int row = 0; row < height; row++)
            {
                var bombCellIndices = GenerateBombCellIndices();

                for(int column = 0; column < width; column++)
                {
                    var buttonCellPresenter = Instantiate(buttonCellPrefab, currentGridLayoutGroup.transform);

                    buttonCellPresenter.gameObject.SetActive(false);
                    
                    buttonCellPresenter.SetInteractable(false);
                    
                    buttonCellPresenter.gameObject.SetActive(true);

                    grid[column, row] = buttonCellPresenter;

                    CellType type = bombCellIndices.Contains(column) ? CellType.Bomb : CellType.Key;

                    buttonCellPresenter.name = $"Cell {column},{row} {type.ToString()}";

                    buttonCellPresenter.Initialize(type, OnPlayerHitBomb, OnPlayerFoundKey);
                }
            }
        }

        public void OnDifficultyChanged(DifficultyManager difficultyManager)
        {
            if (difficultyManager.CurrentDifficulty == currentDifficulty)
                return;

            ClearGrid();
        
            currentDifficulty = difficultyManager.CurrentDifficulty;
        
            ApplyDifficultySettings(difficultyManager.CurrentDifficultyConfig, currentGridLayoutGroup);
            GenerateGrid();
        }
    
        private void ClearGrid()
        {
            currentRow = 0;
            foreach(Transform child in currentGridLayoutGroup.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void OnBetButtonClicked()
        {
            if (currentGameState == GameState.Playing)
                return;
            
            difficultyDropdown.interactable = false;
            betButton.interactable = false;

            if(currentGameState == GameState.GameOver)
                ResetGrid();

            currentGameState = GameState.Playing;
            SetOnlyCurrentRowInteractable();
        }

        private void SetOnlyCurrentRowInteractable()
        {
            for(int row = 0; row < height; row++)
            {
                for(int column = 0; column < width; column++)
                {
                    grid[column, row].SetInteractable(row == currentRow);
                }
            }
        }
        
        private void ApplyDifficultySettings(DifficultyConfig difficultyConfig, GridLayoutGroup gridLayoutGroup)
        {
            currentGridLayoutGroup = gridLayoutGroup;
            width = difficultyConfig.width;
            height = difficultyConfig.height;
            bombsPerRow = difficultyConfig.bombsPerRow;

            grid = new ButtonCellPresenter[width, height];
        }

        private void OnPlayerFoundKey()
        {
            currentRow++;

            if(currentRow >= height)
            {
                OnPlayerCompletedGrid();
                return;
            }

            SetOnlyCurrentRowInteractable();
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
            RevealGrid();
            SetGridInteractable(false);

            currentGameState = GameState.GameOver;
            difficultyDropdown.interactable = true;
            betButton.interactable = true;
        }

        private void RevealGrid()
        {
            for(int row = 0; row < height; row++)
            {
                for(int column = 0; column < width; column++)
                {
                    grid[column, row].RevealUnselectedType();
                }
            }
        }

        private void SetGridInteractable(bool interactable)
        {
            for(int row = 0; row < height; row++)
            {
                for(int column = 0; column < width; column++)
                {
                    grid[column, row].SetInteractable(interactable);
                }
            }
        }

        private int GetBombCellIndex()
        {
            return Random.Range(0, width);
        }

        private void ResetGrid()
        {
            currentRow = 0;
            for(int row = 0; row < height; row++)
            {
                var bombCellIndices = GenerateBombCellIndices();

                for(int column = 0; column < width; column++)
                {
                    CellType type = bombCellIndices.Contains(column) ? CellType.Bomb : CellType.Key;
                    grid[column, row].Initialize(type, OnPlayerHitBomb, OnPlayerFoundKey);
                    grid[column, row].name = $"Cell {column},{row} {type.ToString()}";
                }
            }
        
            SetOnlyCurrentRowInteractable();
        }

        private HashSet<int> GenerateBombCellIndices()
        {
            HashSet<int> bombCellIndices = new HashSet<int>();

            while(bombCellIndices.Count < Mathf.Min(bombsPerRow, width))
            {
                bombCellIndices.Add(GetBombCellIndex());
            }

            return bombCellIndices;
        }
    }
}
