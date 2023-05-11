using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class ConsoleDisplayHandler : MonoBehaviour
{

    [Header("Window Appearance")]
    [SerializeField]
    private string consoleWindowName = "Console";
    [SerializeField]
    private DearImGuiWindowState baseState = DearImGuiWindowState.ShownOnPause;

    



    void Start()
    {
        AddConsoleWindowToWindows();
        ConsoleHandler.Instance.GetMemoizedConsoleString();
    }

    void AddConsoleWindowToWindows()
    {
        DearImGuiWindow consoleWindow = new DearImGuiWindow();
        consoleWindow.name = consoleWindowName;
        consoleWindow.content = RenderConsoleWindow;
        DearImGuiWindowHandler.Instance.AddWindow(consoleWindow, baseState);
    }

    void RenderConsoleWindow()
    {
        string consoleContent = ConsoleHandler.Instance.GetMemoizedConsoleString();
        ImGui.TextUnformatted(consoleContent);

    }

    
}
