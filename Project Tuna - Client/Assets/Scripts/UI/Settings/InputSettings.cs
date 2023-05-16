using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tuna;

[Serializable]
public class InputSettings
{
    public SerializableDictionary<KeyCode, KeyCode> buttonToInputMap = new SerializableDictionary<KeyCode, KeyCode>();

    public SerializableDictionary<KeyCode, string> buttonToDescriptionMap = new SerializableDictionary<KeyCode, string>();

    public InputSettings()
    {
        foreach(KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            buttonToInputMap[key] = key;
        }
    }

    
    public void RemapKey(KeyCode input, KeyCode desiredKey)
    {
        buttonToInputMap[input] = desiredKey;

        Debug.Log(buttonToInputMap[input]);
    }

    public void RemapKeyAvoidDoubleMapping(KeyCode input, KeyCode desiredKey)
    {
        RemapKey(input, desiredKey);
        List<KeyCode> keysToReset = new List<KeyCode>();
        foreach(KeyValuePair<KeyCode, KeyCode> entry in buttonToInputMap)
        {
            if (entry.Value == desiredKey)
                keysToReset.Add(entry.Key);
        }
        
        foreach(KeyCode key in keysToReset)
        {
            buttonToInputMap[key] = KeyCode.None;
        }
    }

    public void AddDescription(KeyCode key, string description)
    {
        buttonToDescriptionMap[key] = description;
    }

}