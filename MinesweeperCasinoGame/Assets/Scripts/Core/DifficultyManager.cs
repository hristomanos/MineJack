using System;
using System.Collections.Generic;
using Config;
using UnityEngine;
using UnityEngine.UI;
using WebGL;

namespace Core
{
    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField] DifficultyConfig[] difficultySettings;
        [SerializeField] private GameObject canvas;
        
        private readonly Dictionary<Difficulty, GridLayoutGroup> gridLayoutGoDictionary = new();
        private readonly Dictionary<Difficulty, DifficultyConfig> difficultySettingsDictionary = new();
        
        public Difficulty CurrentDifficulty { get; private set; } = Difficulty.Easy;
        
        public DifficultyConfig CurrentDifficultyConfig => difficultySettingsDictionary[CurrentDifficulty];
        public GridLayoutGroup CurrentGridLayoutGroup => gridLayoutGoDictionary[CurrentDifficulty];

        public void Initialize()
        {
            InstantiateDifficultySettings();
        }

        public void SelectDifficulty(int index)
        {
            if (Enum.IsDefined(typeof(Difficulty), index) == false)
            {
                Debug.LogError($"Invalid difficulty index: {index}");
                return;
            }
            
            var selectedDifficulty = (Difficulty) index;
            
            if (CurrentDifficulty == selectedDifficulty)
                return;
            
            CurrentDifficulty = selectedDifficulty;
            
            WebGLBridge.NotifyDifficultySelected(selectedDifficulty);
        }
        
        private void InstantiateDifficultySettings()
        {
            foreach (var setting in difficultySettings)
            {
                var gridLayout = Instantiate(setting.gridPrefab, canvas.transform).GetComponent<GridLayoutGroup>();
                
                gridLayoutGoDictionary.Add(setting.difficulty, gridLayout);
                difficultySettingsDictionary.Add(setting.difficulty, setting);
            }
        }
    }
}