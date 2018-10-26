using UnityEngine;

public class BetterPrefs : PlayerPrefs
{
    public static string KEY_SOUNDS_ENABLED = "sounds-enabled";
    public static string KEY_TIPS_ENABLED = "tips-enabled";
    public static string KEY_LEVELS_UNLOCKED = "levels-unlocked";
    public static string PREFIX_LEVEL_COMPLETED = "level-completed-";

    public static bool GetBool(string key, bool defaultValue)
    {
        int value = PlayerPrefs.GetInt(key, -1);
        if (value == -1) return defaultValue; // Key was not stored.
        return value == 1;
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0); // 1:true, 0:false
    }
}