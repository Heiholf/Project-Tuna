using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public struct ConsoleEntry
{
    public DateTime time;
    public string text;

    public ConsoleEntry(DateTime _time, string _text)
    {
        time = _time;
        text = _text;
    }

    public override string ToString()
    {
        return $"[{time.ToString("T", CultureInfo.GetCultureInfo("de-DE"))}] {text}";
    }
}

public class ConsoleHandler
{

    private static ConsoleHandler instance;

    
    public static ConsoleHandler Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ConsoleHandler();
            }
            return instance;
        }
    }

    [Range(0, float.MaxValue)]
    const int consoleLength = 100;
    private ConsoleEntry[] consoleEntries = new ConsoleEntry[consoleLength];
    private string consoleContent = "";
    private bool consoleUpdateOccurred = true;

    private uint mostRecentConsoleEntry = 0;

    public string GetMemoizedConsoleString()
    {
        if (consoleUpdateOccurred)
        {
            UpdateConsoleContent();
            consoleUpdateOccurred = false;
        }
        return consoleContent;
    }

    private void UpdateConsoleContent()
    {
        string newContent = "";
        for(int i = (int)mostRecentConsoleEntry; i != mostRecentConsoleEntry + 1; i = (i + consoleLength - 1) % consoleLength)
        {
            if(new DateTime() != consoleEntries[i].time)
            {
                newContent += consoleEntries[i] + "\n";
            }
            
        }
        consoleContent = newContent;
    }


    public void AddConsoleEntry(ConsoleEntry entry)
    {
        consoleEntries[++mostRecentConsoleEntry % consoleLength] = entry;
        consoleUpdateOccurred = true;
    }

    public void AddLog(string text)
    {
        AddConsoleEntry(new ConsoleEntry(DateTime.Now, text));
    }

    

    const int commandStorageLength = 20;
    private string[] commandStorage = new string[commandStorageLength];

    private int mostRecentCommandStorageEntry = 0;


    public string GetRecentCommand(int index)
    {
        return commandStorage[(mostRecentCommandStorageEntry - index + commandStorageLength) % commandStorageLength];
    }

    public void AddRecentCommand(string command)
    {
        commandStorage[++mostRecentCommandStorageEntry % commandStorageLength] = command;
    }

    void ExecuteCommand(string command)
    {

    }
}
