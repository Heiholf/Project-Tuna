using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using Tuna;

public class ClientSettingsDisplayHandler : MonoBehaviour
{
    private void OnEnable()
    {
        DearImGuiWindow window = new DearImGuiWindow();
        window.name = "Settings";
        window.content = RenderSettings;

        DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.ShownOnPause);
    }


    private void RenderSettings()
    {
        ImGui.BeginChild("body");
        RenderSavingSettings();
        
        ImGui.EndChild();
        
    }


    private void RenderSavingSettings()
    {
        if (ImGui.CollapsingHeader("Saving"))
        {
            string saveDirectoryPath = ClientSettings.Instance.SaveDirectoryPath;
            bool errorOcurred = false;
            ImGui.InputText("Saving Root Directory", ref saveDirectoryPath, 100);
            ImGui.SameLine();
            ImGuiUtils.HelpMarker("The directory where local data is stored. \nDefault: <Game>/");
            if (!saveDirectoryPath.Equals(ClientSettings.Instance.SaveDirectoryPath))
            {
                bool result = SaveManager.Instance.SetBasePath(saveDirectoryPath);
                errorOcurred = !result;
            }
            if (errorOcurred)
            {
                ImGui.TextColored(Color.red, "Input invalid!");
                ImGui.SameLine();
                ImGuiUtils.HelpMarker("The given input is invalid. Maybe the path does not exist or does not lead to a directory.");
            }
        }
    }
}
