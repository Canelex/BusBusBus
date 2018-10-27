using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevelLoading : MonoBehaviour
{
    public void LoadLevel(int buildIndex)
    {
        // todo 1: check if the scene is unlocked
        // todo 2: async and splash screen if slow
        SceneManager.LoadScene(buildIndex);
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
