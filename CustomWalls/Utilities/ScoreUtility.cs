using BS_Utils.Gameplay;
using System.Collections.Generic;

namespace CustomWalls.Utilities
{
    public class ScoreUtility
    {
        private static readonly IList<string> scoreBlockList = new List<string>();
        private static readonly object acquireLock = new object();

        public static bool ScoreIsBlocked { get; private set; } = false;

        internal static void DisableScoreSubmission(string BlockedBy)
        {
            lock (acquireLock)
            {
                if (!scoreBlockList.Contains(BlockedBy))
                {
                    scoreBlockList.Add(BlockedBy);
                }

                if (!ScoreIsBlocked)
                {
                    ScoreSubmission.ProlongedDisableSubmission(Plugin.PluginName);
                    ScoreIsBlocked = true;
                    Logger.log.Info("ScoreSubmission has been disabled.");
                }
            }
        }

        internal static void EnableScoreSubmission(string BlockedBy)
        {
            lock (acquireLock)
            {
                if (scoreBlockList.Contains(BlockedBy))
                {
                    scoreBlockList.Remove(BlockedBy);
                }

                if (ScoreIsBlocked && scoreBlockList.Count == 0)
                {
                    ScoreSubmission.RemoveProlongedDisable(Plugin.PluginName);
                    ScoreIsBlocked = false;
                    Logger.log.Info("ScoreSubmission has been re-enabled.");
                }
            }
        }

        /// <summary>
        /// Should only be called on exit!
        /// </summary>
        internal static void Cleanup()
        {
            lock (acquireLock)
            {
                if (ScoreIsBlocked)
                {
                    Logger.log.Info("Plugin is exiting, ScoreSubmission has been re-enabled.");
                    ScoreSubmission.RemoveProlongedDisable(Plugin.PluginName);
                    ScoreIsBlocked = false;
                }

                if (scoreBlockList.Count != 0)
                {
                    scoreBlockList.Clear();
                }
            }
        }
    }
}
