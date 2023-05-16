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

public struct DearImGuiPopup
{
    public Action content;
    public string name;
    public PopupCloseBehaviour closeBehaviour;

    public DearImGuiPopup(Action _content, string _name, PopupCloseBehaviour _closeBehaviour = PopupCloseBehaviour.CloseOnClick)
    {
        content = _content;
        name = _name;
        closeBehaviour = _closeBehaviour;
    }
}

public enum PopupCloseBehaviour
{
    CloseOnClick,
    DontCloseOnClick
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

    #region Windows
    public string AddWindow(DearImGuiWindow window, DearImGuiWindowState baseState)
    {
        Setup();
        if (Instance.Windows.ContainsKey(window.name))
        {
            window.name = Tuna.Utils.AddEnumerationToString(window.name);
            return AddWindow(window, baseState);
        }

        hasWindowStateChanged = true;
        Instance.Windows.Add(window.name, new DearImGuiWindowStruct(window, baseState));
        return window.name;
    }

    public void UpdateDisplayStateOfWindow(string windowTitle, DearImGuiWindowState newState)
    {
        DearImGuiWindowStruct windowStruct = Windows[windowTitle];
        windowStruct.state = newState;
        Windows[windowTitle] = windowStruct;
        hasWindowStateChanged = true;
    }

    private void ClientStateChanged(ClientState clientState)
    {

        DisplayState = ClientStateHandler.ClientStateToDisplayState(ClientStateHandler.Instance.clientState);
    }

    #endregion

    #region MenuBar
    public void MenuBar()
    {
        if (Instance.DisplayState == DisplayState.Paused)
            Instance.RenderMenuBar();


    }

    readonly Dictionary<DearImGuiWindowState, Color> windowStateToColorMap = new Dictionary<DearImGuiWindowState, Color>{
        {DearImGuiWindowState.AlwaysShown, new Color(50,205,0)},
        {DearImGuiWindowState.ShownOnPause, Color.white},
        {DearImGuiWindowState.Hidden, new Color(235, 86, 0)},
        {DearImGuiWindowState.AlwaysHidden, new Color(0, 0, 0)}
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
            foreach (KeyValuePair<string, DearImGuiWindowStruct> pair in Instance.Windows)
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
                    if (ImGui.MenuItem("Hide during Play", "", shownOnPause))
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

    #endregion

    #region Popup & Modals

    //TODO: Implement smarter way of handling the removal of popups.

    public Dictionary<string, DearImGuiPopup> popups = new Dictionary<string, DearImGuiPopup>();

    public List<string> popupsToBeShown = new List<string>();

    public List<IEnumerator> coroutinesToBeStarted = new List<IEnumerator>();

    public string AddPopup(DearImGuiPopup popup)
    {
        if (Instance.popups.ContainsKey(popup.name))
        {
            popup.name = Tuna.Utils.AddEnumerationToString(popup.name);
            return AddPopup(popup);
        }

        Instance.popups.Add(popup.name, popup);
        return popup.name;
    }

    public void RemovePopup(string name)
    {
        popups.Remove(name);
    }

    
    public void ShowPopup(string name)
    {
        popupsToBeShown.Add(name);
    }

    public void ResetPopups()
    {
        popupsToBeShown = new List<string>();
        coroutinesToBeStarted = new List<IEnumerator>();
    }

    public IEnumerator WaitForPopupToBeRemoved(string name)
    {
        yield return new WaitForSecondsRealtime(3);
        RemovePopup(name);
    }

    public void ShowSimplePopup(string name, string content)
    {
        Action renderContent = () =>
        {
            ImGui.TextWrapped(content);
            if (ImGui.Button("Close"))
            {
                ImGui.CloseCurrentPopup();
            }
        };
        DearImGuiPopup popup = new DearImGuiPopup(renderContent, name, PopupCloseBehaviour.DontCloseOnClick);
        name = AddPopup(popup);
        ShowPopup(name);
        coroutinesToBeStarted.Add(WaitForPopupToBeRemoved(name));

    }

    #endregion
}
