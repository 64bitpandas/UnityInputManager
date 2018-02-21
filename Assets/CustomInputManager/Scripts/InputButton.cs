using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

///Attached to every button for keybind assignment in the main menu.
public class InputButton : MonoBehaviour {

	///Button name, corresponds to the keybind name
	public string buttonName;

	private InputManager input;

	// Use this for initialization
	void Start() {
		//Initialize input manager
		try {
			input = GameObject.FindObjectOfType<InputManager>();
		} catch {
			throw new NullReferenceException("InputManager not found. Did you add it to your scene?");
		}

		//Define click action
		GetComponent<Button>().onClick.AddListener(ClickAction);

		//Get current keybind
		this.GetComponentInChildren<Text>().text = input.GetKeyCode(buttonName);
	}

	// Update is called once per frame
	void Update() {

	}

	void ClickAction() {
		StartCoroutine(input.WaitForKey(buttonName, this));
	}



}