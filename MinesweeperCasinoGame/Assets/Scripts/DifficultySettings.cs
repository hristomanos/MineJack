using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DifficultySettings
{
    public Difficulty difficulty;
    [Min(1)] public int width;
    [Min(1)] public int height;
    [Min(0)] public int bombsPerRow;
    public GridLayoutGroup gridLayoutGroup;
}