using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ClientSettings 
{
    private static ClientSettings instance;

    [SerializeField]
    private const string clientSettingsFileName = "clientSettings.xml";

    public static ClientSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClientSettings();
            }
            return instance;
        }
    }


    public string SaveDirectoryPath = Path.Join(Application.dataPath, "data/");

    public string serverIp = "127.0.0.1";
    public ushort serverPort = 7777;

    public string username = Environment.UserName;


    //TODO: implement handling of changing the SaveDirectoryPath
    public void Setup()
    {
        GlobalCaller.Instance.OnApplicationQuitCall += Save;
        Read();
    }

    public void Read()
    {
        Debug.Log(instance.serverPort);
        ClientSettings readSettings = SaveManager.Instance.ReadClassFromXMLFile<ClientSettings>(clientSettingsFileName);
        //TODO: implement better solution
        ClientSettings.Instance.serverIp = readSettings.serverIp;
        ClientSettings.Instance.serverPort = readSettings.serverPort;
        Debug.Log("Read ClientSettings");
    }

    public void Save()
    {
        SaveManager.Instance.SaveClassAsXMLFile<ClientSettings>(this, clientSettingsFileName);
        Debug.Log("Saved ClientSettings");
    }
}
