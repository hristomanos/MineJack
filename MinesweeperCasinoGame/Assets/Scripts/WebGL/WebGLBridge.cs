using Core;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace WebGL
{
    public static class WebGLBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartGame();

    [DllImport("__Internal")]
    private static extern void SelectDifficulty(Difficulty difficulty);
#endif
        public static void NotifyGameStarted()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StartGame();
#else
            Debug.Log("Would notify JavaScript: StartGame");
#endif
        }

        public static void NotifyDifficultySelected(Difficulty difficulty)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SelectDifficulty(difficulty);
#else
            Debug.Log($"Would notify JavaScript: Difficulty Selected: {difficulty}");
#endif
        }
    }
}