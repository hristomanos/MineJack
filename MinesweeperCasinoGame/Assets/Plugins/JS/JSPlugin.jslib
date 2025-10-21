mergeInto(LibraryManager.library, {
  SelectDifficulty: function (difficultyIndex) {
    console.log("📤 Difficulty selected:", difficultyIndex);
    // Call Unity method on a GameObject named "GridManager"
    unityInstance.SendMessage("GridManager", "OnDifficultyChanged", difficultyIndex);
  },
  StartGame: function () {
  console.log("Bet button was clicked! Start the game!");
  unityInstance.SendMessage("GridManager","OnBetButtonClicked");
  }
});
