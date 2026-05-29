using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using WebGL;

public class GridManager : MonoBehaviour
{
    private void StartGame()
    {
        OnBetButtonClicked();
        WebGLBridge.NotifyGameStarted();
    }
    
    private void SelectDifficulty(int num)
    {
        OnDifficultyChanged(num);
        WebGLBridge.NotifyDifficultySelected(num);
    }

    [FormerlySerializedAs("button")] [SerializeField] private GameObject buttonCellPrefab;
    [SerializeField] private GridLayoutGroup easyGridLayoutGroup;
    [SerializeField] private GridLayoutGroup mediumGridLayoutGroup;
    [SerializeField] private GridLayoutGroup hardGridLayoutGroup;
    [SerializeField] private GridLayoutGroup expertGridLayoutGroup;
    [SerializeField] private GridLayoutGroup masterGridLayoutGroup;

    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private Button betButton;

    [SerializeField] DifficultySettings[] difficultySettings;
    
    private GridLayoutGroup currentGridLayoutGroup;

    private int minBombsPerRow = 1;

    private Difficulty currentDifficulty = Difficulty.Easy;

    private int width;
    private int height = 9;

    private ButtonCellPresenter[,] grid;

    private int currentRow = 0;

    private GameState currentGameState = GameState.WaitingToStart;

    private void Awake()
    {
        var optionsList = new List<string> { "Easy", "Medium", "Hard", "Expert", "Master" };
        difficultyDropdown.AddOptions(optionsList);
        difficultyDropdown.value = (int) currentDifficulty;
        difficultyDropdown.onValueChanged.AddListener(SelectDifficulty);
        betButton.onClick.AddListener(StartGame);
        ApplyDifficultySettings();
        GenerateGrid();
    }
    
    private void GenerateGrid()
    {
        for(int row = 0; row < height; row++)
        {
            var bombCellIndices = GenerateBombCellIndices();

            for(int column = 0; column < width; column++)
            {
                ButtonCellPresenter buttonCellPresenter = Instantiate(buttonCellPrefab, currentGridLayoutGroup.transform).GetComponent<ButtonCellPresenter>();

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

    public void OnDifficultyChanged(int index)
    {
        Difficulty selectedDifficulty = (Difficulty)index;

        if (selectedDifficulty == currentDifficulty)
            return;

        ClearGrid();
        
        currentDifficulty = selectedDifficulty;
        
        ApplyDifficultySettings();
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
        difficultyDropdown.interactable = false;
        betButton.interactable = false;

        if(currentGameState == GameState.GameOver)
        {
            ResetGrid();
        }

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

    private void ApplyDifficultySettings()
    {
        switch(currentDifficulty)
        {
            case Difficulty.Easy:
                width = 4;
                minBombsPerRow = 1;
                currentGridLayoutGroup = easyGridLayoutGroup;
                break;
            case Difficulty.Medium:
                width = 3;
                minBombsPerRow = 1;
                currentGridLayoutGroup = mediumGridLayoutGroup;
                break;
            case Difficulty.Hard:
                width = 2;
                minBombsPerRow = 1;
                currentGridLayoutGroup = hardGridLayoutGroup;
                break;
            case Difficulty.Expert:
                width = 3;
                minBombsPerRow = 2;
                currentGridLayoutGroup = expertGridLayoutGroup;
                break;
            case Difficulty.Master:
                width = 4;
                minBombsPerRow = 3;
                currentGridLayoutGroup = masterGridLayoutGroup;
                break;
            default:
                width = 4;
                minBombsPerRow = 1;
                break;
        }

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
        EndGame();
    }

    private void OnPlayerCompletedGrid()
    {
        Debug.Log("Player Wins!");
        EndGame();
    }
    
    private void EndGame()
    {
        for(int row = 0; row < height; row++)
        {
            for(int column = 0; column < width; column++)
            {
                grid[column, row].RevealUnselectedType();
                grid[column, row].SetInteractable(false);
            }
        }

        currentGameState = GameState.GameOver;
        difficultyDropdown.interactable = true;
        betButton.interactable = true;
    }

    private int GetBombCellIndex()
    {
        return Random.Range(0, width);
    }

    

    public void ResetGrid()
    {
        currentRow = 0;
        for(int row = 0; row < height; row++)
        {
            var bombCellIndices = GenerateBombCellIndices();

            for(int column = 0; column < width; column++)
            {
                CellType type = bombCellIndices.Contains(column) ? CellType.Bomb : CellType.Key;
                grid[column, row].Initialize(type, EndGame, OnPlayerFoundKey);
                grid[column, row].name = $"Cell {column},{row} {type.ToString()}";
            }
        }
        
        SetOnlyCurrentRowInteractable();
    }

    private HashSet<int> GenerateBombCellIndices()
    {
        HashSet<int> bombCellIndices = new HashSet<int>();

        while(bombCellIndices.Count < Mathf.Min(minBombsPerRow, width))
        {
            bombCellIndices.Add(GetBombCellIndex());
        }

        return bombCellIndices;
    }
}
