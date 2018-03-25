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
            case "leftshoulder":
                return state.Buttons.LeftShoulder;
            case "rightshoulder":
                return state.Buttons.RightShoulder;
            case "leftstick":
                return state.Buttons.LeftStick;
            case "rightstick":
                return state.Buttons.RightStick;
            case "guide":
                return state.Buttons.Guide;
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
                return 10;
            case "y":
                return 11;
            case "b":
                return 12;
            case "a":
                return 13;
            case "start":
                return 20;
            case "back":
                return 21;
            case "leftshoulder":
                return 30;
            case "rightshoulder":
                return 31;
            case "leftstick":
                return 40;
            case "rightstick":
                return 41;
            case "guide":
                return 22;
            case "dpadup":
                return 50;
            case "dpaddown":
                return 51;
            case "dpadleft":
                return 52;
            case "dpadright":
                return 53;
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
                return 60;
            case "righttrigger":
                return 61;
            case "leftstickx":
                return 70;
            case "leftsticky":
                return 71;
            case "rightstickx":
                return 72;
            case "rightsticky":
                return 73;
        }

        throw new NullReferenceException("Axis " + axisName + " could not be found");
    }
}