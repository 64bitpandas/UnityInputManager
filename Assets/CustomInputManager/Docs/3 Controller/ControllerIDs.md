# Using Controller Inputs

CustomInputManager uses [XInputDotNet](https://github.com/speps/XInputDotNet) for controller management. However, CustomInputManager builds off on this and makes controller input even easier.

### Detecting Controllers
This is already done for you automatically :D Just plug in an XBox controller during runtime and a debug message will display in the console notifying you of the controller number.

### Multiple Controllers
Currently, CustomInputManager only supports singleplayer, so the controller first detected (regardless of number) will be used. This will be changed shortly in the next update!

### Getting Controller Input
This is also done for you! As long as you map a controller button or axis to a custom button or axis (as outlined in the [Getting Started Guide](../0%20Setup/GettingStarted.md)) using `GetKey()`, `GetKeyDown()`, `GetKeyUp()`, and `GetAxis()` will use controller input.

The ID's corresponding to each controller button are below. Use this in your configuration file in place of `ControllerButton` and `ControllerAxis`. Use the **String ID** and not the Numeric ID.

If you want to get the button name of a keybind, use `GetControllerButton()` and `GetAxisName()` in InputManager. More info in [API](../0%20Setup/API.md).

| **String ID** | **Type** | **Numeric ID** |
|:-------------:|:--------:|:--------------:|
|       x       |  Button  |        0       |
|       y       |  Button  |        1       |
|       b       |  Button  |        2       |
|       a       |  Button  |        3       |
|     start     |  Button  |        4       |
|      back     |  Button  |        5       |
|     guide     |  Button  |        6       |
|  leftshoulder |  Button  |        7       |
| rightshoulder |  Button  |        8       |
|   leftstick   |  Button  |        9       |
|   rightstick  |  Button  |       10       |
|     dpadup    |  Button  |       11       |
|    dpaddown   |  Button  |       12       |
|    dpadleft   |  Button  |       13       |
|   dpadright   |  Button  |       14       |
|  lefttrigger  |   Axis   |       15       |
|  righttrigger |   Axis   |       16       |
|   leftstickx  |   Axis   |       17       |
|   leftsticky  |   Axis   |       18       |
|  rightstickx  |   Axis   |       19       |
|  rightsticky  |   Axis   |       20       |