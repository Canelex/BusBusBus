using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int targetFrameRate;
    public Text textSounds;
    public Text textTips;
    private bool soundsEnabled;
    private bool tipsEnabled;

    void Start()
    {
        // Set the target framerate.
        if (Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        // Load SFX setting and update button.
        soundsEnabled = BetterPrefs.GetBool(Globals.KEY_SOUNDS_ENABLED, Globals.DEFAULT_SOUND_ENABLED);
        textSounds.text = "Sound: " + (soundsEnabled ? "On" : "Off");

        // Load hints setting and update button.
        tipsEnabled = BetterPrefs.GetBool(Globals.KEY_TIPS_ENABLED, Globals.DEFAULT_TIPS_ENABLED);
        textTips.text = "Tips: " + (tipsEnabled ? "On" : "Off");
    }

    public void ToggleSounds()
    {
        // Update SFX setting and update button.
        soundsEnabled = !soundsEnabled;
        textSounds.text = "Sound: " + (soundsEnabled ? "On" : "Off");
        BetterPrefs.SetBool(Globals.KEY_SOUNDS_ENABLED, soundsEnabled);
    }

    public void ToggleTips()
    {
        // Update hints button and update button.
        tipsEnabled = !tipsEnabled;
        textTips.text = "Tips: " + (tipsEnabled ? "On" : "Off");
        BetterPrefs.SetBool(Globals.KEY_TIPS_ENABLED, tipsEnabled);
    }

    public void Play()
    {
        SceneManager.LoadScene(1); // Load level select scene at build index 1.
    }

}
