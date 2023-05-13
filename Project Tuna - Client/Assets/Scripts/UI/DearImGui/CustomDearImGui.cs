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

    public static void ErrorText(string text)
    {
        ImGui.TextColored(Color.red, text);
    }

    public static void ErrorTextWithHint(string text, string hint)
    {
        ImGui.TextColored(Color.red, text);
        ImGui.SameLine();
        ImGuiUtils.HelpMarker(hint);
    }

}
