using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputHandler
{

    public static KeyCode ConvertDesiredKeyToInputKey(KeyCode key)
    {
        return ClientSettings.Instance.inputSettings.buttonToInputMap[key];
    }

    public static bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(ConvertDesiredKeyToInputKey(key));
    }

    public static bool GetKey(KeyCode key)
    {
        return Input.GetKey(ConvertDesiredKeyToInputKey(key));
    }

    public static bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(ConvertDesiredKeyToInputKey(key));
    }

    public static KeyCode GetCurrentPressedKey()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key)) return key;
        }
        return KeyCode.None;
    }
}
