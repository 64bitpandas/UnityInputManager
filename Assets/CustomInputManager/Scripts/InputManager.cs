using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

	///Full list of custom keybinds. Initialize with default controls.
	private KeybindList controlList;

	///Full list of custom axes. Initialize with default controls.
	private AxisList axisList;

	///Event for key detection
	private Event currentEvent;

	///Config file interface
	private ConfigFileIO config;

	/*
	####################
	# Internal Methods #
	####################
	 */

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {

		//Init ConfigFileIO
		config = new ConfigFileIO();
		controlList = config.controlList;
		axisList = config.axisList;
		defaultsPath = config.defaultsPath;
		configPath = config.configPath;

		infoTextContent = "Press " + cancelKeyCode + " to cancel key selection";

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


	///While key is being set: Waits for a valid input
	public IEnumerator WaitForKey(string name, InputButton btnRef) {

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
					controlList.getKeybind(name).keyCode = btnTexts[0].text;
					Debug.Log("Set key " + name + " to " + btnTexts[0].text);
					infoText.enabled = false;
					axisList.RefreshList();

					//Save to file
					config.WriteControls(config.configPath);

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

	///<summary> Returns true if the given key is pressed down</summary>
	public bool GetKey(string name) {
		return Input.GetKey(GetKeyCode(name).ToLower());
	}

	///<summary>
	/// Returns the current value of the axis
	/// 1 = positive key down, -1 = negative key down.
	/// Joystick/Gamepad axis values will be somewhere in between.
	///</summary>
	public float GetAxis(string name) {
		return 0;
	}

	///<summary> Returns true on the given key's initial press</summary>
	public bool GetKeyDown(string name) {
		return Input.GetKeyDown(GetKeyCode(name).ToLower());
	}

	///<summary> Returns true on the given key's release</summary>
	public bool GetKeyUp(string name) {
		return Input.GetKeyUp(GetKeyCode(name).ToLower());
	}

	///<summary> Returns the KeyCode corresponding to the keybind with given name. No case conversion.</summary>
	public string GetKeyCode(string name) {
		return controlList.getKeybind(name).keyCode;
	}

	///<summary> Wipes user preferences and copies default configuration to user configuration</summary>
	public void ResetControls() {
		config.ResetControls();
	}
}