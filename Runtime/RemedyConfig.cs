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

        private RemedyTypeSettings m_defaultTypeSettings = new();
        private List<RemedyTypeSettings> m_typeSettingsList = new();

        [NonSerialized]
        private Dictionary<RemedyType, RemedyTypeSettings> m_cachedSettings = new();

        public RemedyTypeSettings GetTypeSettings(RemedyType type)
        {
            if (m_cachedSettings.Count == 0)
            {
                CacheSettings();
            }

            if (!m_cachedSettings.ContainsKey(type))
            {
                RemedyTypeSettings newSettings = new RemedyTypeSettings(type.ToString(), m_defaultTypeSettings);
                m_cachedSettings.Add(type, newSettings);
            }
            
            return m_cachedSettings[type];
        }

        private void CacheSettings()
        {
            m_cachedSettings.Clear();
            
            foreach (RemedyTypeSettings typeSettings in m_typeSettingsList)
            {
                if (!Enum.TryParse(typeSettings.RemedyType, out RemedyType remedyType))
                {
                    Debug.LogError($"Error parsing remedy type {typeSettings.RemedyType} in RemedyConfig!");
                    continue;
                }

                if (m_cachedSettings.ContainsKey(remedyType))
                {
                    Debug.LogWarning($"Found duplicate RemedyTypeSetting for RemedyType {typeSettings.RemedyType}. Skipping.");
                    continue;
                }
                
                m_cachedSettings.Add(remedyType, typeSettings);
            }
        }
    }
}