using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ClientState
{
    Unpaused,
    Paused
}

public class ClientStateHandler : MonoBehaviour
{
    private static ClientStateHandler instance;

    public static ClientStateHandler Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("ClientStateHandler-Instance does not exist!");
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(instance is null)
        {
            instance = this;
        } else
        {
            Debug.LogWarning($"ClientStateHandler-Instance already exists! Destory myself: {this.name}.");
            Destroy(this);
        }
    }

    public ClientState clientState = ClientState.Unpaused;

    public Action<ClientState> OnClientStateChanged;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (Instance.clientState)
            {
                case ClientState.Paused:
                    Instance.clientState = ClientState.Unpaused;
                    break;
                case ClientState.Unpaused:
                    Instance.clientState = ClientState.Paused;
                    break;
            }
            OnClientStateChanged.Invoke(Instance.clientState);
        }
    }

    public static DisplayState ClientStateToDisplayState(ClientState clientState)
    {
        switch (clientState)
        {
            case ClientState.Paused:
                return DisplayState.Paused;
            case ClientState.Unpaused:
                return DisplayState.Resuming;
            default:
                Debug.LogError($"ClientState {clientState} could not be transformed to DisplayState");
                return DisplayState.Undefined;
                
        }
    }



}
