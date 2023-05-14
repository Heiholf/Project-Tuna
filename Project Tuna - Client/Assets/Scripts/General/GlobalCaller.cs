using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCaller : MonoBehaviour
{
    private static GlobalCaller instance;

    public static GlobalCaller Instance
    {
        get
        {
            if (instance is null)
            {
                Debug.LogError($"{nameof(instance)}-Instance is already not yet set.");
            }
            return instance;
        }
        private set
        {
            if (instance is not null)
            {
                Debug.LogWarning($"{nameof(instance)}-Instance is already set, destroying new one.");
                Destroy(value);
            }
            instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CallGlobalSetups();
    }

    private void CallGlobalSetups()
    {
        ClientSettings.Instance.Setup();
    }

    public Action OnApplicationQuitCall = () => {};


    private void OnApplicationQuit()
    {
        OnApplicationQuitCall.Invoke();
    }

}
