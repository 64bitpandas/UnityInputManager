using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

///Keybind object representing one custom keybind configuration.
public class Keybind {

    public string name;
    public string keyCode;
    public string controllerKeyCode = "";

    public Keybind(string newName, string newKeyCode) {
        name = newName;
        keyCode = newKeyCode;
    }

    ///Controller support
    public Keybind(string newName, string newKeyCode, string newControllerKeyCode): this(newName, newKeyCode) {
        controllerKeyCode = newControllerKeyCode;
    }

    ///<summary> Does this Keybind include a map to a controller button? </summary>
    public bool HasControllerInput() {
        return controllerKeyCode.Length > 0;
    }

    public override string ToString() {
        return name;
    }
}

///List of keybinds, contains all methods necessary to access and modify keybinds inside.
public class KeybindList {
    //List of keybinds for internal use
    private List<Keybind> keys;

    public KeybindList() {
        keys = new List<Keybind>();
    }

    public void AddKeybind(string name, string keyCode) {
        keys.Add(new Keybind(name, keyCode));
    }

    public void AddKeybind(string name, string keyCode, string controllerKeyCode) {
        keys.Add(new Keybind(name, keyCode, controllerKeyCode));
    }

    public void AddKeybind(Keybind newKey) {
        keys.Add(newKey);
    }

    public void AddKeybind(string metadata) {
        try {

            string name = metadata.Substring(0, metadata.IndexOf(":"));
            metadata = metadata.Remove(0, metadata.IndexOf(":")+ 1);

            //Controller input detected 
            if (metadata.Contains(":")) {
                string key = metadata.Substring(0, metadata.IndexOf(":"));
                metadata = metadata.Remove(0, metadata.IndexOf(":")+ 1);
                AddKeybind(name, key, metadata);
            } else {
                AddKeybind(name, metadata);
            }

        } catch (Exception e) {
            throw new ArgumentException("metadata of invalid format: " + metadata + "\n" + e);
        }

    }

    public Keybind GetKeybind(string name) {
        foreach (Keybind key in keys)
            if (key.name.Equals(name))
                return key;

        throw new NullReferenceException("Keybind of name " + name + " does not exist.");
    }

    public void RemoveKeybind(string name) {
        keys.Remove(GetKeybind(name));
    }

    public void Reset() {
        keys.RemoveRange(0, keys.Count);
    }

    public void GenerateButtons() {

        //Get (or not) template button
        GameObject templateButton;
        try {
            templateButton = GameObject.Find("TemplateButton");
        } catch {
            throw new NullReferenceException("TemplateButton could not be found in the scene! Remember to add it in.");
        }

        int count = 1;
        foreach (Keybind key in keys) {
            GameObject newButton = GameObject.Instantiate(templateButton, templateButton.transform.parent);
            //Set label name
            newButton.GetComponentsInChildren<Text>()[1].text = key.name;
            newButton.name = key.name;

            //Set tag
            newButton.tag = "KeybindButton";

            //Set corresponding button
            newButton.GetComponent<InputButton>().buttonName = key.name;

            //Translate for easier viewing
            newButton.transform.Translate(Vector3.down * 50 * count);
            count++;

            //Set target graphic
            newButton.GetComponent<Button>().targetGraphic = newButton.GetComponent<Image>();
        }
    }

    public override string ToString() {
        string result = "";
        foreach (Keybind key in keys)
            result += key.name + ":" + key.keyCode + ":" + key.controllerKeyCode + "\n";

        return result;
    }

}