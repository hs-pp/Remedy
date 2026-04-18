using System;
using System.Collections.Generic;
using UnityEngine;

namespace RemedySystem
{
    [CreateAssetMenu(fileName = "RemedyConfig", menuName = "Remedy/RemedyConfig")]
    public class RemedyConfig : ScriptableObject
    {
        [SerializeField]
        private bool m_enableRemedy = true;
        public bool EnableRemedy => m_enableRemedy;

        [SerializeField]
        private RemedyTypeSettings m_defaultTypeSettings = new();
        [SerializeField]
        private List<RemedyTypeSettings> m_typeSettings = new();

        [NonSerialized]
        private Dictionary<string, RemedyTypeSettings> m_cachedSettings = new();

        public RemedyTypeSettings GetTypeSettings(RemedyType type)
        {
            if (m_cachedSettings.Count == 0)
            {
                CacheSettings();
            }

            if (!m_cachedSettings.ContainsKey(type.Name))
            {
                m_cachedSettings.Add(type.Name, m_defaultTypeSettings);
            }
            
            return m_cachedSettings[type.Name];
        }

        private void CacheSettings()
        {
            m_cachedSettings.Clear();
            
            foreach (RemedyTypeSettings typeSettings in m_typeSettings)
            {
                if (m_cachedSettings.ContainsKey(typeSettings.RemedyType))
                {
                    Debug.LogWarning($"Found duplicate RemedyTypeSetting for RemedyType {typeSettings.RemedyType}. Skipping.");
                    continue;
                }
                
                m_cachedSettings.Add(typeSettings.RemedyType, typeSettings);
            }
        }
#if UNITY_EDITOR
        public const string ENABLE_REMEDY_VARNAME = "m_enableRemedy";
        public const string DEFAULT_TYPE_SETTINGS_VARNAME = "m_defaultTypeSettings";
        public const string TYPE_SETTINGS_VARNAME = "m_typeSettings";
#endif
    }
}
