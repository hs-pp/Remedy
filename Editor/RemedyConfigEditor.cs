using RemedySystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Remedy.Editor
{
    [CustomEditor(typeof(RemedyConfig))]
    public class RemedyConfigEditor : UnityEditor.Editor
    {
        private const string UXML_PATH = "Remedy/RemedyConfigUXML";
        private const string ENABLE_REMEDY_FIELD_TAG = "enable-remedy-field";
        private const string DEFAULT_TYPE_SETTINGS_CONTAINER_TAG = "default-type-settings-container";
        private const string TYPE_SETTINGS_LISTVIEW_TAG = "type-settings-listview";
        
        private PropertyField m_enableRemedyField;
        private VisualElement m_defaultTypeSettingsContainer;
        private ListView m_typeSettingsListView;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            CreateLayout(root);
            return root;
        }

        private void CreateLayout(VisualElement root)
        {
            var uxmlAsset = Resources.Load<VisualTreeAsset>(UXML_PATH);
            uxmlAsset.CloneTree(root);
            
            m_enableRemedyField = root.Q<PropertyField>(ENABLE_REMEDY_FIELD_TAG);
            m_enableRemedyField.BindProperty(serializedObject.FindProperty(RemedyConfig.ENABLE_REMEDY_VARNAME));
            
            m_defaultTypeSettingsContainer = root.Q(DEFAULT_TYPE_SETTINGS_CONTAINER_TAG);
            m_defaultTypeSettingsContainer.Add(CreateTypeSettingsFields(serializedObject.FindProperty(RemedyConfig.DEFAULT_TYPE_SETTINGS_VARNAME)));
            
            m_typeSettingsListView = root.Q<ListView>(TYPE_SETTINGS_LISTVIEW_TAG);
            m_typeSettingsListView.BindProperty(serializedObject.FindProperty(RemedyConfig.TYPE_SETTINGS_VARNAME));
            m_typeSettingsListView.Rebuild();
            m_typeSettingsListView.onAdd += OnAddTypeSetting;
        }

        private void OnAddTypeSetting(BaseListView obj)
        {
            Debug.Log("lol");
        }

        private static VisualElement CreateTypeSettingsFields(SerializedProperty typeSettingsProperty)
        {
            var container = new VisualElement();
            container.Add(new PropertyField(typeSettingsProperty.FindPropertyRelative(RemedyTypeSettings.LOG_VERBOSITY_VARNAME)));
            container.Add(new PropertyField(typeSettingsProperty.FindPropertyRelative(RemedyTypeSettings.TYPE_COLOR_VARNAME)));
            return container;
        }
    }
}
