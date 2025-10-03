using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject gridPlaceholder;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float space;

    private ButtonCell[,] grid;

    int currentRow = 0;

    void Start()
    {
        grid = new ButtonCell[width, height];

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int h = 0; h < height; h++)
        {
            var badCell = GetBadCell();
            for(int w = 0; w < width; w++)
            {
                ButtonCell buttonCell = Instantiate(button, new Vector3(w * space, h * space, 0), Quaternion.identity, gridPlaceholder.transform).GetComponent<ButtonCell>();

                grid[w, h] = buttonCell;

                buttonCell.name = $"Cell {w},{h}";

                buttonCell.Initialize(w == badCell ? ButtonType.BOMB : ButtonType.KEY);

                buttonCell.SetInteractable(w < width && h == currentRow);

                buttonCell.OnClick.AddListener(OnButtonClicked);
            }
        }
    }

    void OnButtonClicked()
    {
        currentRow++;
        for(int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                grid[w, h].SetInteractable(w < width && h == currentRow);
            }
        }
    }

    float GetBadCell()
    {
        return Random.Range(0, width);
    }
}
