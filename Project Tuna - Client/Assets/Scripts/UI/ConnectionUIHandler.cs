using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using System.Text.RegularExpressions;

public class ConnectionUIHandler : MonoBehaviour
{

    #region Singleton
    private static ConnectionUIHandler instance;

    public static ConnectionUIHandler Instance
    {
        get
        {
            if (instance is null)
                Debug.LogError($"{nameof(ConnectionUIHandler)}-Instance has not yet been created.");
            return instance;
        }
        set
        {
            if (instance is not null)
            {
                Debug.LogWarning($"{nameof(ConnectionUIHandler)}-Instance has already been created. Destroying new one.");
                Destroy(value);

                return;
            }
            instance = value;
        }
    }
    #endregion

    [SerializeField] private string connectToServerWindowName = "Connect to Server";
    [SerializeField] private string joinGameWindowName = "Join Game";


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        AddConnectionWindow();
        AddJoinGameWindow();
        NetworkManager.Instance.OnConnectionLostEvent += OnConnectionStopped;
        NetworkManager.Instance.OnLocalClientConnectedEvent += OnLocalClientConnected;

    }

    #region ConnectToServerWindow

    void AddConnectionWindow()
    {
        DearImGuiWindow window = new DearImGuiWindow();
        window.content = RenderConnectToServerWindow;
        window.name = connectToServerWindowName;
        connectToServerWindowName = DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysShown);
    }



    readonly Regex ipRegex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");


    void RenderConnectToServerWindow()
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
        }
        else
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
        }
        else
        {
            ImGuiUtils.ErrorTextWithHint("port invalid", $"The given input does not match the required shape for a port. The port has to be a positive integer below {ushort.MaxValue}.");
            isWithoutErrors = false;
        }

        if (!isWithoutErrors)
            ImGui.PushStyleColor(ImGuiCol.Button, Color.gray);
        if (ImGui.Button("Connect to Server") && isWithoutErrors)
        {
            OnConnectPressed();

            if (!isWithoutErrors)
                ImGui.PopStyleColor();

        } 
    }

    #endregion

    #region JoinGameWindow

    void AddJoinGameWindow()
    {
        DearImGuiWindow window = new DearImGuiWindow();
        window.content = RenderJoinGameWindow;
        window.name = joinGameWindowName;
        joinGameWindowName = DearImGuiWindowHandler.Instance.AddWindow(window, DearImGuiWindowState.AlwaysHidden);
    }

    void RenderJoinGameWindow()
    {
        string username = ClientSettings.Instance.username;
        ImGui.InputText("Username", ref username, 15);
        ImGui.SameLine();
        ImGuiUtils.HelpMarker("Enter the name other players will see in game.");

        if(checkUsername(username) == UsernameResult.Valid)
        {
            ClientSettings.Instance.username = username;
            if(ImGui.Button("Join Game"))
            {
                JoinGame();
            }
        } else
        {
            string error;
            switch(checkUsername(username))
            {
                case UsernameResult.TooShort:
                    error = "Entered username is too short.";
                    break;
                case UsernameResult.ContainsSpaces:
                    error = "Entered username contains spaces.";
                    break;
                default:
                    error = "";
                    break;
            }
            ImGuiUtils.ErrorText(error);
        }


    }

    enum UsernameResult {
        Valid, 
        TooShort,
        ContainsSpaces,
    }

    UsernameResult checkUsername(string username)
    {
        if(username.Length < 3)
            return UsernameResult.TooShort;
        if (username.Contains(" "))
            return UsernameResult.ContainsSpaces;
        return UsernameResult.Valid;

    }

    void JoinGame()
    {
        NetworkManager.Instance.SendJoinGameMessage(ClientSettings.Instance.username);
        Debug.Log("Joining game");
    }

    public void OnGameJoined()
    {
        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(joinGameWindowName, DearImGuiWindowState.AlwaysHidden);
    }

    #endregion

    #region EventHandlers

    void OnConnectPressed()
    {
        NetworkManager.Instance.ConnectToServer(ClientSettings.Instance.serverIp, ClientSettings.Instance.serverPort);

        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(connectToServerWindowName, DearImGuiWindowState.AlwaysHidden);
    }

    void OnConnectionStopped()
    {
        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(connectToServerWindowName, DearImGuiWindowState.AlwaysShown);
        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(joinGameWindowName, DearImGuiWindowState.AlwaysHidden);
    }

    void OnLocalClientConnected()
    {
        DearImGuiWindowHandler.Instance.UpdateDisplayStateOfWindow(joinGameWindowName, DearImGuiWindowState.AlwaysShown);
    }

    #endregion
}
