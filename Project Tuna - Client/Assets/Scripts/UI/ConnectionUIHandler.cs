using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using System.Text.RegularExpressions;

public class ConnectionUIHandler : MonoBehaviour
{

    [SerializeField] private string windowName = "Connect to Server";


    void Start()
    {
        AddConnectionWindow();
        NetworkManager.Instance.OnConnectionLostEvent += OnConnectionStopped;
    }

    void AddConnectionWindow()
    {
        DearImGuiWindow window = new DearImGuiWindow();
        window.content = RenderConnectionWindow;
        window.name = windowName;
        windowName = DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
    }

    readonly Regex ipRegex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");


    void RenderConnectionWindow()
    {

        string ip = ClientSettings.Instance.serverIp;
        string portString = ClientSettings.Instance.serverPort.ToString();
        ushort portUShort = ClientSettings.Instance.serverPort;

        bool isWithoutErrors = true;

        //IP Input
        ImGui.InputText("IP", ref ip, 15);
        ImGui.SameLine();
        ImGuiUtils.HelpMarker("The ip adress of the server you want to connect to. It should be in a form like: 251.235.126.175 \nIf you want to connect to localhost use: 127.0.0.1\n\nYou have to probably ask the hoster of the server for this information.");

        if (!ipRegex.IsMatch(ip))
        {
            ImGuiUtils.ErrorTextWithHint("ip address invalid", "The given input does not match the required shape for a IPv4 address.");
            isWithoutErrors = false;
        } else
        {
            ClientSettings.Instance.serverIp = ip;
        }

        //Port Input
        ImGui.InputText("Port", ref portString, 5);
        ImGui.SameLine();
        ImGuiUtils.HelpMarker("The port the server is listening to.\n\nYou have to ask the hoster of the server for this information.");

        if (ushort.TryParse(portString, out portUShort))
        {
            ClientSettings.Instance.serverPort = portUShort;
        } else
        {
            ImGuiUtils.ErrorTextWithHint("port invalid", $"The given input does not match the required shape for a port. The port has to be a positive integer below {ushort.MaxValue}.");
            isWithoutErrors = false;
        }

        if (!isWithoutErrors)
            ImGui.PushStyleColor(ImGuiCol.Button, Color.gray);
        if (ImGui.Button("Connect to Server") && isWithoutErrors)
        {
            OnConnectPressed();
        }
        if (!isWithoutErrors)
            ImGui.PopStyleColor();


    }

    void OnConnectPressed()
    {
        NetworkManager.Instance.ConnectToServer(ClientSettings.Instance.serverIp, ClientSettings.Instance.serverPort);

        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(windowName, DearImGuiWindowState.AlwaysHidden);
    }

    void OnConnectionStopped()
    {
        
        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(windowName, DearImGuiWindowState.AlwaysShown);
    }

}
