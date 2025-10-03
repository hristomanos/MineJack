using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject gridPlaceholder;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float space;

    private Button[,] grid;

    int currentRow = 0;

    void Start()
    {
        grid = new Button[width, height];

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int h = 0; h < height; h++)
        {
            var badCell = GetBadCell();
            for(int w = 0; w < width; w++)
            {
                GameObject cell = Instantiate(button, new Vector3(w * space, h * space, 0), Quaternion.identity, gridPlaceholder.transform);

                grid[w, h] = cell.GetComponent<Button>();

                cell.name = $"Cell {w},{h}";

               Button cellButton = cell.GetComponent<Button>();

                cellButton.interactable = w < width && h == currentRow;

                cellButton.onClick.AddListener(OnButtonClicked);

                if (w == badCell)
                    cell.GetComponent<Image>().color = Color.red;
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
                grid[w, h].interactable = w < width && h == currentRow;
            }
        }
    }

    float GetBadCell()
    {
        return Random.Range(0, width);
    }
}
