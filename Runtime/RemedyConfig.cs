using UnityEngine;

namespace RemedySystem
{
    [CreateAssetMenu(fileName = "RemedyConfig", menuName = "Remedy/RemedyConfig")]
    public partial class RemedyConfig : ScriptableObject
    {
        [SerializeField]
        private bool m_useRemedy = true;
        public bool UseRemedy => m_useRemedy;
        
        [SerializeField]
        private bool m_disableAllLogs = false;
        public bool DisableAllLogs => m_disableAllLogs;

        public RemedyTypeSettings UnorganizedTypeSettings = new();
    }
}