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
	void Start () {
		try {
			input = GameObject.FindObjectOfType<InputManager>();
		} catch {
			throw new NullReferenceException("InputManager not found. Did you add it to your scene?");
		}

		GetComponent<Button>().onClick.AddListener(ClickAction);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ClickAction() {
		input.WaitForKey(id, this);
	}



}
