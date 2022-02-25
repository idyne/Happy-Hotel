namespace FateGames
{
    public static class AnalyticsManager
    {
        public static void Initialize()
        {
            //GameAnalytics.Initialize();
        }

        public static void ReportStartProgress()
        {
            TinySauce.OnGameStarted();
        }

        public static void ReportFinishProgress(bool success)
        {
            TinySauce.OnGameFinished(SaveSystem.PlayerData.Money);
        }
    }
}

