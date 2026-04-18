using RemedySystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(RemedyTypeSettings))]
public class RemedyTypeSettingsDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new VisualElement();
        
        if (!string.IsNullOrEmpty(property.FindPropertyRelative(RemedyTypeSettings.REMEDY_TYPE_VARNAME).stringValue))
        {
            PropertyField remedyTypeField = new PropertyField(property.FindPropertyRelative(RemedyTypeSettings.REMEDY_TYPE_VARNAME));
            remedyTypeField.SetEnabled(false);
            container.Add(remedyTypeField);   
        }
        
        container.Add(new PropertyField(property.FindPropertyRelative(RemedyTypeSettings.LOG_VERBOSITY_VARNAME)));
        container.Add(new PropertyField(property.FindPropertyRelative(RemedyTypeSettings.TYPE_COLOR_VARNAME)));
        
        return container;
    }
}
