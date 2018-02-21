using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// Interface between configuration files and InputManager.
public class ConfigFileIO {

    ///Dynamic path to controls.cfg
    public string configPath, defaultsPath;
    public KeybindList controlList = new KeybindList();

    public ConfigFileIO() {
        configPath = Application.dataPath + "/CustomInputManager/Config/controls.cfg";
        defaultsPath = Application.dataPath + "/CustomInputManager/Config/defaultcontrols.cfg";

        if (!Directory.Exists(Application.dataPath + "/CustomInputManager/Config")) {
            Debug.Log("Creating Config folder...");
            AssetDatabase.CreateFolder("Assets/CustomInputManager", "Config");
        }

        //validate controls file
        try {
            LoadControls(configPath);
            Debug.Log("Successfully loaded controls file");
        } catch {
            Debug.Log("Controls file is nonexistent or corrupted. Generating new one...");
            if (File.Exists(defaultsPath)) {
                LoadControls(defaultsPath);
                Debug.Log("Loaded controls from defaultcontrols.cfg");
            } else {
                Debug.Log("Default Controls are nonexistent or corrupted. Generating new one...");
                controlList.addKeybind("SampleKey", "Space");
                WriteControls(defaultsPath);
            }

            WriteControls(configPath);
        }
    }

    ///Writes controls from ControlList to the config file.
    public void WriteControls(string filePath) {
        using(var writer = new StreamWriter(File.Create(filePath))) {
            writer.WriteLine(controlList);
        }
    }

    ///Loads controls from the config file to ControlList.
    public void LoadControls(string filePath) {
        string[] keys = File.ReadAllLines(filePath);
        controlList.reset();
        foreach (string key in keys)
            if (key.Length > 0)
                controlList.addKeybind(key);

        // Debug.Log(controlList);
    }

    public void ResetControls() {
        LoadControls(defaultsPath);
        WriteControls(configPath);
        Debug.Log("Reset Controls");
    }
}