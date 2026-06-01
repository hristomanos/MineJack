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
            difficultyDropdown.value = (int) initialDifficulty;
            
            var optionsList = new List<string> { "Easy", "Medium", "Hard", "Expert", "Master" };
            difficultyDropdown.AddOptions(optionsList);
        }
    }
}