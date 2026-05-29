using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "DifficultySettings", menuName = "Config/DifficultySettings", order = 1)]
    public class DifficultyConfig : ScriptableObject
    {
        public Difficulty difficulty;
        [Min(1)] public int width = 1;
        [Min(1)] public int height = 1;
        [Min(0)] public int bombsPerRow = 0;
        public GameObject gridPrefab;
    }
}