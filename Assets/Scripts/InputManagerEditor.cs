using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    SerializedProperty configPath, defaultsPath, templateButton, sceneHasKeybindButtons, INFO_TEXT;

    void OnEnable()
    {
        configPath = serializedObject.FindProperty("configPath");
        defaultsPath = serializedObject.FindProperty("defaultsPath");
        sceneHasKeybindButtons = serializedObject.FindProperty("hasButtons");
        INFO_TEXT = serializedObject.FindProperty("INFO_TEXT");
    }

    public override void OnInspectorGUI()
    {
		InputManager input = (InputManager)target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(configPath);
        EditorGUILayout.PropertyField(defaultsPath);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(sceneHasKeybindButtons);
        EditorGUI.BeginDisabledGroup(!sceneHasKeybindButtons.boolValue);
        EditorGUILayout.PropertyField(INFO_TEXT);
        EditorGUI.EndDisabledGroup();
        
		EditorGUILayout.LabelField("Layout is 'id:name:keybind'.");
		if(GUILayout.Button("Edit Default Controls"))
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(@input.defaultsPath, 2);
		if(GUILayout.Button("Save Changes"))
            input.LoadControls(input.defaultsPath);

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}