using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

///Methods for Control changing menu, with integrated InputManager by Ben C.
///Since 2017 December
///Last Modified 2018 February 
[ExecuteInEditMode]
public class InputManager : MonoBehaviour {

	///Dynamic path to controls.cfg
	public string configPath, defaultsPath;

	[SerializeField]
	///KeyCode to cancel key selection
	private KeyCode cancelKeyCode = KeyCode.Backspace;

	[SerializeField]
	///Does this scene have keybind buttons?
	private bool sceneHasKeybindButtons;

	[SerializeField]
	///Text to display in InfoText
	private string infoTextContent;

	[SerializeField]
	///Full list of custom keybinds. Initialize with default controls.
	private KeybindList controlList = new KeybindList();

	///Event for key detection
	private Event currentEvent;

	/*
	####################
	# Internal Methods #
	####################
	 */

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		infoTextContent = "Press " + cancelKeyCode + " to cancel key selection";
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
				controlList.addKeybind(0, "SampleKey", "Space");
				WriteControls(defaultsPath);
			}

			WriteControls(configPath);
		}
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		//Create buttons
		if (sceneHasKeybindButtons) {
			try {
				GameObject.Find("InfoText").GetComponent<Text>().enabled = false;
			} catch {
				throw new NullReferenceException("InfoText not found! Create a Text object named 'InfoText'.");
			}
		}
	}

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI() {
		currentEvent = Event.current;
	}

	/// <summary>
	/// Returns the Input Manager in the scene 
	/// Assign to an InputManager object in your script.
	/// </summary>
	public static InputManager GetInputManager() {
		try {
			return (InputManager)FindObjectOfType(typeof(InputManager));
		} catch {
			throw new NullReferenceException("InputManager could not be found!");
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

	///While key is being set: Waits for a valid input
	public IEnumerator WaitForKey(int id, InputButton btnRef) {

		Debug.Log("Test");
		//0 is keybind label, 1 is name label
		Text[] btnTexts = btnRef.GetComponentsInChildren<Text>();

		//Text that indicates when selection is occurring
		Text infoText;

		try {
			infoText = GameObject.Find("InfoText").GetComponent<Text>();
			infoText.text = infoTextContent;
			infoText.enabled = true;
		} catch {
			throw new NullReferenceException("InfoText not found! Create a Text object named 'InfoText'.");
		}

		while (true) {
			if (currentEvent != null && (currentEvent.isKey || currentEvent.isMouse)) {
				if (currentEvent.keyCode == cancelKeyCode) {
					infoText.enabled = false;
					Debug.Log("Key Selection cancelled");
					yield break;
				} else if (currentEvent.keyCode != cancelKeyCode && currentEvent.keyCode != KeyCode.None) {
					btnTexts[0].text = currentEvent.keyCode.ToString();
					Debug.Log("Key Selection successful");
					controlList.getKeybind(id).keyCode = btnTexts[0].text;
					Debug.Log("Set key " + id + " to " + btnTexts[0].text);
					infoText.enabled = false;

					//Save to file
					WriteControls(configPath);

					yield break;
				} else yield return null;
			} else yield return null;
		}
	}

	public void GenerateButtons() {
		controlList.generateButtons();
	}

	/*
	#######
	# API #
	#######	
	 */

	///<summary> Returns true if the given key is pressed down</summary>
	public bool GetKey(string name) {
		return Input.GetKey(GetKeyCode(name));
	}

	///<summary> Returns true on the given key's initial press</summary>
	public bool GetKeyDown(string name) {
		return Input.GetKeyDown(GetKeyCode(name));
	}

	///<summary> Returns true on the given key's release</summary>
	public bool GetKeyUp(string name) {
		return Input.GetKeyUp(GetKeyCode(name));
	}

    ///<summary> Returns the KeyCode corresponding to the keybind with given name</summary>
    public string GetKeyCode(string name) {
		return controlList.getKeybind(name).keyCode;
	}

    ///<summary> Returns the KeyCode corresponding to the keybind with given ID</summary>
    public string GetKeyCode(int id) {
		return controlList.getKeybind(id).keyCode;
	}

}