using UnityEngine;

namespace WebGL
{
    public static class WebGLBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartGame();

    [DllImport("__Internal")]
    private static extern void SelectDifficulty(int difficulty);
#else
        public static void NotifyGameStarted()
        {
            Debug.Log("Would notify JavaScript: StartGame");
        }

        public static void NotifyDifficultySelected(int difficulty)
        {
            Debug.Log($"Would notify JavaScript: Difficulty Selected: {(Difficulty) difficulty}");
        }
#endif
    }
}