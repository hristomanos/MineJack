using System;
using UnityEngine;
using UnityEngine.UI;

namespace Grid.ButtonCell
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class ButtonCellPresenter : MonoBehaviour
    {
        [SerializeField] private ButtonCellView view;
    
        private Button button;
    
        private Action playerLoses;
        private Action playerFoundKey;

        private bool isRevealed;
    
        public CellType Type { get; private set; }

        private void Awake()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(OnClicked);
        }

        public void Initialize(CellType cellType, Action onPlayerLoses, Action onPlayerFoundKey)
        {
            playerFoundKey = onPlayerFoundKey;
            playerLoses = onPlayerLoses;
        
            Type = cellType;
            view.Initialize(cellType);
        
            ResetCell();
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        private void OnClicked()
        {
            isRevealed = true;

            if (Type == CellType.Bomb)
            {
                playerLoses?.Invoke();
            }
            else
            {
                playerFoundKey?.Invoke();
            }

            view.RevealSelectedType(button);
        }

        public void RevealUnselectedType()
        {
            if(isRevealed)
                return;

            view.RevealHiddenKey();
        }

        private void ResetCell()
        {
            isRevealed = false;
            view.OnReset(button);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClicked);
        }
    }
}
