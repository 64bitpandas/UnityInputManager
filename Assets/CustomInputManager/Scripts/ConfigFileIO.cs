using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// Interface between configuration files and InputManager.
public class ConfigFileIO {

    ///Dynamic path to controls.cfg
    public string configPath, defaultsPath;
    public KeybindList controlList = new KeybindList();
    public AxisList axisList;

    public ConfigFileIO() {

        axisList = new AxisList(controlList);
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
                ///Default sample config
                controlList.AddKeybind("SampleKey", "Space");
                controlList.AddKeybind("SampleKeyTwo", "W");
                axisList.AddAxis("SampleAxis", "SampleKey", "SampleKeyTwo");
                WriteControls(defaultsPath);
            }

            ResetControls();
        }
    }

    ///Writes controls from ControlList to the config file.
    public void WriteControls(string filePath) {
        using(var writer = new StreamWriter(File.Create(filePath))) {
            if(filePath.Equals(defaultsPath)) {
                writer.WriteLine("# Please do not delete the sample controls if you wish to run the samples.");
                writer.WriteLine("# Comments such as these will not be transferred to user configuration.\n");
            }
            writer.WriteLine(controlList);
            writer.WriteLine(axisList);
            writer.Close();
        }
    }

    ///Loads controls from the config file to ControlList.
    public void LoadControls(string filePath) {
        string[] keys = File.ReadAllLines(filePath);
        controlList.Reset();
        foreach (string key in keys)
            if (key.Length > 0 && !key.Contains("#")) {
                if (key.Contains("AXIS"))
                    axisList.AddAxis(key);
                else
                    controlList.AddKeybind(key);
            }

        // Debug.Log(controlList);
    }

    public void ResetControls() {
        LoadControls(defaultsPath);
        WriteControls(configPath);
        Debug.Log("Reset Controls");
    }
}