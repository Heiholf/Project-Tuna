using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public struct ClientData
{
    public string username;
    public ushort id;

    public ClientData(string _username, ushort _id)
    {
        username = _username;
        id = _id;
    }
}

public class ClientManager : MonoBehaviour
{

    #region Singleton
    private static ClientManager instance;

    public static ClientManager Instance
    {
        get
        {
            if (instance is null)
                Debug.LogError($"{nameof(ClientManager)}-Instance has not yet been created.");
            return instance;
        }
        set
        {
            if (instance is not null)
            {
                Debug.LogWarning($"{nameof(ClientManager)}-Instance has already been created. Destroying new one.");
                Destroy(value);

                return;
            }
            instance = value;
        }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public Dictionary<ushort, string> clientIDs = new Dictionary<ushort, string>();

    public Dictionary<string, ClientData> clients = new Dictionary<string, ClientData>();

    public string AddClient(ClientData client)
    {
        if (clients.ContainsKey(client.username))
        {
            client.username = Tuna.Utils.AddEnumerationToString(client.username);
            return AddClient(client);
        }
        clients[client.username] = client;
        return client.username;
    }

    public void RemoveClient(string username)
    {
        clientIDs.Remove(clients[username].id);
        clients.Remove(username);
    }

    public void RemoveClient(ushort id)
    {
        clients.Remove(clientIDs[id]);
        clientIDs.Remove(id);
    }



}
