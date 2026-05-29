using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCellView : MonoBehaviour
{
    [SerializeField] private GameObject grenadeImage;
    [SerializeField] private GameObject keyImageContainer;
    [SerializeField] private Image keyImage;
    [SerializeField] private Image keyBackgroundImage;
    private Image imageContainer;
    
    private readonly Color keyColor = Color.green;
    private readonly Color bombColor = Color.red;
    [SerializeField, Range(0, 1)] private float keyAlpha = 0.4f;
    [SerializeField, Range(0, 1)] private float disabledBombAlpha = 0.9f;
    [SerializeField, Range(0, 1)] private float defaultDisabledAlpha = 0.5f;
    

    CellType type;
    
    public void Initialize(CellType cellType)
    {
        type = cellType;
        
        imageContainer = GetComponent<Image>();
    }

    public void RevealSelectedType(Button button)
    {
        switch(type)
        {
            case CellType.Key:
                imageContainer.color = keyColor;
                keyImageContainer.SetActive(true);
                break;
        
            case CellType.Bomb:
                imageContainer.color = bombColor;
                SetDisabledButtonColourAlpha(disabledBombAlpha, button);
                grenadeImage.SetActive(true);
                break;
        
            default:
                Debug.LogError($"Button type {type} not supported");
                break;
        }
    }
    
    public void RevealHiddenKey()
    {
        if (type != CellType.Key)
            return;
    
        imageContainer.color = keyColor.WithAlpha(keyAlpha);
        keyImage.color = keyImage.color.WithAlpha(keyAlpha);
        keyBackgroundImage.color = keyBackgroundImage.color.WithAlpha(keyAlpha);
        keyImageContainer.SetActive(true);
    }
    
    private void SetDisabledButtonColourAlpha(float value, Button button)
    {
        var colours = button.colors;
        colours.disabledColor = colours.disabledColor.WithAlpha(value);
        button.colors = colours;
    }

    public void OnReset(Button button)
    {
        imageContainer.color = Color.white;
        SetDisabledButtonColourAlpha(defaultDisabledAlpha, button);
        
        keyImage.color = new Color(keyImage.color.r, keyImage.color.g, keyImage.color.b, 1f);
        keyBackgroundImage.color = new Color(keyBackgroundImage.color.r, keyBackgroundImage.color.g, keyBackgroundImage.color.b, 1f);
        
        keyImageContainer.SetActive(false);
        grenadeImage.SetActive(false);
    }
}