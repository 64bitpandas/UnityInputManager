using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///List of keybinds
public class KeybindList {
    //List of keybinds for internal use
    private List<Keybind> keys;

    public KeybindList() {
        keys = new List<Keybind>();
    }

    public void addKeybind(int id, string name, string keyCode) {
        keys.Add(new Keybind(id, name, keyCode));
    }

    public void addKeybind(Keybind newKey) {
        keys.Add(newKey);
    }

    public void addKeybind(string metadata) {
        try {

            int id = Int32.Parse(metadata.Substring(0, metadata.IndexOf(":")));
            metadata = metadata.Remove(0, metadata.IndexOf(":")+ 1);
            string name = metadata.Substring(0, metadata.IndexOf(":"));
            metadata = metadata.Remove(0, metadata.IndexOf(":")+ 1);

            addKeybind(id, name, metadata);
        } catch (Exception e) {
            throw new ArgumentException("metadata of invalid format: " + metadata + "\n" + e);
        }

    }

    public Keybind getKeybind(string name) {
        foreach (Keybind key in keys)
            if (key.name.Equals(name))
                return key;

        throw new NullReferenceException("Keybind of name " + name + " does not exist.");
    }

    public Keybind getKeybind(int id) {
        foreach (Keybind key in keys)
            if (key.id == id)
                return key;

        throw new NullReferenceException("Keybind of id " + id + " does not exist.");
    }

    public void removeKeybind(string name) {
        keys.Remove(getKeybind(name));
    }

    public void removeKeybind(int id) {
        keys.Remove(getKeybind(id));
    }

    public void reset() {
        keys.RemoveRange(0, keys.Count);
    }

    public void generateButtons() {

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

            //Translate for easier viewing
            newButton.transform.Translate(Vector3.down * 50 * count);
            count++;

            //Set ID
            newButton.GetComponent<InputButton>().id = key.id;

            //Set target graphic
            newButton.GetComponent<Button>().targetGraphic = newButton.GetComponent<Image>();
        }
    }

    public override string ToString() {
        string result = "";
        foreach (Keybind key in keys)
            result += key.id + ":" + key.name + ":" + key.keyCode + "\n";

        return result;
    }

}

///Keybind object representing one custom keybind configuration.
public class Keybind {
    public int id;
    public string name;
    public string keyCode;

    public Keybind(int newId, string newName, string newKeyCode) {
        id = newId;
        name = newName;
        keyCode = newKeyCode;
    }
}