using System;
using System.Collections.Generic;
using System.Linq;
using RemedySystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

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
            m_defaultTypeSettingsContainer.Add(new PropertyField(serializedObject.FindProperty(RemedyConfig.DEFAULT_TYPE_SETTINGS_VARNAME)));

            m_typeSettingsListView = root.Q<ListView>(TYPE_SETTINGS_LISTVIEW_TAG);
            m_typeSettingsListView.BindProperty(serializedObject.FindProperty(RemedyConfig.TYPE_SETTINGS_VARNAME));
            m_typeSettingsListView.Rebuild();
            m_typeSettingsListView.onAdd += OnAddTypeSetting;
        }

        private void OnAddTypeSetting(BaseListView obj)
        {
            Rect rect = new Rect()
            {
                x = obj.worldBound.position.x + obj.worldBound.size.x - CreateTypeSettingContentPopup.SCREEN_WIDTH,
                y = obj.worldBound.position.y + obj.worldBound.size.y,
            };
            PopupWindow.Show(rect, new CreateTypeSettingContentPopup(GetRemedyTypesWithNoSettings(), OnAddTypeSetting));
        }

        private List<string> GetRemedyTypesWithNoSettings()
        {
            List<string> remedyTypes = Enum.GetNames(typeof(RemedyType)).ToList();

            SerializedProperty typeSettingsSP = serializedObject.FindProperty(RemedyConfig.TYPE_SETTINGS_VARNAME);
            for (int i = 0; i < typeSettingsSP.arraySize; i++)
            {
                remedyTypes.Remove(typeSettingsSP.GetArrayElementAtIndex(i).FindPropertyRelative(RemedyTypeSettings.REMEDY_TYPE_VARNAME).stringValue);
            }
            
            return remedyTypes;
        }
        
        private void OnAddTypeSetting(string remedyType)
        {
            if (string.IsNullOrEmpty(remedyType))
            {
                return;
            }

            serializedObject.Update();
            
            // Keep the list ordered to match the RemedyType enum.
            if (!Enum.TryParse(remedyType, out RemedyType newRemedyType))
            {
                Debug.LogError($"Could not parse remedy type '{remedyType}' when adding RemedyTypeSettings.");
                return;
            }
            
            SerializedProperty typeSettingsSP = serializedObject.FindProperty(RemedyConfig.TYPE_SETTINGS_VARNAME);
            int insertIndex = typeSettingsSP.arraySize;
            for (int i = 0; i < typeSettingsSP.arraySize; i++)
            {
                SerializedProperty existingTypeProperty = typeSettingsSP
                    .GetArrayElementAtIndex(i)
                    .FindPropertyRelative(RemedyTypeSettings.REMEDY_TYPE_VARNAME);

                if (!Enum.TryParse(existingTypeProperty.stringValue, out RemedyType existingRemedyType))
                {
                    continue;
                }

                if (existingRemedyType > newRemedyType)
                {
                    insertIndex = i;
                    break;
                }
            }

            // create new element
            typeSettingsSP.InsertArrayElementAtIndex(insertIndex);

            // copy settings over from default
            SerializedProperty newTypeSettingsSP = typeSettingsSP.GetArrayElementAtIndex(insertIndex);
            SerializedProperty defaultTypeSettingsSP = serializedObject.FindProperty(RemedyConfig.DEFAULT_TYPE_SETTINGS_VARNAME);

            newTypeSettingsSP.FindPropertyRelative(RemedyTypeSettings.REMEDY_TYPE_VARNAME).stringValue = remedyType;
            newTypeSettingsSP.FindPropertyRelative(RemedyTypeSettings.LOG_VERBOSITY_VARNAME).enumValueIndex =
                defaultTypeSettingsSP.FindPropertyRelative(RemedyTypeSettings.LOG_VERBOSITY_VARNAME).enumValueIndex;
            newTypeSettingsSP.FindPropertyRelative(RemedyTypeSettings.TYPE_COLOR_VARNAME).colorValue =
                defaultTypeSettingsSP.FindPropertyRelative(RemedyTypeSettings.TYPE_COLOR_VARNAME).colorValue;

            // cleanup
            serializedObject.ApplyModifiedProperties();
            m_typeSettingsListView.Rebuild();
        }
    }

    public class CreateTypeSettingContentPopup : PopupWindowContent
    {
        public const float SCREEN_WIDTH = 300;
        public const float SCREEN_HEIGHT = 44;
        
        private const string UXML_PATH = "Remedy/CreateTypeSettingUXML";
        private const string REMEDY_TYPE_DROPDOWN_TAG = "remedy-type-dropdown";
        private const string ADD_BUTTON_TAG = "add-button";
        
        private DropdownField m_remedyTypeDropdown;
        private Button m_addButton;
        private List<string> m_remedyTypes;
        private Action<string> m_onAddCallback;

        public CreateTypeSettingContentPopup(List<string> remedyTypes, Action<string> onAddCallback)
        {
            m_remedyTypes = remedyTypes;
            m_onAddCallback = onAddCallback;
        }

        public override VisualElement CreateGUI()
        {
            VisualElement root = new VisualElement();
            var uxmlAsset = Resources.Load<VisualTreeAsset>(UXML_PATH);
            uxmlAsset.CloneTree(root);
            
            m_remedyTypeDropdown = root.Q<DropdownField>(REMEDY_TYPE_DROPDOWN_TAG);
            m_remedyTypeDropdown.choices = m_remedyTypes;
            m_addButton = root.Q<Button>(ADD_BUTTON_TAG);
            m_addButton.clicked += OnAddPressed;
            
            root.style.width = SCREEN_WIDTH;
            root.style.height = SCREEN_HEIGHT;
            return root;
        }
        
        private void OnAddPressed()
        {
            if (string.IsNullOrEmpty(m_remedyTypeDropdown.value))
            {
                return;
            }
            
            m_onAddCallback?.Invoke(m_remedyTypeDropdown.value);
            editorWindow.Close();
        }
    }
}
