using System;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    
    public class GameHudView : MonoBehaviour
    {
        [SerializeField] private Button betButton;
        [SerializeField] private TMP_Dropdown difficultyDropdown;
        
        public UnityEvent OnBetButtonClickedEvent => betButton.onClick;
        public UnityEvent<int> OnDifficultyChangedEvent => difficultyDropdown.onValueChanged;

        private Difficulty initialDifficulty;

        public void Initialize(Difficulty difficulty)
        {
            initialDifficulty = difficulty;
            
            difficultyDropdown.ClearOptions();
            
            var optionsList = new List<string> { "Easy", "Medium", "Hard", "Expert", "Master" };
            difficultyDropdown.AddOptions(optionsList);
            
            difficultyDropdown.value = (int) initialDifficulty;
            difficultyDropdown.RefreshShownValue();
        }
        
        public void SetBetButtonInteractable(bool interactable)
        {
            betButton.interactable = interactable;
        }
        
        public void SetDifficultyDropdownInteractable(bool interactable)
        {
            difficultyDropdown.interactable = interactable;
        }

        private void OnValidate()
        {
            if (betButton == null) 
                Debug.LogWarning($"{nameof(GameHudView)}: betButton is not assigned in the inspector.", this);
            
            if (difficultyDropdown == null) 
                Debug.LogWarning($"{nameof(GameHudView)}: difficultyDropdown is not assigned in the inspector.", this);
        }
    }
}