using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    SerializedProperty configPath, defaultsPath;

    void OnEnable()
    {
        configPath = serializedObject.FindProperty("configPath");
        defaultsPath = serializedObject.FindProperty("defaultsPath");
    }

    public override void OnInspectorGUI()
    {
		InputManager input = (InputManager)target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(configPath);
        EditorGUILayout.PropertyField(defaultsPath);
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