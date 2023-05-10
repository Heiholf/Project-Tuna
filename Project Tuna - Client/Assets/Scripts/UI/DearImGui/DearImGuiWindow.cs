using System.Collections;
using System.Collections.Generic;
using System;
using ImGuiNET;

public class DearImGuiWindow
{
    public string name;
    public Action content;


    public void Render()
    {
        content.Invoke();
    }


    public DearImGuiWindow Clone()
    {
        DearImGuiWindow clone = new DearImGuiWindow();
        clone.name = name;
        clone.content = content;
        return clone;
    }

}
