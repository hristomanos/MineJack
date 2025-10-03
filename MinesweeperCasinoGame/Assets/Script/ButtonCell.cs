using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCell : MonoBehaviour
{
    private ButtonType buttonType;
    private Button button;
    private Image image;

    private Color KeyColor = Color.green;
    private Color BombColor = Color.red;
    public UnityEvent OnClick => button.onClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        OnClick.AddListener(OnClicked);
    }


    public void Initialize(ButtonType buttonType)
    {

        this.buttonType = buttonType;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    private void OnClicked()
    {
        switch(buttonType)
        {
            case ButtonType.KEY:
                image.color = KeyColor;
                break;
            case ButtonType.BOMB:
                image.color = BombColor;
                break;
        }
    }
}
