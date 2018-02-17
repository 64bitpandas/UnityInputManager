using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    SerializedProperty keybindList;

    void OnEnable()
    {
        keybindList = serializedObject.FindProperty("test");
		Debug.Log(keybindList);
    }

    public override void OnInspectorGUI()
    {
		InputManager input = (InputManager)target;

		DrawDefaultInspector();
        serializedObject.Update();
        // EditorGUILayout.PropertyField(keybindList);
		EditorGUILayout.LabelField("Foo");
		if(GUILayout.Button("Add Keybind"));
		if(GUILayout.Button("Save Changes"));
			
        serializedObject.ApplyModifiedProperties();
    }
}