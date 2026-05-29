using UnityEngine;
using UnityEngine.UI;

namespace Grid.ButtonCell
{
    [RequireComponent(typeof(Image))]
    public class ButtonCellView : MonoBehaviour
    {
        [SerializeField] private GameObject grenadeImage;
        [SerializeField] private GameObject keyImageContainer;
        [SerializeField] private Image keyImage;
        [SerializeField] private Image keyBackgroundImage;
        [SerializeField] private Image cellImage;
    
        [SerializeField] private  Color keyColor = Color.green;
        [SerializeField] private  Color bombColor = Color.red;
        [SerializeField, Range(0, 1)] private float keyAlpha = 0.4f;
        [SerializeField, Range(0, 1)] private float disabledBombAlpha = 0.9f;
        [SerializeField, Range(0, 1)] private float defaultDisabledAlpha = 0.5f;
        
        private CellType type;
    
        public void Initialize(CellType cellType)
        {
            type = cellType;
        }

        public void RevealSelectedType(Button button)
        {
            switch(type)
            {
                case CellType.Key:
                    cellImage.color = keyColor;
                    keyImageContainer.SetActive(true);
                    break;
        
                case CellType.Bomb:
                    cellImage.color = bombColor;
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

            cellImage.color = WithAlpha(keyColor, keyAlpha);
            keyImage.color = WithAlpha(keyImage.color, keyAlpha);
            keyBackgroundImage.color = WithAlpha(keyBackgroundImage.color, keyAlpha);
            keyImageContainer.SetActive(true);
        }
    
        private void SetDisabledButtonColourAlpha(float value, Button button)
        {
            var colours = button.colors;
            colours.disabledColor = WithAlpha(colours.disabledColor, value);
            button.colors = colours;
        }

        public void OnReset(Button button)
        {
            cellImage.color = Color.white;
            SetDisabledButtonColourAlpha(defaultDisabledAlpha, button);

            keyImage.color = WithAlpha(keyImage.color, 1f);
            keyBackgroundImage.color = WithAlpha(keyBackgroundImage.color, 1f);

            keyImageContainer.SetActive(false);
            grenadeImage.SetActive(false);
        }

        private static Color WithAlpha(Color color, float alpha) => new(color.r, color.g, color.b, alpha);
    }
}