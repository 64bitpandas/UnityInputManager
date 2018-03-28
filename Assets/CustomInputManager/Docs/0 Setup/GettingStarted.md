# CustomInputManager Quick Start Guide

Welcome to CustomInputManager! This simple guide will get you started on a simple project, with several custom keybinds and a menu where your user can create their own configurations!

### Step One: Download

You can download the latest release [here](https://github.com/dbqeo/UnityInputManager/releases). Open the Unity package in your project, and follow the instructions in the popup dialog box to import the code in.

**Important Note:** The Unity Package includes XInputDotNet by default. If you are already using XInputDotNet in your project, uncheck the directory in the import screen.

### Step Two: Configuration

Upon import, a folder named 'CustomInputManager' will be created. In order to add the Input Manager to your scene, add 'Scenes/InputManager.cs' into any object that will not be cloned or destroyed.

Select the object to view the custom Inspector controls for the InputManager script. Here, you can open the default configuration file for editing (it will open the file in your default code editor). 

![InputManager Inspector](inputmanager.PNG "Input Manager")

In the default configuration file, each line corresponds to one custom keybind. The format for each line is `Name:KeyCode:ControllerButton`. 
 - Name: General identifier for the keybind. This is the name that will display on auto-generated keybind buttons, and the name that will be used through the API to get key inputs.
 - KeyCode: Default key input that corresponds to this keybind. You may reference the [Unity API page](https://docs.unity3d.com/ScriptReference/KeyCode.html) for a full list of valid keybinds. This is only for the default configuration, and can be reassigned by the user.
 - ControllerButton: Name of the controller button you want to map to this keybind. Valid Button names can be found at [Controller IDs](../3%20Controller/ControllerIDs.md). **This is optional!** If you do not support controller input, stop after KeyCode. 

You can also generate axes to bind two keybinds and a controller axis under the same name. The format for an axis is `AXIS:Name:PositiveKey:NegativeKey:ControllerAxis`.
 - AXIS: That's it. Literally type the word 'AXIS' in front to let the parser know that it is an axis and not a keybind.
 - AxisName: General identifier for the axis. This is the name that will be used through the API to get key inputs.
 - PositiveKey: Name of custom keybind to map to positive key of the axis. When this key is pressed down, `GetAxis()` will return 1. **NOTE: You do not use the KeyCode (like 'A' or 'Space')! You use the name of the custom keybind (like 'Jump' or 'Run')!**
 - NegativeKey: Name of custom keybind to map to negative key of the axis. When this key is pressed down, `GetAxis()` will return -1.
 - ControllerAxis: Name of the controller button you want to map to this axis. Valid Axis names can be found at [Controller IDs](../3%20Controller/ControllerIDs.md). **This is optional!** If you do not support controller input, stop after KeyCode. 


 After editing the configuration file, remember to click the "Save Configuration" button in the Inspector to update the database before generating keybind buttons. 

 ### Step Three: Generating Keybind Buttons
 
 In your Main Menu or Options scene where you want to insert keybind buttons for users to assign custom configurations, add the InputManager script and the TemplateButton prefab. Additionally, create a Text object titled "InfoText." This will be displayed during keybind assignment to let the user know that they are currently in keybind selection mode, and how to cancel it. There is no need to edit the text content; it will be automatically assigned in script.
 
 In the Inspector Panel, check the toggle box for "Scene Has Keybind Buttons." This will reveal further options:
  - Cancel Keybind: What key the user should press if they are in keybind selection mode and do not wish to edit the keybind. This will be displayed in the InfoText.
  - Generate Keybind Buttons: This will generate buttons underneath where you placed the TemplateButton. Each will already have the correct title and ID assigned, so the only thing to do is rearrange them wherever you wish. If you want to change the button style, font, etc. you may change them in the TemplateButton and the changes will be reflected in all generated buttons.
  - Nuke All Buttons: Removes all generated buttons. Click this when you wish to regenerate buttons after you edit the default configuration, or the TemplateButton.

### Step Four: Use API In Your Scripts

The InputManager API is very similar to default unity Input interface, with the difference that it is a nonstatic class. Therefore, you can initialize InputManager using the static convenience method in InputManager, `InputManager.GetInputManager()`.

```csharp
private InputManager input;

...

void Start () {
    input = InputManager.GetInputManager();
}
```

You can view the full API [here](./API.md).

Multiplayer support will be added in the next major update.