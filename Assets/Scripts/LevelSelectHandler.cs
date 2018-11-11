using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectHandler : MonoBehaviour
{
    public RectTransform content;
    public ButtonLevelSelect levelButtonPrefab;
    public ButtonLevelUnlock unlockButtonPrefab;

    void Start()
    {
        UpdateContent();
    }

    public void UpdateContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // How many levels are unlocked? (5 by default)
        int levelsToDisplay = BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
        if (levelsToDisplay > Levels.NUM_LEVELS) levelsToDisplay = Levels.NUM_LEVELS;

        bool previousCompleted = true;
        for (int level = 1; level <= levelsToDisplay; level++)
        {
            // Check if the level is unlocked/completed/etc.
            bool levelUnlocked = previousCompleted;
            bool levelCompleted = Levels.IsLevelCompleted(level);

            // Create button object.
            ButtonLevelSelect button = Instantiate(levelButtonPrefab, content.transform);
            button.init(level, levelUnlocked, levelCompleted);
            button.transform.localPosition = Vector2.down * (160 * level - 28);

            // Mark level as completed? for next one.
            previousCompleted = levelCompleted;
        }

        // Player has finished all his levels and there are more to unlock.
        if (previousCompleted && levelsToDisplay != Levels.NUM_LEVELS)
        {
            ButtonLevelUnlock button = Instantiate(unlockButtonPrefab, content.transform);
            button.transform.localPosition = Vector2.down * (levelsToDisplay * 160 + 132);
            content.sizeDelta = new Vector2(content.sizeDelta.x, 264 + levelsToDisplay * 160);
        }
        else
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, 104 + levelsToDisplay * 160);
        }
    }
}
