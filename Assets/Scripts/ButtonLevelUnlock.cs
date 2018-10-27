using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class ButtonLevelUnlock : MonoBehaviour
{
    public int levels;

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

                // Unlock some levels.
                int levelsUnlocked = BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
                BetterPrefs.SetInt(Globals.KEY_LEVELS_UNLOCKED, levelsUnlocked + levels);

                // Update canvas if it exists
                CanvasController canvas = FindObjectOfType<CanvasController>();
                if (canvas != null)
                {
                    canvas.UpdateVictoryUI(true);
                }

                // Update level select if it exists
                LevelSelectScript lselect = FindObjectOfType<LevelSelectScript>();
                if (lselect != null)
                {
                    lselect.UpdateContent();
                }

                break;
            case ShowResult.Skipped:
                Debug.Log("Ad was skipped.");
                break;
            case ShowResult.Failed:
                Debug.Log("Failed to show ad.");
                break;
        }
    }

}
