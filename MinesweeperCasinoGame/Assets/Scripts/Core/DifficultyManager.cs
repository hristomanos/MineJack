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
            if (difficultySettings == null || difficultySettings.Length == 0)
            {
                Debug.LogError("No difficulty settings assigned.", this);
                return;
            }
            
            InstantiateDifficultySettings();
            SetActiveGridLayout(CurrentDifficulty);
        }
        
        public bool TrySelectDifficulty(int index)
        {
            if (Enum.IsDefined(typeof(Difficulty), index) == false)
            {
                Debug.LogError($"Invalid difficulty index: {index}");
                return false;
            }
            
            var selectedDifficulty = (Difficulty) index;
            
            if (CurrentDifficulty == selectedDifficulty)
                return false;
            
            CurrentDifficulty = selectedDifficulty;
            SetActiveGridLayout(CurrentDifficulty);
            
            WebGLBridge.NotifyDifficultySelected(selectedDifficulty);
            return true;
        }
        
        private void InstantiateDifficultySettings()
        {
            foreach (var setting in difficultySettings)
            {
                var gridLayout = Instantiate(setting.gridPrefab, canvas.transform).GetComponent<GridLayoutGroup>();

                if (difficultySettingsDictionary.ContainsKey(setting.difficulty))
                {
                    Debug.LogError($"Duplicate difficulty setting: {setting.difficulty}", this);
                    return;
                }
                
                gridLayoutGoDictionary.Add(setting.difficulty, gridLayout);
                difficultySettingsDictionary.Add(setting.difficulty, setting);
            }
        }
        
        private void SetActiveGridLayout(Difficulty difficulty)
        {
            foreach (var (key, value) in gridLayoutGoDictionary)
            {
                value.gameObject.SetActive(key == difficulty);
            }
        }
    }
}