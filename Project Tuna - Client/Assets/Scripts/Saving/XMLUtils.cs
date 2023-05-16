using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class XMLUtils 
{
    
    public static string SerializeClassToXML<T>(T classObject){
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(classObject.GetType());
        StringWriter writer = new StringWriter();
        serializer.Serialize(writer, classObject);
        string xmlString = writer.ToString();
        return xmlString;
    }

    public static T ParseClassFromXML<T>(string xmlString)
    {
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        StringReader reader = new StringReader(xmlString);
        object obj = serializer.Deserialize(reader);
        return (T)obj;
    }
}
