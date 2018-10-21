using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int targetFrameRate;
    public Text textSounds;
    public Text textHints;
    private bool soundsEnabled;
    private bool hintsEnabled;

    void Start()
    {
        // Set the target framerate.
        if (Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        // Load SFX setting and update button.
        soundsEnabled = BetterPrefs.GetBool(BetterPrefs.KEY_SOUNDS_ENABLED, true);
        textSounds.text = "Sound: " + (soundsEnabled ? "On" : "Off");

        // Load hints setting and update button.
        hintsEnabled = BetterPrefs.GetBool(BetterPrefs.KEY_HINTS_ENABLED, true);
        textHints.text = "Hints: " + (hintsEnabled ? "On" : "Off");
    }

    public void ToggleSFX()
    {
        // Update SFX setting and update button.
        soundsEnabled = !soundsEnabled;
        textSounds.text = "Sound: " + (soundsEnabled ? "On" : "Off");
        BetterPrefs.SetBool(BetterPrefs.KEY_SOUNDS_ENABLED, soundsEnabled);
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
