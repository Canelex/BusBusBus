using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        // TODO: Convert to async perhaps?
    }

    public void LoadSceneIn(int index)
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + index);
    }
}
