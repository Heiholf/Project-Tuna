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
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            DearImGuiWindow window = new DearImGuiWindow();
            window.content = WindowDraw;
            window.name = "TestWindow";


            DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
        }*/

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            DearImGuiWindow window = new DearImGuiWindow();
            window.content = WindowDraw2;
            window.name = "TestWindow2";

            DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
        }*/

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            DearImGuiWindowHandler.Instance.ShowSimplePopup("Test", "Dies ist eine tolle Testnachricht :)");
        }*/

      

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
            ImGui.OpenPopup("Delete?");

        bool test = true;
        // Always center this window when appearing
        if (ImGui.BeginPopupModal("Delete?", ref test))
        {
            ImGui.Text("All those beautiful files will be deleted.\nThis operation cannot be undone!");
            ImGui.Separator();

            //static int unused_i = 0;
            //ImGui.Combo("Combo", &unused_i, "Delete\0Delete harder\0");

            bool dont_ask_me_next_time = false;
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0, 0));
            ImGui.Checkbox("Don't ask me next time", ref dont_ask_me_next_time);
            ImGui.PopStyleVar();

            if (ImGui.Button("OK", new Vector2(120, 0))) { ImGui.CloseCurrentPopup(); }
            ImGui.SetItemDefaultFocus();
            ImGui.SameLine();
            if (ImGui.Button("Cancel", new Vector2(120, 0))) { ImGui.CloseCurrentPopup(); }
            ImGui.EndPopup();
        }
    }
}
