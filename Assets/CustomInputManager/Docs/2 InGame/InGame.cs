using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour {

	private InputManager input; //input manager
	private SpriteRenderer squareSprite; //sprite that is shown on camera
	private Text instructionText; //Displays what key to press for action

	// Use this for initialization
	void Start () {
		input = InputManager.GetInputManager();
		instructionText = (Text)FindObjectOfType(typeof(Text));
		squareSprite = (SpriteRenderer)FindObjectOfType(typeof(SpriteRenderer));

		//Assign Text
		instructionText.text = "Press " + input.GetKeyCode("SampleKey") + " to change color\n" + "Or press " + input.GetControllerButton("SampleKey") + " on controller";
	}
	
	// Update is called once per frame
	void Update () {
		
		//Change sprite color to random color when key pressed
		if(input.GetKeyDown("SampleKey")) {
			float r = Random.Range(0f, 1f);
			float g = Random.Range(0f, 1f);
			float b = Random.Range(0f, 1f);
			squareSprite.color = new Color(r,g,b,1);
		}


	}
}
