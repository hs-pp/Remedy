using System;
using UnityEngine;

namespace RemedySystem
{
    [Serializable]
    public class RemedyTypeSettings
    {
        [SerializeField]
        private LogVerbosity m_logVerbosity = LogVerbosity.Normal;
        public LogVerbosity LogVerbosity => m_logVerbosity;
        
        [SerializeField]
        private Color m_typeColor = Color.gray;
        public Color TypeColor => m_typeColor;
    }

    [Serializable]
    public enum LogVerbosity : int
    {
        Quiet = 0, // Don't log using this one.
        Minimal = 1,
        Normal = 2,
        Detailed = 3,
        Debug = 4,
    }
}