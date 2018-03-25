# API Reference

Please [open an issue](https://github.com/dbqeo/UnityInputManager/issues/new) if any accessible function or property in `InputManager` is not documented here.

- `static GetInputManager() : InputManager` Returns the InputManager object in the current scene. Throws a `NullReferenceException` if no InputManager is found.
 - `GetKey(string name) : bool` Returns true if the custom keybind with given name is currently pressed down.
 - `GetKeyDown(string name) : bool` Returns true on initial key press.
 - `GetKeyUp(string name) : bool` Returns true when the key is released.
 - `GetKeyCode(string name) : string`  Returns the KeyCode corresponding to the keybind with given name.
 - `ResetControls() : void` Copies default configuration to user configuration.



### `static GetInputManager() : InputManager`

Returns the InputManager object in the current scene. Throws a `NullReferenceException` if no InputManager is found.

Example:
```csharp
...
//Get the InputManager and use it to reset controls.
InputManager input = InputManager.GetInputManager();

if(input != null)
    input.ResetControls();
```

### `GetAxis(string name) : float`

Returns the current value of the custom axis specified.

1f = positive key down, -1f = negative key down.

Joystick/Gamepad axis values will be somewhere in between.

Joystick is checked first, if joystick returns zero or is disconnected, then defaults to keyboard value.

Example:
```csharp
...
//Player turn speed is based on the axis value of 'Turn'.
InputManager input = InputManager.GetInputManager();

void Update() {
    transform.Rotate(Vector3.right * input.GetAxis("Turn"));
}
```

### `GetKey(string name) : bool`

Returns true if the custom keybind with given name is currently pressed down.

Prioritizes controller input first; if the controller button is not pressed down or no controller is detected, then defaults to keyboard controls.

Example:
```csharp
...
//Move player when key 'Move' is pressed down
InputManager input = InputManager.GetInputManager();

void Update() {
    if(input.GetKey("Move"))
        transform.translate(Vector3.left);
}
```

### `GetKeyDown(string name) : bool`

Returns true on initial keypress of the given keybind.

Prioritizes controller input first; if the controller button is not pressed down or no controller is detected, then defaults to keyboard controls.

Example:
```csharp
...
//Move player 10 units left every time 'Move' is pressed.
InputManager input = InputManager.GetInputManager();

void Update() {
    if(input.GetKeyDown("Move"))
        transform.translate(Vector3.left * 10f);
}
```

### `GetKeyUp(string name) : bool`

Returns true on release of the given keybind.

Prioritizes controller input first; if the controller button is not pressed down or no controller is detected, then defaults to keyboard controls.

Example:
```csharp
...
//Move player 10 units left every time 'Move' is released.
InputManager input = InputManager.GetInputManager();

void Update() {
    if(input.GetKeyUp("Move"))
        transform.translate(Vector3.left * 10f);
}
```

### `GetKeyCode(string name) : string`  

Returns the KeyCode corresponding to the keybind with given name.

Example:
```csharp
...
//'Move' corresponds to the player pressing the 'W' key.
InputManager input = InputManager.GetInputManager();

void Start() {
    //Prints 'W' to the console
    print(input.GetKeyCode("Move"));
}
```

### `GetControllerButton(string name) : string`  

Returns the controller button corresponding to the keybind with given name.

Example:
```csharp
...
//'Move' corresponds to the player pressing the 'X' key on their controller.
InputManager input = InputManager.GetInputManager();

void Start() {
    //Prints 'X' to the console
    print(input.GetControllerButton("Move"));
}
```

### `GetAxisName(string name) : string`  

Returns the controller axis corresponding to the custom axis with given name.

Example:
```csharp
...
//'Turn' corresponds to the x-axis on the left stick of the controller.
InputManager input = InputManager.GetInputManager();

void Start() {
    //Prints 'leftstickx' to the console
    print(input.GetAxisName("Turn"));
}
```

### `GetAxisPositive(string name) : string`  

Returns the KeyCode corresponding to the positive key of the custom axis.

Example:
```csharp
...
//'Turn' rotates left (negative) if 'A' is pressed and rotates right (positive) if 'D' is pressed
InputManager input = InputManager.GetInputManager();

void Start() {
    //Prints 'D' to the console
    print(input.GetAxisPositive("Turn"));
}
```

### `GetAxisNegative(string name) : string`  

Returns the KeyCode corresponding to the negative key of the custom axis.

Example:
```csharp
...
//'Turn' rotates left (negative) if 'A' is pressed and rotates right (positive) if 'D' is pressed
InputManager input = InputManager.GetInputManager();

void Start() {
    //Prints 'A' to the console
    print(input.GetAxisNegative("Turn"));
}
```

### - `ResetControls() : void` 

Copies default configuration to user configuration, effectively wiping all custom configurations made by the user.

Example:
```csharp
...
//Resets player configuration if the boolean wantsToReset is true.
InputManager input = InputManager.GetInputManager();
public bool wantsToReset;

void Update() {
    if(wantsToReset) {
        input.ResetControls();
        wantsToReset = false;
    }
}
```

## Internal Functions

The below functions are created for internal use only, but may be of some use to you:

 - `void GenerateButtons()` - Creates buttons for users to assign keybinds in a menu. 
 - `IEnumerator WaitForKey(string name, InputButton btnRef)` - When the Button `btnRef` is pressed, waits for the user to press a key and assigns that key to the custom keybind.
 