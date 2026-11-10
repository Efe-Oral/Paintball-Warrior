using UnityEngine;

public static class LevelProgress
{
    private const string StarsKeyPrefix = "level_stars_";

    public static int GetStars(int levelIndex)
    {
        return PlayerPrefs.GetInt(StarsKeyPrefix + levelIndex, 0);
    }

    public static bool IsUnlocked(int levelIndex)
    {
        return levelIndex == 0 || GetStars(levelIndex - 1) > 0;
    }

    public static void ReportResult(int levelIndex, int stars)
    {
        int best = Mathf.Max(GetStars(levelIndex), stars);
        PlayerPrefs.SetInt(StarsKeyPrefix + levelIndex, best);
        PlayerPrefs.Save();
    }
}
