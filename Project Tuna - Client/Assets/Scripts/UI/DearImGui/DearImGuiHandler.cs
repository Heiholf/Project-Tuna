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
        DearImGuiWindowHandler.Instance.MenuBar();
        foreach (DearImGuiWindow window in DearImGuiWindowHandler.Instance.WindowsToBeRendered)
        {
            ImGui.Begin(window.name);
            window.Render();
            ImGui.End();
        }
        foreach(string popupName in DearImGuiWindowHandler.Instance.popupsToBeShown)
        {
            ImGui.OpenPopup(popupName);
        }
        foreach (DearImGuiPopup popup in DearImGuiWindowHandler.Instance.popups.Values)
        {
            PopupCloseBehaviour closeBehaviour = popup.closeBehaviour;
            if(closeBehaviour == PopupCloseBehaviour.CloseOnClick)
            {
                if (ImGui.BeginPopup(popup.name))
                {
                    popup.content.Invoke();
                    ImGui.EndPopup();
                }
            } else if(closeBehaviour == PopupCloseBehaviour.DontCloseOnClick)
            {
                if (ImGui.BeginPopupModal(popup.name))
                {
                    popup.content.Invoke();
                    ImGui.EndPopup();
                }
            }
            
        }
        foreach(IEnumerator enumerator in DearImGuiWindowHandler.Instance.coroutinesToBeStarted)
        {
            StartCoroutine(enumerator);
        }
        
        DearImGuiWindowHandler.Instance.ResetPopups();

    }
}
