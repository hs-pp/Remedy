using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RemedySystem.Editor
{
    public static class RemedySettingsProvider
    {
        private const string SETTINGS_PATH = "Project/Remedy";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new SettingsProvider(SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Remedy",
                activateHandler = (_, root) =>
                {
                    root.Clear();
                    root.Add(new SettingsProviderView());
                },
                keywords = new System.Collections.Generic.HashSet<string>(new[] { "Remedy", "Debug" })
            };
        }
    }
    
    public class SettingsProviderView : VisualElement
    {
        private const string UXML_PATH = "Remedy/RemedySettingsProviderUXML";
        private const string NO_CONFIG_FOUND_SCREEN_TAG = "no-config-found-screen";
        private const string CREATE_BUTTON_TAG = "create-button";
        private const string DISPLAY_CONFIG_SCREEN_TAG = "display-config-screen";
        private const string MULTIPLE_CONFIGS_FOUND_LABEL_TAG = "multi-config-found-label";
        private const string CONFIG_OBJECTFIELD_TAG = "config-objectfield";
        private const string REMEDY_CONFIG_EDITOR_CONTAINER_TAG = "remedy-config-editor-container";

        private VisualElement m_noConfigFoundScreen;
        private Button m_createButton;
        private VisualElement m_displayConfigScreen;
        private Label m_multipleConfigsFoundLabel;
        private ObjectField m_configObjectField;
        private VisualElement m_remedyEditorContainer;

        private UnityEditor.Editor m_configEditor;

        public SettingsProviderView()
        {
            CreateLayout();
            LoadRemedyConfig();
        }

        private void CreateLayout()
        {
            VisualTreeAsset uxmlAsset = Resources.Load<VisualTreeAsset>(UXML_PATH);
            uxmlAsset.CloneTree(this);
            
            m_noConfigFoundScreen = this.Q<VisualElement>(NO_CONFIG_FOUND_SCREEN_TAG);
            m_createButton = this.Q<Button>(CREATE_BUTTON_TAG);
            m_displayConfigScreen = this.Q<VisualElement>(DISPLAY_CONFIG_SCREEN_TAG);
            m_multipleConfigsFoundLabel = this.Q<Label>(MULTIPLE_CONFIGS_FOUND_LABEL_TAG);
            m_configObjectField = this.Q<ObjectField>(CONFIG_OBJECTFIELD_TAG);
            m_remedyEditorContainer = this.Q<VisualElement>(REMEDY_CONFIG_EDITOR_CONTAINER_TAG);

            style.marginLeft = 8;

            m_createButton.clicked += () =>
            {
                string assetPath = EditorUtility.SaveFilePanelInProject(
                    "Create RemedyConfig",
                    "RemedyConfig",
                    "asset",
                    "Create the RemedyConfig asset inside a Resources folder so Remedy can load it at runtime.",
                    "Assets/");

                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                if (!assetPath.Contains("/Resources/"))
                {
                    EditorUtility.DisplayDialog(
                        "Invalid Location",
                        "RemedyConfig must be created inside a Resources folder.",
                        "OK");
                    return;
                }

                RemedyConfig newConfig = ScriptableObject.CreateInstance<RemedyConfig>();
                AssetDatabase.CreateAsset(newConfig, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Selection.activeObject = newConfig;
                EditorGUIUtility.PingObject(newConfig);
                LoadRemedyConfig();
            };
        }

        private void LoadRemedyConfig()
        {
            RemedyConfig[] configs = Resources.LoadAll<RemedyConfig>("");

            m_noConfigFoundScreen.style.display = configs.Length == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            m_displayConfigScreen.style.display = configs.Length == 0 ? DisplayStyle.None : DisplayStyle.Flex;
            
            if (configs.Length > 0)
            {
                m_multipleConfigsFoundLabel.style.display = configs.Length > 1 ? DisplayStyle.Flex : DisplayStyle.None;
                RemedyConfig remedyConfig = configs[0];
                
                m_configObjectField.value = remedyConfig;
                
                m_remedyEditorContainer.Clear();
                m_configEditor = UnityEditor.Editor.CreateEditor(remedyConfig);
                m_remedyEditorContainer.Add(m_configEditor.CreateInspectorGUI());
            }
        }
    }
}
