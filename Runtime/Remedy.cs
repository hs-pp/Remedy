using RemedySystem;
using SystemCoreSystem;
using UnityEngine;
using Object = UnityEngine.Object;

public class Remedy : AKeySystem
{
    #region Log
    private static string ConfigPath = "Remedy/RemedyConfig";
    private static RemedyConfig m_config;
    public static RemedyConfig Config // Lazy load this rather than OnInitialize in case we need logging before SystemCore is ready.
    {
        get
        {
            if (m_config == null)
            {
                m_config = Resources.Load<RemedyConfig>(ConfigPath);
                if (m_config == null)
                {
                    // Only scenario where we can't use Remedy.Log lol.
                    Debug.LogError($"[Remedy] Could not find config at path: \".../Resource/{ConfigPath}\"");
                }
            }

            return m_config;
        }
    }

    [HideInCallstack] // <-- lol this shit doesnt work.
    public static void Log(RemedyType logType, LogVerbosity verbosity, object message, Object context = null)
    {
        if (Config == null || !Config.UseRemedy)
        {
            Debug.Log(message, context);
            return;
        }

        if (Config.DisableAllLogs)
        {
            return;
        }

        RemedyTypeSettings settings = logType.GetSettings(Config);
        if (settings == null || settings.LogVerbosity < verbosity)
        {
            return;
        }

        Debug.Log($"{GetLogTypeHeader(settings, logType)} {message}", context);
    }

    [HideInCallstack]
    public static void LogWarning(RemedyType logType, LogVerbosity verbosity, object message, Object context = null)
    {
        if (Config == null || !Config.UseRemedy)
        {
            Debug.LogWarning(message, context);
            return;
        }

        if (Config.DisableAllLogs)
        {
            return;
        }

        RemedyTypeSettings settings = logType.GetSettings(Config);
        if (settings == null || settings.LogVerbosity < verbosity)
        {
            return;
        }

        Debug.LogWarning($"{GetLogTypeHeader(settings, logType)} {message}", context);
    }

    [HideInCallstack]
    public static void LogError(RemedyType logType, object message, Object context = null)
    {
        if (Config == null || !Config.UseRemedy)
        {
            Debug.LogError(message, context);
            return;
        }

        if (Config.DisableAllLogs)
        {
            return;
        }

        RemedyTypeSettings settings = logType.GetSettings(Config);
        if (settings == null)
        {
            return;
        }

        Debug.LogError($"{GetLogTypeHeader(settings, logType)} {message}", context);
    }

    private static string GetLogTypeHeader(RemedyTypeSettings remedyTypeSettings, RemedyType remedyType)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(remedyTypeSettings.TypeColor)}>[{remedyType.TypeName}] </color>";
    }

    #endregion
    
}