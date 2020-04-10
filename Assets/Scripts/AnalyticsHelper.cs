using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsHelper {
    public static float time_startPlayingLevel;
    public static float time_startCreatingLevel;
    private static Dictionary<string, int> triesPerLevel = new Dictionary<string, int>();

    public static void AddTry(int level) {
        Debug.Log("AnalyticsHelper: adding a try");
        AddTry(level.ToString());
    }

    public static void AddTry(string level) {
        if (triesPerLevel.ContainsKey(level)) {
            triesPerLevel[level]++;
        } else {
            triesPerLevel[level] = 1;
        }
    }

    public static int GetTries(int level) => triesPerLevel[level.ToString()];

    public static void ResetTries(int level) => triesPerLevel[level.ToString()] = 0;
}