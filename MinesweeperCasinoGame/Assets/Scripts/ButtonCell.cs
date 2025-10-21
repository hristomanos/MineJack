using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCell : MonoBehaviour
{
    [SerializeField] private GameObject grenadeImage;
    [SerializeField] private GameObject keyImageContainer;
    [SerializeField] private Image keyImage;
    [SerializeField] private Image keyBackgroundImage;

    private ButtonType buttonType;
    private Button button;
    private Image buttonImage;

    private Color keyColor = Color.green;
    private Color bombColor = Color.red;

    public UnityEvent OnClick => button.onClick;

    public Action playerLoses;
    public Action playerFoundKey;

    private bool isActive;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        OnClick.AddListener(OnClicked);
    }

    public void Initialize(ButtonType buttonType, Action onPlayerLoses, Action onPlayerFoundKey)
    {
        playerFoundKey = onPlayerFoundKey;
        playerLoses = onPlayerLoses;
        this.buttonType = buttonType;

        ResetCell();
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    private void OnClicked()
    {
        isActive = true;

        if (buttonType == ButtonType.BOMB)
        {
            playerLoses?.Invoke();
        }
        else
        {
            playerFoundKey?.Invoke();
        }

        RevealType();
    }

    public void RevealType()
    {
        switch(buttonType)
        {
            case ButtonType.KEY:
                buttonImage.color = keyColor;
                keyImageContainer.SetActive(true);
                break;
            case ButtonType.BOMB:
                buttonImage.color = bombColor;
                SetDisabledButtonColourAlpha(0.9f);
                grenadeImage.SetActive(true);
                break;
        }
    }

    public void RevealHiddenType()
    {
        if(isActive)
            return;

        switch(buttonType)
        {
            case ButtonType.KEY:
                buttonImage.color = new Color(keyColor.r, keyColor.g, keyColor.b, 0.4f);
                keyImage.color = new Color(keyImage.color.r, keyImage.color.g, keyImage.color.b, 0.4f);
                keyBackgroundImage.color = new Color(keyBackgroundImage.color.r, keyBackgroundImage.color.g, keyBackgroundImage.color.b, 0.4f);
                keyImageContainer.SetActive(true);
                break;
        }
    }

    public void ResetCell()
    {
        buttonImage.color = Color.white;

        isActive = false;

        SetDisabledButtonColourAlpha(0.5f);

        keyImage.color = new Color(keyImage.color.r, keyImage.color.g, keyImage.color.b, 1f);
        keyBackgroundImage.color = new Color(keyBackgroundImage.color.r, keyBackgroundImage.color.g, keyBackgroundImage.color.b, 1f);

        keyImageContainer.SetActive(false);
        grenadeImage.SetActive(false);
    }

    private void SetDisabledButtonColourAlpha(float value)
    {
        var colours = button.colors;
        var disabledColour = colours.disabledColor;
        disabledColour.a = value;
        colours.disabledColor = disabledColour;
        button.colors = colours;
    }
}
