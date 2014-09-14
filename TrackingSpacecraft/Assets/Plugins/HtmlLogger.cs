using UnityEngine;

namespace HTMLEngine.Unity3D
{
    /// <summary>
    /// HTML logger to catch HTMLEngine messages
    /// </summary>
    public class HtmlLogger : HtLogger
    {
        /// <summary>
        /// Logger implementation.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="message">Message.</param>
        public override void Log(HtLogLevel level, string message)
        {
            switch (level)
            {
                case HtLogLevel.Debug:
                    Debug.Log("[DEBUG]" + message);break;
                case HtLogLevel.Info:
                    Debug.Log("[INFO]" + message);break;
                case HtLogLevel.Warning:
                    Debug.LogWarning("[WARN]" + message); break;
                case HtLogLevel.Error:
                    Debug.LogError("[ERROR]" + message); break;
            }
        }
    }
}