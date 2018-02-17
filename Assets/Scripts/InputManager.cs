using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

///Methods for Control changing menu, with integrated InputManager by Ben C.
///Since 2017 December
///Last Modified 2018 February 
[ExecuteInEditMode]
public class InputManager : MonoBehaviour {
	
	///Dynamic path to controls.cfg
	private string configPath, defaultsPath;

	///Full list of custom keybinds. Initialize with default controls.
	public KeybindList controlList = new KeybindList();

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
        configPath = Application.dataPath + "/controls.cfg";
		defaultsPath = Application.dataPath + "/defaultcontrols.cfg";

        //validate controls file
		try {
			LoadControls(configPath);
            Debug.Log("Successfully loaded controls file");
		} catch {
			Debug.Log("Controls file is nonexistent or corrupted. Generating new one...");
			if (File.Exists(defaultsPath)) {
				LoadControls(defaultsPath);
				Debug.Log("Loaded controls from defaultcontrols.cfg");
			}
			else {
				Debug.Log("Default Controls are nonexistent or corrupted. Generating new one...");
				controlList.addKeybind(0, "SampleKey", "w");
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
			if(key.Length > 0)
            	controlList.addKeybind(key);
	}

}

///Keybind object representing one custom keybind configuration.
public class Keybind {
	public int id;
	public string name;
	public string keyCode;

	public Keybind(int newId, string newName, string newKeyCode) {
		id = newId;
		name = newName;
		keyCode = newKeyCode;
	}
}

///List of keybinds
public class KeybindList {
	//List of keybinds for internal use
	private List<Keybind> keys;

	public KeybindList() {
		keys = new List<Keybind>();
	}

	public void addKeybind(int id, string name, string keyCode) {
		keys.Add(new Keybind(id, name, keyCode));
	}

	public void addKeybind(Keybind newKey) {
		keys.Add(newKey);
	}

	public void addKeybind(string metadata) {
		try {

            int id = Int32.Parse(metadata.Substring(0, metadata.IndexOf(":")));
            metadata = metadata.Remove(0, metadata.IndexOf(":") + 1);
            string name = metadata.Substring(0, metadata.IndexOf(":"));
            metadata = metadata.Remove(0, metadata.IndexOf(":") + 1);

            addKeybind(id, name, metadata);
		} catch(Exception e) {
			throw new ArgumentException("metadata of invalid format: " + metadata + "\n" + e);
		}
		
	}

	public Keybind getKeybind(string name) {
		foreach(Keybind key in keys)
			if(key.name.Equals(name))
				return key;
		
		throw new NullReferenceException("Keybind of name " + name + " does not exist.");
	}

    public Keybind getKeybind(int id) {
        foreach (Keybind key in keys)
            if (key.id == id)
                return key;

        throw new NullReferenceException("Keybind of id " + id + " does not exist.");
    }

	public void removeKeybind(string name) {
		keys.Remove(getKeybind(name));
	}

	public void removeKeybind(int id) {
		keys.Remove(getKeybind(id));
	}

	public void reset() {
		keys.RemoveRange(0, keys.Count);
	}

	public override string ToString() {
		string result = "";
		foreach (Keybind key in keys)
			result += key.id + ":" + key.name + ":" + key.keyCode + "\n";
		
		return result;
	}

}