using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

///Attached to every button for keybind assignment in the main menu.
public class InputButton : MonoBehaviour {

	///Button name, corresponds to the keybind name
	public string buttonName;

	///Does this correspond to a keyboard key or a controller button?
	public bool isControllerButton;

	private InputManager input;

	// Use this for initialization
	void Start() {
		//Initialize input manager
		try {
			input = InputManager.GetInputManager();
		} catch {
			throw new NullReferenceException("InputManager not found. Did you add it to your scene?");
		}

		//Define click action
		GetComponent<Button>().onClick.AddListener(ClickAction);

		//Get current keybind if not a template button
		if (buttonName.Length > 0) {
			this.GetComponentInChildren<Text>().text = (isControllerButton) ? input.GetControllerButton(buttonName) : input.GetKeyCode(buttonName);
		}
		//Hide the template button
		else
			gameObject.SetActive(false);
	}

	void ClickAction() {
		if (isControllerButton)
			StartCoroutine(input.WaitForController(buttonName, this));
		else
			StartCoroutine(input.WaitForKey(buttonName, this));
	}



}