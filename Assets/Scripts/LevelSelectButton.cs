using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public GameObject lockIcon;
    public bool unlockedByDefault;
    private bool levelUnlocked;

	void Start ()
    {
        // Check if the level should be unlocked.
        levelUnlocked = unlockedByDefault || BetterPrefs.GetBool(BetterPrefs.PREFIX_LEVEL_UNLOCKED + levelIndex, false);

        if (levelUnlocked)
        {
            lockIcon.gameObject.SetActive(false); // Hide lock icon on unlocked levels.
        }
	}

    public void TryToLoadLevel()
    {
        if (levelUnlocked)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            // TODO: Play some kind of rejection sound?
        }
    }
}
