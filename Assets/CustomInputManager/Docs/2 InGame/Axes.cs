using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Axes : MonoBehaviour {

	private InputManager input; //input manager
	private SpriteRenderer squareSprite; //sprite that is shown on camera
	private Text instructionText; //Displays what key to press for action

	// Use this for initialization
	void Start() {
		input = InputManager.GetInputManager();
		instructionText = (Text)FindObjectOfType(typeof(Text));
		squareSprite = (SpriteRenderer)FindObjectOfType(typeof(SpriteRenderer));

		//Assign Text
		instructionText.text = "Use " + input.GetAxisName("SampleAxis") + " to move the square.\n" + "Or press " + input.GetAxisPositive("SampleAxis").keyCode + " and " + input.GetAxisNegative("SampleAxis").keyCode;
	}

	// Update is called once per frame
	void Update() {

		//Move left and right based on axis value
		squareSprite.transform.Translate(Vector3.right * input.GetAxis("SampleAxis") * 0.25f);

	}
}