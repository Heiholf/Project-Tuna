using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class DearImGuiHandler : MonoBehaviour
{

    private void OnEnable()
    {
        ImGuiUn.Layout += OnLayout;
    }

    private void OnDisable()
    {
        ImGuiUn.Layout -= OnLayout;
    }

    private void OnLayout()
    {
        ImGui.ShowDemoWindow();
    }
}
