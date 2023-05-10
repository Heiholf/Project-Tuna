using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class AddWindow : MonoBehaviour
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
      
    }

    


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DearImGuiWindow window = new DearImGuiWindow();
            window.content = WindowDraw;
            window.name = "TestWindow";


            DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DearImGuiWindow window = new DearImGuiWindow();
            window.content = WindowDraw2;
            window.name = "TestWindow2";

            DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
        }
    }

    void WindowDraw()
    {
        ImGui.Text("Hello, world");
        if (ImGui.Button("Save"))
        {
            // do stuff
        }
        string s = "";

        ImGui.InputText("string", ref s, 1000);

        float v = 0f;
        ImGui.SliderFloat("float", ref v, 0.0f, 1.0f);
    }

    void WindowDraw2()
    {
        ImGui.Text("Hello, world 2");
        if (ImGui.Button("Save"))
        {
            // do stuff
        }
    }
}
