using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ClientSettings 
{
    private static ClientSettings instance;

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
}
