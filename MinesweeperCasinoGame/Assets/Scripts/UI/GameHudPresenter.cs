using Core;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(GameHudView))]
    public class GameHudPresenter : MonoBehaviour
    {
        [SerializeField] private GameHudView view;
        
        public UnityEvent OnBetButtonClickedEvent => view.OnBetButtonClickedEvent;
        public UnityEvent<int> OnDifficultyChangedEvent => view.OnDifficultyChangedEvent;
        
        private Difficulty initialDifficulty = Difficulty.Easy;
        
        private void Awake()
        {
            if (view == null) 
                view = GetComponent<GameHudView>();
            
            view.Initialize(initialDifficulty);
        }

        public void SetInteractable(bool interactable)
        {
            view.SetBetButtonInteractable(interactable);
            view.SetDifficultyDropdownInteractable(interactable);
        }
    }
}