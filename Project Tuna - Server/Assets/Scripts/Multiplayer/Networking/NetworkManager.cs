using Riptide;
using Riptide.Utils;
using UnityEngine;

public enum ClientToServeMessageID : ushort
{
    joinGame = 1,
}

public enum ServerToClientMessageID : ushort
{
    updatedUsername = 1,
    joinRejectReason,
}


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
            if(instance is not null)
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

    public Server Server { get; private set; }


    [SerializeField]
    private ushort port;
    [SerializeField]
    private ushort maxClientCount;

   

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();


        Server.ClientDisconnected += OnClientDisconnected;

        Server.Start(port, maxClientCount);
        Debug.Log("Started Server");
    }

    private void FixedUpdate()
    {
        Server.Update();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void OnClientDisconnected(object sender, ServerDisconnectedEventArgs args)
    {
        
    }
}
