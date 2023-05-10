using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class DearImGuiSaveHandler : MonoBehaviour
{
    private void Start()
    {
        ImGui.LoadIniSettingsFromDisk(SaveManager.Instance.GetPathFromBasePath("imgui_inifile.ini"));
        Debug.Log("Loaded config");
    }

    private void OnApplicationQuit()
    {
        ImGui.SaveIniSettingsToDisk(SaveManager.Instance.GetPathFromBasePath("imgui_inifile.ini"));
        Debug.Log("Saved config");
    }
}
