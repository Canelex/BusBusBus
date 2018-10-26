using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    public RectTransform content;
    public LevelSelectButton levelButtonPrefab;
    public UnlockLevelsButton unlockButtonPrefab;

    public void UpdateContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // How many levels are unlocked? (5 by default)
        int numLevels = BetterPrefs.GetInt(BetterPrefs.KEY_LEVELS_UNLOCKED, 5);

        // Resize the content box
        content.sizeDelta = new Vector2(content.sizeDelta.x, 264 + numLevels * 160);

        bool levelCompleted = true; // First level will be unlocked.
        for (int i = 0; i < numLevels; i++)
        {
            int levelIndex = i + 2;
            bool levelUnlocked = levelCompleted; // Previous level completed?
            levelCompleted = BetterPrefs.GetBool(BetterPrefs.PREFIX_LEVEL_COMPLETED + levelIndex, false);
            LevelSelectButton lsb = Instantiate(levelButtonPrefab, content.transform);
            lsb.init(levelIndex, levelUnlocked, levelCompleted);
            lsb.transform.localPosition = Vector2.down * (i * 160 + 132);
        }

        UnlockLevelsButton ulb = Instantiate(unlockButtonPrefab, content.transform);
        ulb.transform.localPosition = Vector2.down * (numLevels * 160 + 132);
    }

    void Start()
    {
        UpdateContent();
    }

}
