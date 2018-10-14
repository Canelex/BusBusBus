using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public Image lockIcon;
    public Sprite spriteLocked;
    public Sprite spriteUnlocked;
    private bool levelLocked;

	void Start ()
    {
        levelLocked = PlayerPrefs.GetInt("LevelCompleted" + levelIndex) == 0;

        if (levelIndex == 1)
        {
            levelLocked = false;
        }

        if (levelLocked)
        {
            lockIcon.sprite = spriteLocked;
        }
        else
        {
            lockIcon.sprite = spriteUnlocked;
        }
	}

    public void TryToLoadLevel()
    {
        if (!levelLocked)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            // Play some kind of rejection sound?
        }
    }
}
