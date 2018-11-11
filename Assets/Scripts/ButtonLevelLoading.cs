using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevelLoading : MonoBehaviour
{
    public void LoadLevel(int buildIndex)
    {
        // todo: async and splash screen if slow?
        // Is the level unlocked?
        if (Levels.IsLevelUnlocked(Levels.GetLevelFromBuildIndex(buildIndex)))
        {
            SceneManager.LoadScene(buildIndex);
        }
    }

    public void LoadLevelIn(int levels)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(currentIndex + levels);
    }

    public void ReloadLevel()
    {
        LoadLevelIn(0); // Load level with offset 0
    }
}
