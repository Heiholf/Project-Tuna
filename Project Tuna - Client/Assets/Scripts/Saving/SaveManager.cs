using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager
{
    private static SaveManager instance = null;

    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveManager();
            }
            return instance;
        }
    }

    

    private string basePath = ClientSettings.Instance.SaveDirectoryPath;

    public string BasePath
    {
        get { return Instance.basePath; }
    }

    public bool SetBasePath(string newPath)
    {
        if (Directory.Exists(newPath))
        {
            basePath = newPath;
            return true;
        }
        return false;
    }


    public string GetPathFromBasePath(string subpath)
    {
        return Path.GetFullPath(Path.Combine(BasePath, subpath));
    }

    // Warning: Untested
    private bool MoveBasePathWithFiles(string newPath)
    {
        if (Directory.Exists(newPath))
        {
            Directory.Move(basePath, newPath);
            basePath = newPath;
            return true;
        }
        return false;
    }


    public void SaveClassAsXMLFile<T>(T classObject, string subpath)
    {
        string xmlString = XMLUtils.SerializeClassToXML<T>(classObject);

        File.Delete(Path.Combine(BasePath, subpath));
        File.WriteAllText(Path.Combine(BasePath, subpath), xmlString);
    }

    public T ReadClassFromXMLFile<T>(string subpath)
    {
        string xmlString = File.ReadAllText(Path.Combine(BasePath, subpath));

        return XMLUtils.ParseClassFromXML<T>(xmlString);
    }



}
