using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using Tuna;
using System;

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
        RenderInputSettings();

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
                ImGuiUtils.ErrorTextWithHint("Input invalid!", "The given input is invalid. Maybe the path does not exist or does not lead to a directory.");
            }
        }
    }

    private void RenderInputSettings()
    {
        if (ImGui.CollapsingHeader("Input"))
        {
            if (ImGui.TreeNode("Basic"))
            {
                foreach (KeyValuePair<KeyCode, string> pair in ClientSettings.Instance.inputSettings.buttonToDescriptionMap)
                {
                    RenderBasicInputElement(pair);
                }
                ImGui.TreePop();
            }
            if (ImGui.TreeNode("Advanced"))
            {
                foreach (KeyValuePair<KeyCode, KeyCode> pair in ClientSettings.Instance.inputSettings.buttonToInputMap)
                {
                    RenderAdvancedInputElement(pair);
                }
                ImGui.TreePop();
            }
        }
    }

    private void RenderBasicInputElement(KeyValuePair<KeyCode, string> pair)
    {

        ImGui.Text(pair.Value);
        ImGui.SameLine();
        string buttonText = InputHandler.ConvertDesiredKeyToInputKey(pair.Key).ToString();
        if (ImGui.Button(buttonText))
        {
            buttonText = "";
            StartCoroutine(WaitForKeyInput((KeyCode newKey) =>
            {
                ClientSettings.Instance.inputSettings.RemapKey(pair.Key, newKey);
            }));
        }

    }

    private void RenderAdvancedInputElement(KeyValuePair<KeyCode, KeyCode> pair)
    {

        ImGui.Text(pair.Key.ToString());
        ImGui.SameLine();
        string buttonText = pair.Value.ToString();
        if (ImGui.Button(buttonText))
        {
            buttonText = "";
            StartCoroutine(WaitForKeyInput((KeyCode newKey) =>
            {
                ClientSettings.Instance.inputSettings.RemapKey(pair.Key, newKey);
            }));
        }

    }

    private IEnumerator WaitForKeyInput(Action<KeyCode> callback)
    {
        yield return new WaitForSecondsRealtime(0.1f);

        yield return new WaitUntil(() =>
        {
            if (Input.anyKeyDown)
            {
                callback.Invoke(InputHandler.GetCurrentPressedKey());
                return true;
            }
            return false;
        });

    }
}
