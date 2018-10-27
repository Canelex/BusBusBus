using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
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
        int levelsUnlocked = BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
        int totalLevels = SceneManager.sceneCountInBuildSettings - 2;

        bool previousCompleted = true;
        for (int level = 0; level < levelsUnlocked; level++)
        {
            if (level >= totalLevels)
            {
                previousCompleted = false;
                break;
            }

            bool levelUnlocked = previousCompleted;
            bool levelCompleted = BetterPrefs.GetBool(Globals.PREFIX_LEVEL_COMPLETED + (level + 2), false);
           
            ButtonLevelSelect levelSelect = Instantiate(levelButtonPrefab, content.transform);
            levelSelect.init(level + 2, levelUnlocked, levelCompleted);
            levelSelect.transform.localPosition = Vector2.down * (132 + 160 * level);

            previousCompleted = levelCompleted;
        }

        if (previousCompleted) // Show unlock button when level reached.
        {
            ButtonLevelUnlock levelUnlock = Instantiate(unlockButtonPrefab, content.transform);
            levelUnlock.transform.localPosition = Vector2.down * (levelsUnlocked * 160 + 132);
            content.sizeDelta = new Vector2(content.sizeDelta.x, 264 + levelsUnlocked * 160);
        }
        else
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, 264 + (levelsUnlocked - 1) * 160);
        }
    }
}
