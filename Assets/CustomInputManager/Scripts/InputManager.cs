using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

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

	///Event for key detection
	private Event currentEvent;

	///Config file interface
	private ConfigFileIO config;

	///ButtonState tracking for GetKeyUp and GetKeyDown
	private bool[] keyPress = new bool[75];

	///Gamepad information
	private bool playerIndexSet = false;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private GamePadState prevState;

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

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update() {
		// Find a PlayerIndex, for a single player game
		// Will find the first controller that is connected and use it
		if (!playerIndexSet || !prevState.IsConnected) {
			for (int i = 0; i < 4; ++i) {
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected) {
					print(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}

		prevState = state;
		state = GamePad.GetState(playerIndex);
	}


	///While key is being set: Waits for a valid input
	public IEnumerator WaitForKey(string name, InputButton btnRef) {

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
					print("Key Selection cancelled");
					yield break;
				} else if (currentEvent.keyCode != cancelKeyCode && currentEvent.keyCode != KeyCode.None) {
					btnTexts[0].text = currentEvent.keyCode.ToString();
					print("Key Selection successful");
					config.controlList.GetKeybind(name).keyCode = btnTexts[0].text;
					print("Set key " + name + " to " + btnTexts[0].text);
					infoText.enabled = false;
					config.axisList.RefreshList();

					//Save to file
					config.WriteControls(config.configPath);

					yield break;
				} else yield return null;
			} else yield return null;
		}
	}

	///While controller button is being set: Waits for a valid input
	public IEnumerator WaitForController(string name, InputButton btnRef) {

		Text btnText = btnRef.GetComponentInChildren<Text>();

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
					print("Controler Button Selection cancelled");
					yield break;
				}
			} 
			
			string currButton = GamepadStates.GetPressedButton(state);
			if(currButton != null) {
				print("Controller Button Selection successful");
				btnText.text = currButton;
				config.controlList.GetKeybind(name).controllerKeyCode = currButton;
				print("Set button for " + name + " to " + currButton);
				infoText.enabled = false;
				config.axisList.RefreshList();
				//Save to file
					config.WriteControls(config.configPath);
				
				yield break;
			}

			yield return null;
		}
	}

	public void GenerateButtons() {
		config = new ConfigFileIO();
		config.controlList.GenerateButtons();
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

	///<summary> Returns true if the given key or controller button is pressed down</summary>
	public bool GetKey(string name) {

		if (config.controlList.GetKeybind(name).HasControllerInput()&&
			state.IsConnected &&
			GamepadStates.ToButtonState(config.controlList.GetKeybind(name).controllerKeyCode, state)== ButtonState.Pressed)
			return true;

		return Input.GetKey(GetKeyCode(name).ToLower());
	}

	///<summary>
	/// Returns the current value of the axis
	/// 1 = positive key down, -1 = negative key down.
	/// Joystick/Gamepad axis values will be somewhere in between.
	/// Joystick is checked first, if joystick returns zero or is disconnected, then defaults to keyboard value.
	///</summary>
	public float GetAxis(string name) {
		float result = 0f;

		//gamepad connected
		if (state.IsConnected)
			result = GamepadStates.ToAxisValue(GetAxisName(name), state);

		if (result == 0f)
			result = (GetKey(GetAxisNegative(name).name)) ? -1f :
			(GetKey(GetAxisPositive(name).name)) ? 1f : 0f;

		return result;
	}

	///<summary> Returns true on the given key's initial press</summary>
	public bool GetKeyDown(string name) {
		//Controller detected
		if (config.controlList.GetKeybind(name).HasControllerInput()&& state.IsConnected)
			if (GamepadStates.ToButtonState(config.controlList.GetKeybind(name).controllerKeyCode, state)== ButtonState.Pressed) {
				if (keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)])
					return false;

				keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)] = true;
				return true;
			}
		else
			keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)] = false;

		return Input.GetKeyDown(GetKeyCode(name).ToLower());
	}

	///<summary> Returns true on the given key's release</summary>
	public bool GetKeyUp(string name) {
		//Controller detected
		if (config.controlList.GetKeybind(name).HasControllerInput()&& state.IsConnected)
			if (GamepadStates.ToButtonState(config.controlList.GetKeybind(name).controllerKeyCode, state)!= ButtonState.Pressed) {
				if (!keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)])
					return false;

				keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)] = false;
				return true;
			}
		else
			keyPress[GamepadStates.ToButtonID(config.controlList.GetKeybind(name).controllerKeyCode)] = true;

		return Input.GetKeyUp(GetKeyCode(name).ToLower());
	}

	///<summary> Returns the KeyCode corresponding to the keybind with given name. No case conversion.</summary>
	public string GetKeyCode(string name) {
		return config.controlList.GetKeybind(name).keyCode;
	}

	///<summary> Returns the Controller button corresponding to the keybind with given name. No case conversion.</summary>
	public string GetControllerButton(string name) {
		return config.controlList.GetKeybind(name).controllerKeyCode;
	}

	///<summary> Returns the controller axis corresponding to the custom axis with given name. No case conversion.</summary>
	public string GetAxisName(string name) {
		return config.axisList.GetAxis(name).controllerAxis;
	}

	///<summary> Returns the Keybind corresponding to the positive key of the custom axis.</summary>
	public Keybind GetAxisPositive(string name) {
		return config.axisList.GetAxis(name).positiveKey;
	}

	///<summary> Returns the Keybind corresponding to the negative key of the custom axis.</summary>
	public Keybind GetAxisNegative(string name) {
		return config.axisList.GetAxis(name).negativeKey;
	}

	///<summary> Wipes user preferences and copies default configuration to user configuration</summary>
	public void ResetControls() {
		config.ResetControls();
	}
}