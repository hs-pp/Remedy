using UnityEngine;
using Object = UnityEngine.Object;

namespace RemedySystem
{
    public static class Remedy
    {
        #region Log

        // [AutoStaticsCleanup] Unity 6.5 feature
        private static RemedyConfig m_config;
        public static RemedyConfig Config // Lazy load this rather than OnInitialize in case we need logging before SystemCore is ready.
        {
            get
            {
                if (m_config == null)
                {
                    RemedyConfig[] configs = Resources.LoadAll<RemedyConfig>("");
                    if (configs.Length == 0)
                    {
                        Debug.LogWarning($"[Remedy] Found no RemedyConfigs in Resources folder. Please create a new one. Using temp for now.");
                        m_config = ScriptableObject.CreateInstance<RemedyConfig>();
                    }
                    else if (configs.Length > 1)
                    {
                        Debug.LogWarning($"[Remedy] Found multiple RemedyConfigs in project. Loading the first one.");
                        m_config = configs[0];
                    }
                    else if (configs.Length == 1)
                    {
                        m_config = configs[0];
                    }
                }

                return m_config;
            }
        }

        [HideInCallstack] // <-- lol this shit doesnt work.
        public static void Log(RemedyType logType, LogVerbosity verbosity, object message, Object context = null)
        {
            if (Config == null || !Config.EnableRemedy)
            {
                return;
            }

            RemedyTypeSettings settings = Config.GetTypeSettings(logType);
            if (settings == null || settings.LogVerbosity < verbosity)
            {
                return;
            }

            Debug.Log($"{GetLogTypeHeader(settings)} {message}", context);
        }

        [HideInCallstack]
        public static void LogWarning(RemedyType logType, LogVerbosity verbosity, object message, Object context = null)
        {
            if (Config == null || !Config.EnableRemedy)
            {
                return;
            }

            RemedyTypeSettings settings = Config.GetTypeSettings(logType);
            if (settings == null || settings.LogVerbosity < verbosity)
            {
                return;
            }

            Debug.LogWarning($"{GetLogTypeHeader(settings)} {message}", context);
        }

        [HideInCallstack]
        public static void LogError(RemedyType logType, object message, Object context = null)
        {
            if (Config == null || !Config.EnableRemedy)
            {
                return;
            }

            RemedyTypeSettings settings = Config.GetTypeSettings(logType);
            if (settings == null)
            {
                return;
            }

            Debug.LogError($"{GetLogTypeHeader(settings)} {message}", context);
        }

        private static string GetLogTypeHeader(RemedyTypeSettings remedyTypeSettings)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(remedyTypeSettings.TypeColor)}>[{remedyTypeSettings.RemedyType}] </color>";
        }

        #endregion
    }
}