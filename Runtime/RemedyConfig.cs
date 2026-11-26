using System;
using System.Collections.Generic;
using UnityEngine;

namespace RemedySystem
{
    public class RemedyConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private bool m_enableRemedy = true;
        public bool EnableRemedy => m_enableRemedy;

        [SerializeField]
        private RemedyTypeSettings m_defaultTypeSettings = new();
        [SerializeField]
        private List<RemedyTypeSettings> m_typeSettings = new();

        [NonSerialized]
        private Dictionary<string, RemedyTypeSettings> m_cachedSettings;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        /// <summary>
        /// Any deserialize (inspector edit, undo, reimport) can change the type settings, so rebuild the cache on next use.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            m_cachedSettings = null;
        }

        public RemedyTypeSettings GetTypeSettings(RemedyType type)
        {
            if (m_cachedSettings == null)
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
            m_cachedSettings = new Dictionary<string, RemedyTypeSettings>();
            
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
