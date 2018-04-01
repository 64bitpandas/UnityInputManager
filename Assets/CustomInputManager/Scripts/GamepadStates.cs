using System;
using XInputDotNetPure;

///Converts string values to their corresponding ButtonState or StickValue
public class GamepadStates {

    public static ButtonState ToButtonState(string stateName, GamePadState state) {
        switch (stateName.ToLower()) {
            case "x":
                return state.Buttons.X;
            case "y":
                return state.Buttons.Y;
            case "b":
                return state.Buttons.B;
            case "a":
                return state.Buttons.A;
            case "start":
                return state.Buttons.Start;
            case "back":
                return state.Buttons.Back;
            case "guide":
                return state.Buttons.Guide;
            case "leftshoulder":
                return state.Buttons.LeftShoulder;
            case "rightshoulder":
                return state.Buttons.RightShoulder;
            case "leftstick":
                return state.Buttons.LeftStick;
            case "rightstick":
                return state.Buttons.RightStick;
            case "dpadup":
                return state.DPad.Up;
            case "dpaddown":
                return state.DPad.Down;
            case "dpadleft":
                return state.DPad.Left;
            case "dpadright":
                return state.DPad.Right;
        }

        throw new NullReferenceException("Button " + stateName + " could not be found");
    }

    public static int ToButtonID(string stateName) {
        switch (stateName.ToLower()) {
            case "x":
                return 0;
            case "y":
                return 1;
            case "b":
                return 2;
            case "a":
                return 3;
            case "start":
                return 4;
            case "back":
                return 5;
            case "guide":
                return 6;
            case "leftshoulder":
                return 7;
            case "rightshoulder":
                return 8;
            case "leftstick":
                return 9;
            case "rightstick":
                return 10;
            case "dpadup":
                return 11;
            case "dpaddown":
                return 12;
            case "dpadleft":
                return 13;
            case "dpadright":
                return 14;
        }

        throw new NullReferenceException("Button " + stateName + " could not be found");
    }

    public static float ToAxisValue(string axisName, GamePadState state) {

        switch (axisName.ToLower()) {

            case "lefttrigger":
                return state.Triggers.Left;
            case "righttrigger":
                return state.Triggers.Right;
            case "leftstickx":
                return state.ThumbSticks.Left.X;
            case "leftsticky":
                return state.ThumbSticks.Left.Y;
            case "rightstickx":
                return state.ThumbSticks.Right.X;
            case "rightsticky":
                return state.ThumbSticks.Right.Y;
        }

        throw new NullReferenceException("Axis " + axisName + " could not be found");
    }

    public static int ToAxisID(string axisName) {

        switch (axisName.ToLower()) {

            case "lefttrigger":
                return 15;
            case "righttrigger":
                return 16;
            case "leftstickx":
                return 17;
            case "leftsticky":
                return 18;
            case "rightstickx":
                return 19;
            case "rightsticky":
                return 20;
        }

        throw new NullReferenceException("Axis " + axisName + " could not be found");
    }

    public static string GetPressedButton(GamePadState state) {

        if(!state.IsConnected)
            return null;
        if (state.Buttons.X == ButtonState.Pressed)
            return "X";
        if (state.Buttons.Y == ButtonState.Pressed)
            return "Y";
        if (state.Buttons.B == ButtonState.Pressed)
            return "B";
        if (state.Buttons.A == ButtonState.Pressed)
            return "A";
        if (state.Buttons.Start == ButtonState.Pressed)
            return "Start";
        if (state.Buttons.Back == ButtonState.Pressed)
            return "Back";
        if (state.Buttons.Guide == ButtonState.Pressed)
            return "Guide";
        if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            return "LeftShoulder";
        if (state.Buttons.RightShoulder == ButtonState.Pressed)
            return "RightShoulder";
        if (state.Buttons.LeftStick == ButtonState.Pressed)
            return "LeftStick";
        if (state.Buttons.RightStick == ButtonState.Pressed)
            return "RightStick";
        if (state.DPad.Up == ButtonState.Pressed)
            return "DPadUp";
        if (state.DPad.Down == ButtonState.Pressed)
            return "DPadDown";
        if (state.DPad.Left == ButtonState.Pressed)
            return "DPadLeft";
        if (state.DPad.Right == ButtonState.Pressed)
            return "DPadRight";

        return null;
    }
}