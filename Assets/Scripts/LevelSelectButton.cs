using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    private int levelIndex;
    private bool levelUnlocked;
    private bool levelCompleted;
    public Image levelIcon;
    public Text levelText;
    public Sprite lockIcon;
    public Sprite checkIcon;

    public void init(int index, bool unlocked, bool completed)
    {
        // Setup the button with information
        levelIndex = index;
        levelUnlocked = unlocked;
        levelCompleted = completed;

        // Update name and text component
        levelText.text = gameObject.name = "Level " + (levelIndex - 1);

        // Update level select button icon
        if (!levelUnlocked)
        {
            levelIcon.sprite = lockIcon; // Level locked, show lock icon
        }
        else if (levelCompleted)
        {
            levelIcon.sprite = checkIcon; // Level completed, show checkmark icon
        }
        else
        {
            levelIcon.gameObject.SetActive(false); // Level incomplete, show nothing
        }
    }

    public void LoadLevel()
    {
        if (levelUnlocked)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            // Play some kind of sound?
        }
    }


}
