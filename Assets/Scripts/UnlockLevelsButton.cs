using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnlockLevelsButton : MonoBehaviour
{
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:

                int numLevels = 15; // Number of levels
                int numUnlocked = BetterPrefs.GetInt(BetterPrefs.KEY_LEVELS_UNLOCKED, 5);
                BetterPrefs.SetInt(BetterPrefs.KEY_LEVELS_UNLOCKED, Mathf.Min(numLevels, numUnlocked + 3));

                LevelSelectScript script = FindObjectOfType<LevelSelectScript>();
                if (script != null)
                {
                    script.UpdateContent();
                }
                // Unlock 3 levels.

                Debug.Log("Finished Ad!");
                break;
            case ShowResult.Skipped:
                Debug.Log("Skipped Ad...");
                break;
            case ShowResult.Failed:
                Debug.Log("Failed Ad...");
                break;
        }
    }

}
