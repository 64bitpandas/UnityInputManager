using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

///Attached to every button for keybind assignment in the main menu.
public class InputButton : MonoBehaviour {

	///Button ID, corresponds to the keybind ID
	public int id;

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
		this.GetComponentInChildren<Text>().text = input.controlList.getKeybind(id).keyCode;
	}

	// Update is called once per frame
	void Update() {

	}

	void ClickAction() {
		StartCoroutine(input.WaitForKey(id, this));
	}



}