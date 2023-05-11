using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using ImGuiNET;
using System;
using Tuna;

public enum DearImGuiWindowState
{
    AlwaysShown,
    ShownOnPause,
    Hidden,
    AlwaysHidden,
}

public enum DisplayState
{
    Resuming,
    Paused,
    Undefined,
}

public struct DearImGuiWindowStruct
{
    public DearImGuiWindow window;
    public DearImGuiWindowState state;

    public DearImGuiWindowStruct(DearImGuiWindow _window, DearImGuiWindowState _state)
    {
        window = _window;
        state = _state;
    }
}


public class DearImGuiWindowHandler
{
    private static DearImGuiWindowHandler instance = null;

    public static DearImGuiWindowHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DearImGuiWindowHandler();
            }
            return instance;
        }
    }

    private DisplayState _displayState = ClientStateHandler.ClientStateToDisplayState(ClientStateHandler.Instance.clientState);

    public DisplayState DisplayState
    {
        get { return _displayState; }
        set
        {
            hasWindowStateChanged = true;
            _displayState = value;
        }
    }


    private bool hasWindowStateChanged = false;

    private Dictionary<string, DearImGuiWindowStruct> _windows = new Dictionary<string, DearImGuiWindowStruct>();

    public Dictionary<string, DearImGuiWindowStruct> Windows
    {
        get { return _windows; }
        set
        {
            hasWindowStateChanged = true;
            _windows = value;
        }
    }

    private List<DearImGuiWindow> memoizedWindowsToBeRendered = new List<DearImGuiWindow>();

    private List<DearImGuiWindow> CalculateWindowsToBeRendered()
    {
        List<DearImGuiWindow> result = new List<DearImGuiWindow>();
        foreach (DearImGuiWindowStruct windowStruct in Instance.Windows.Values)
        {
            if (windowStruct.state == DearImGuiWindowState.AlwaysHidden || windowStruct.state == DearImGuiWindowState.Hidden)
                continue;
            else if (windowStruct.state == DearImGuiWindowState.AlwaysShown)
                result.Add(windowStruct.window);
            else if (windowStruct.state == DearImGuiWindowState.ShownOnPause && Instance.DisplayState == DisplayState.Paused)
                result.Add(windowStruct.window);

        }

        return result;
    }

    private List<DearImGuiWindow> GetWindowsToBeRendered()
    {
        if (hasWindowStateChanged)
        {
            Instance.memoizedWindowsToBeRendered = CalculateWindowsToBeRendered();
            hasWindowStateChanged = false;
        }
        return Instance.memoizedWindowsToBeRendered;
    }

    public List<DearImGuiWindow> WindowsToBeRendered
    {
        get { return Instance.GetWindowsToBeRendered(); }
    }

    bool isSetup = false;

    private void Setup()
    {
        if (isSetup)
        {
            return;
        }
        ClientStateHandler.Instance.OnClientStateChanged += ClientStateChanged;
        isSetup = true;
    }

    public string AddWindow(DearImGuiWindow window, DearImGuiWindowState baseState)
    {
        Setup();
        if (Instance.Windows.ContainsKey(window.name))
        {
            Regex regex = new Regex(@".* - (?<number>\d+)");
            Match match = regex.Match(window.name);
            if (match.Success)
            {
                int number = int.Parse(match.Groups["number"].Value);
                string newName = window.name.Substring(0, match.Groups["number"].Index) + (number + 1);
                window.name = newName;
            } else
            {
                window.name += " - 2";
            }
            return AddWindow(window, baseState);
        }

        hasWindowStateChanged = true;
        Instance.Windows.Add(window.name, new DearImGuiWindowStruct(window, baseState));
        return window.name;
    }

    private void ClientStateChanged(ClientState clientState)
    {

        DisplayState = ClientStateHandler.ClientStateToDisplayState(ClientStateHandler.Instance.clientState);
    }


    public void MenuBar()
    {
        if (Instance.DisplayState == DisplayState.Paused)
            Instance.RenderMenuBar();


    }

    readonly Dictionary<DearImGuiWindowState, Color> windowStateToColorMap = new Dictionary<DearImGuiWindowState, Color>{
        {DearImGuiWindowState.AlwaysShown, new Color(50,205,0)},
        {DearImGuiWindowState.ShownOnPause, Color.white},
        {DearImGuiWindowState.Hidden, new Color(235, 86, 0)}
    };

    private bool RenderWindowMenuItem(DearImGuiWindowStruct windowStruct, DearImGuiWindowState state)
    {
        
        ImGui.PushStyleColor(ImGuiCol.Text, Instance.windowStateToColorMap[state]);
        bool val = ImGui.BeginMenu(windowStruct.window.name, state != DearImGuiWindowState.AlwaysHidden);
        ImGui.PopStyleColor();
        return val;
    }

    private void RenderMenuBar()
    {
        ImGui.BeginMainMenuBar();
        if (ImGui.BeginMenu("Windows"))
        {
            foreach(KeyValuePair<string, DearImGuiWindowStruct> pair in Instance.Windows)
            {
                DearImGuiWindowStruct windowStruct = pair.Value;
                DearImGuiWindowState state = windowStruct.state;
                //TODO: Fix weird color bug
                if (RenderWindowMenuItem(windowStruct, state))
                {
                    bool alwaysShown = state == DearImGuiWindowState.AlwaysShown;
                    bool shownOnPause = state == DearImGuiWindowState.ShownOnPause;
                    bool hidden = state == DearImGuiWindowState.Hidden;

                    bool somethingChanged = false;

                    if (ImGui.MenuItem("Show always", "", alwaysShown))
                    {
                        windowStruct.state = DearImGuiWindowState.AlwaysShown;
                        somethingChanged = true;
                    }
                    if(ImGui.MenuItem("Hide during Play", "", shownOnPause))
                    {
                        windowStruct.state = DearImGuiWindowState.ShownOnPause;
                        somethingChanged = true;
                    }
                    if (ImGui.MenuItem("Hide always", "", hidden))
                    {
                        windowStruct.state = DearImGuiWindowState.Hidden;
                        somethingChanged = true;
                    }

                    if (somethingChanged)
                    {
                        Instance.hasWindowStateChanged = true;
                        Instance.Windows[pair.Key] = windowStruct;
                        ImGui.EndMenu();
                        break;
                    }

                    ImGui.EndMenu();
                   
                }
                
            }
            
            ImGui.EndMenu();
        }
       

        ImGui.EndMainMenuBar();
    }
}
