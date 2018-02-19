using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    SerializedProperty configPath, defaultsPath, templateButton, sceneHasKeybindButtons, cancelKeyCode;

    void OnEnable()
    {
        configPath = serializedObject.FindProperty("configPath");
        defaultsPath = serializedObject.FindProperty("defaultsPath");
        sceneHasKeybindButtons = serializedObject.FindProperty("sceneHasKeybindButtons");
        cancelKeyCode = serializedObject.FindProperty("cancelKeyCode");
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        InputManager input = (InputManager)target;
        input.LoadControls(input.defaultsPath);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(configPath);
        EditorGUILayout.PropertyField(defaultsPath);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.PropertyField(sceneHasKeybindButtons);

        if (sceneHasKeybindButtons.boolValue) {
            EditorGUILayout.PropertyField(cancelKeyCode);
            if (GUILayout.Button("Generate Keybind Buttons"))
            {
                CreateTag();
                input.GenerateButtons();
            }
            if (GUILayout.Button("Nuke All Buttons"))
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("KeybindButton"))
                    DestroyImmediate(obj);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Layout is 'id:name:keybind'.");
        if(GUILayout.Button("KeyCode Reference"))
            Application.OpenURL("https://docs.unity3d.com/2018.1/Documentation/ScriptReference/KeyCode.html");
        if (GUILayout.Button("Edit Default Controls"))
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(@input.defaultsPath, 2);
        if (GUILayout.Button("Save Changes"))
        {
            input.LoadControls(input.defaultsPath);
            input.WriteControls(input.configPath);
        }


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
        for (int i = 0; i < tagsProp.arraySize; i++) {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; return; }
        }

        // if not found, add it
        if (!found) {
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