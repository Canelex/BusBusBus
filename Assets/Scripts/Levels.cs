static class Levels
{
    private static int FIRST_LEVEL_BUILD_INDEX = 2;
    public static int NUM_LEVELS = 20;

    public static bool IsLevelCompleted(int level)
    {
        return BetterPrefs.GetBool(Globals.PREFIX_LEVEL_COMPLETED + level, false);
    }

    public static void SetLevelCompleted(int level)
    {
        BetterPrefs.SetBool(Globals.PREFIX_LEVEL_COMPLETED + level, true);
    }

    public static bool IsLevelUnlocked(int level)
    {
        if (level <= 0) return true; // This is not a level, unlocked.
        return level <= BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
    }

    public static void UnlockLevels(int levels)
    {
        int unlocked = BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
        unlocked += levels;
        if (unlocked > NUM_LEVELS) unlocked = NUM_LEVELS;
        BetterPrefs.SetInt(Globals.KEY_LEVELS_UNLOCKED, unlocked);
    }

    public static int GetLevelFromBuildIndex(int buildIndex)
    {
        return (buildIndex - FIRST_LEVEL_BUILD_INDEX) + 1;
    }

    public static int GetBuildIndexFromLevel(int level)
    {
        return (FIRST_LEVEL_BUILD_INDEX + level) - 1;
    }
}