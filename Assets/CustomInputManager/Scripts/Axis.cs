using System;
using System.Collections.Generic;
using UnityEngine;

///Axis object containing one positive and one negative button. These buttons can be remapped to different keys.
///Axis can also be used with Joystick axes.
public class Axis {

    ///Name of axis
    public string name;

    ///Positive button (when pressed, returns value of 1)
    public Keybind positiveKey;

    ///Negative button (when pressed, returns value of -1)
    public Keybind negativeKey;

    public Axis(string name, Keybind positiveKey, Keybind negativeKey) {
        this.name = name;
        this.positiveKey = positiveKey;
        this.negativeKey = negativeKey;
    }
}

///Axis list
public class AxisList {

    private List<Axis> axList;
    private KeybindList controlList;

    public AxisList(KeybindList list) {
        controlList = list;
    }

    ///Refreshes axis list based on key list.
    public void RefreshList() {
        foreach (Axis ax in axList) {
            ax.positiveKey = controlList.GetKeybind(ax.name);
        }

        Debug.Log("Refreshed Axis List");
    }

    public void AddAxis(Axis newAxis) {
        axList.Add(newAxis);
    }

    public void AddAxis(string name, string positiveKey, string negativeKey) {
        axList.Add(new Axis(name, controlList.GetKeybind(positiveKey), controlList.GetKeybind(negativeKey)));
    }

    public void AddAxis(string name, Keybind positiveKey, Keybind negativeKey) {
        axList.Add(new Axis(name, positiveKey, negativeKey));
    }

    ///Format AXIS:name:positiveKeyName:negativeKeyName
    public void AddAxis(string metadata) {
        try {
            metadata = metadata.Substring(5);
            string name = metadata.Substring(0, metadata.IndexOf(":"));
            metadata = metadata.Remove(0, metadata.IndexOf(":")+ 1);
            string positiveKey = metadata.Substring(0, metadata.IndexOf(":"));

            AddAxis(name, positiveKey, metadata);
        } catch (Exception e) {
            throw new ArgumentException("metadata of invalid format: " + metadata + "\n" + e);
        }
    }

    public Axis GetAxis(string name) {
        foreach (Axis ax in axList)
            if (ax.name.Equals(name))
                return ax;

        throw new NullReferenceException("Axis of name " + name + " does not exist.");
    }

    public void RemoveAxis(string name) {
        axList.Remove(GetAxis(name));
    }

    public override string ToString() {
        string result = "";
        foreach(Axis ax in axList) {
            result += "AXIS:" + ax.name + ":" + ax.positiveKey + ":" + ax.negativeKey + "\n";
        }

        return result;
    }
}