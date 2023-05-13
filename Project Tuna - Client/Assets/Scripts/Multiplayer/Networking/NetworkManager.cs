using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            if (instance is null)
                Debug.LogError($"{nameof(NetworkManager)}-Instance has not yet been created.");
            return instance;
        }
        set
        {
            if (instance is not null)
            {
                Debug.LogWarning($"{nameof(NetworkManager)}-Instance has already been created. Destroying new one.");
                Destroy(value);

                return;
            }
            instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public Client Client { get; private set;}

    [SerializeField]
    private string ip;
    [SerializeField]
    private ushort port;

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();

        Client.Disconnected += OnLocalClientDisconnected;
        Client.ConnectionFailed += OnConnectionFailed;
        
    }

    public void ConnectToServer(string ip, ushort port)
    {
        Client.Connect($"{ip}:{port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    private void OnLocalClientDisconnected(object sender, EventArgs args)
    {
        OnConnectionLost();
    }

    private void OnConnectionFailed(object sender, ConnectionFailedEventArgs args)
    {
        OnConnectionLost();
    }

    public Action OnConnectionLostEvent;

    private void OnConnectionLost()
    {
        OnConnectionLostEvent.Invoke();
    }


}
