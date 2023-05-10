using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class ImGuiUtils
{
    public static void HelpMarker(string hint)
    {
        ImGui.TextDisabled("(?)");
        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.PushTextWrapPos(ImGui.GetFontSize()* 35.0f);
            ImGui.TextUnformatted(hint);
            ImGui.PopTextWrapPos();
            ImGui.EndTooltip();
        }
    }

}
