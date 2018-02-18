using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    SerializedProperty configPath, defaultsPath, templateButton, INFO_TEXT, sceneHasKeybindButtons;

    void OnEnable()
    {
        configPath = serializedObject.FindProperty("configPath");
        defaultsPath = serializedObject.FindProperty("defaultsPath");
        INFO_TEXT = serializedObject.FindProperty("INFO_TEXT");
        sceneHasKeybindButtons = serializedObject.FindProperty("sceneHasKeybindButtons");
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

		InputManager input = (InputManager)target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(configPath);
        EditorGUILayout.PropertyField(defaultsPath);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(sceneHasKeybindButtons);
        
        if(sceneHasKeybindButtons.boolValue) {
            EditorGUILayout.PropertyField(INFO_TEXT);
            if(GUILayout.Button("Generate Keybind Buttons")) {
                CreateTag();
                Debug.Log("TEST");
                input.controlList.generateButtons();
            }
            if(GUILayout.Button("Nuke All Buttons"))
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("KeybindButton"))
                    DestroyImmediate(obj);
        }
        
		EditorGUILayout.LabelField("Layout is 'id:name:keybind'.");
		if(GUILayout.Button("Edit Default Controls"))
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(@input.defaultsPath, 2);
		if(GUILayout.Button("Save Changes"))
            input.LoadControls(input.defaultsPath);

        
        serializedObject.ApplyModifiedProperties();
    }

    public void CreateTag() {
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // For Unity 5 we need this too
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        // Adding a Tag
        string s = "KeybindButton";

        // First check if it is not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; return; }
        }

        // if not found, add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s;
        }

        // Setting a Layer (Let's set Layer 10)
        string layerName = "KeybindButtons";

        // --- Unity 5 ---
        SerializedProperty sp = layersProp.GetArrayElementAtIndex(10);
        if (sp != null) sp.stringValue = layerName;
        // and to save the changes
        tagManager.ApplyModifiedProperties();

    }
}