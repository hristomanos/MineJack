using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GridLayoutGroup gridLayoutGroup_Easy;
    [SerializeField] private GridLayoutGroup gridLayoutGroup_Medium;
    [SerializeField] private GridLayoutGroup gridLayoutGroup_Hard;
    [SerializeField] private GridLayoutGroup gridLayoutGroup_Expert;
    [SerializeField] private GridLayoutGroup gridLayoutGroup_Master;

    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private Button betButton;

    private GridLayoutGroup currentGridLayoutGroup;

    private int minBombsPerRow = 1;

    private Difficulty currentDifficulty = Difficulty.EASY;
    private Difficulty previousDifficulty;

    private int width;
    private int height = 9;
    private float space = 1.2f;

    private ButtonCell[,] grid;

    private int currentRow = 0;

    private bool gameEnded = false;

    private void Awake()
    {
        difficultyDropdown.AddOptions(new List<string> { "Easy", "Medium", "Hard", "Expert", "Master" });
        difficultyDropdown.value = (int) currentDifficulty;
        previousDifficulty = currentDifficulty;
        difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
        betButton.onClick.AddListener(OnBetButtonClicked);
        InitiliaseBaseOffDifficulty();
        GenerateGrid();
    }

    private void OnBetButtonClicked()
    {
        difficultyDropdown.interactable = false;
        betButton.interactable = false;

        if(gameEnded)
        {
            ResetGrid();
        }

        for(int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                grid[w, h].SetInteractable(h == currentRow);
            }
        }
    }

    private void GenerateGrid()
    {
        for(int h = 0; h < height; h++)
        {
            HashSet<int> bombCellIndices = new HashSet<int>();

            while(bombCellIndices.Count < Mathf.Min(minBombsPerRow,width))
            {
                bombCellIndices.Add(GetBombCellIndex());
            }

            for(int w = 0; w < width; w++)
            {
                ButtonCell buttonCell = Instantiate(button, new Vector3(w * space, h * space, 0), Quaternion.identity, currentGridLayoutGroup.transform).GetComponent<ButtonCell>();

                buttonCell.gameObject.SetActive(false);

                buttonCell.SetInteractable(false);

                buttonCell.gameObject.SetActive(true);

                grid[w, h] = buttonCell;

                ButtonType type = bombCellIndices.Contains(w) ? ButtonType.BOMB : ButtonType.KEY;

                buttonCell.name = $"Cell {w},{h} {type.ToString()}";

                buttonCell.Initialize(type, OnGameEnds, OnPlayerFoundKey);
            }
        }
    }

    private void OnDifficultyChanged(int index)
    {
        currentDifficulty = (Difficulty)index;

        if(previousDifficulty != currentDifficulty)
        {
            ClearGrid();
            previousDifficulty = currentDifficulty;
        }

        InitiliaseBaseOffDifficulty();
        GenerateGrid();
    }

    private void InitiliaseBaseOffDifficulty()
    {
        switch(currentDifficulty)
        {
            case Difficulty.EASY:
                width = 4;
                minBombsPerRow = 1;
                currentGridLayoutGroup = gridLayoutGroup_Easy;
                break;
            case Difficulty.MEDIUM:
                width = 3;
                minBombsPerRow = 1;
                currentGridLayoutGroup = gridLayoutGroup_Medium;
                break;
            case Difficulty.HARD:
                width = 2;
                minBombsPerRow = 1;
                currentGridLayoutGroup = gridLayoutGroup_Hard;
                break;
            case Difficulty.EXPERT:
                width = 3;
                minBombsPerRow = 2;
                currentGridLayoutGroup = gridLayoutGroup_Expert;
                break;
            case Difficulty.MASTER:
                width = 4;
                minBombsPerRow = 3;
                currentGridLayoutGroup = gridLayoutGroup_Master;
                break;
            default:
                width = 4;
                minBombsPerRow = 1;
                break;
        }

        grid = new ButtonCell[width, height];
    }

    private void OnPlayerFoundKey()
    {
        currentRow++;

        if(currentRow >= height)
        {
            Debug.Log("Player Wins!");
            OnGameEnds();
            return;
        }

        for(int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                grid[w, h].SetInteractable(h == currentRow);
            }
        }
    }

    private void OnGameEnds()
    {
        for(int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                grid[w, h].RevealHiddenType();
                grid[w, h].SetInteractable(false);
            }
        }

        gameEnded = true;
        difficultyDropdown.interactable = true;
        betButton.interactable = true;
    }

    private int GetBombCellIndex()
    {
        return Random.Range(0, width);
    }

    private void ClearGrid()
    {
        currentRow = 0;
        foreach(Transform child in currentGridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetGrid()
    {
        gameEnded = false;
        currentRow = 0;
        for(int h = 0; h < height; h++)
        {
            HashSet<int> bombCellIndices = new HashSet<int>();

            while(bombCellIndices.Count < Mathf.Min(minBombsPerRow, width))
            {
                bombCellIndices.Add(GetBombCellIndex());
            }

            for(int w = 0; w < width; w++)
            {
                ButtonType type = bombCellIndices.Contains(w) ? ButtonType.BOMB : ButtonType.KEY;
                grid[w, h].Initialize(type, OnGameEnds, OnPlayerFoundKey);
                grid[w, h].SetInteractable(h == currentRow);
                grid[w, h].name = $"Cell {w},{h} {type.ToString()}";
            }
        }
    }
}
