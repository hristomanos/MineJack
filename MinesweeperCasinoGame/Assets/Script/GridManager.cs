using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject gridPlaceholder;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float space;

    private ButtonCell[,] grid;

    private int currentRow = 0;

    private void Awake()
    {
        grid = new ButtonCell[width, height];
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int h = 0; h < height; h++)
        {
            var bombCellIndex = GetBombCellIndex();
            for(int w = 0; w < width; w++)
            {
                ButtonCell buttonCell = Instantiate(button, new Vector3(w * space, h * space, 0), Quaternion.identity, gridPlaceholder.transform).GetComponent<ButtonCell>();

                buttonCell.gameObject.SetActive(false);

                buttonCell.SetInteractable(h == currentRow);

                buttonCell.gameObject.SetActive(true);

                grid[w, h] = buttonCell;

                buttonCell.name = $"Cell {w},{h}";

                buttonCell.Initialize(w == bombCellIndex ? ButtonType.BOMB : ButtonType.KEY, OnGameEnds, OnPlayerFoundKey);
            }
        }
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
    }

    private int GetBombCellIndex()
    {
        return Random.Range(0, width);
    }

    public void ResetGrid()
    {
        currentRow = 0;
        for(int h = 0; h < height; h++)
        {
            var bombCellIndex = GetBombCellIndex();
            for(int w = 0; w < width; w++)
            {
                grid[w, h].Initialize(w == bombCellIndex ? ButtonType.BOMB : ButtonType.KEY, OnGameEnds, OnPlayerFoundKey);
                grid[w, h].SetInteractable(h == currentRow);
            }
        }
    }
}
