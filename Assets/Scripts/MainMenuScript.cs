using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int targetFrameRate;
    public Text textSFX;
    public Text textHints;
    private bool sfxEnabled;
    private bool hintsEnabled;

    void Start()
    {
        // Set the target framerate.
        if (Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        // Load SFX setting and update button.
        sfxEnabled = BetterPrefs.GetBool(BetterPrefs.KEY_SFX_ENABLED, true);
        textSFX.text = "SFX: " + (sfxEnabled ? "On" : "Off");

        // Load hints setting and update button.
        hintsEnabled = BetterPrefs.GetBool(BetterPrefs.KEY_HINTS_ENABLED, true);
        textHints.text = "Hints: " + (hintsEnabled ? "On" : "Off");
    }

    public void ToggleSFX()
    {
        // Update SFX setting and update button.
        sfxEnabled = !sfxEnabled;
        textSFX.text = "SFX: " + (sfxEnabled ? "On" : "Off");
        BetterPrefs.SetBool(BetterPrefs.KEY_SFX_ENABLED, sfxEnabled);
    }

    public void ToggleHints()
    {
        // Update hints button and update button.
        hintsEnabled = !hintsEnabled;
        textHints.text = "Hints: " + (hintsEnabled ? "On" : "Off");
        BetterPrefs.SetBool(BetterPrefs.KEY_HINTS_ENABLED, hintsEnabled);
    }

    public void Play()
    {
        SceneManager.LoadScene(1); // Load level select scene at build index 1.
    }

}
