using System;
using UnityEngine;

namespace RemedySystem
{
    [Serializable]
    public class RemedyTypeSettings
    {
        [SerializeField]
        private string m_remedyType;
        public string RemedyType => m_remedyType;
        
        [SerializeField]
        private LogVerbosity m_logVerbosity = LogVerbosity.Normal;
        public LogVerbosity LogVerbosity => m_logVerbosity;
        
        [SerializeField]
        private Color m_typeColor = Color.gray;
        public Color TypeColor => m_typeColor;

        public RemedyTypeSettings() {}
        public RemedyTypeSettings(string remedyType, RemedyTypeSettings defaultSettings)
        {
            m_remedyType = remedyType;
            m_logVerbosity = defaultSettings.LogVerbosity;
            m_typeColor = defaultSettings.TypeColor;
        }
    }

    [Serializable]
    public enum LogVerbosity : int
    {
        Quiet = 0,
        Minimal = 1,
        Normal = 2,
        Verbose = 3,
        Debug = 4,
    }
}