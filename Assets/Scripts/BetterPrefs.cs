using UnityEngine;

public static class BetterPrefs
{
    public static string KEY_SOUNDS_ENABLED = "sounds-enabled";
    public static string KEY_HINTS_ENABLED = "hints-enabled";
    public static string PREFIX_LEVEL_UNLOCKED = "level-unlocked-";

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