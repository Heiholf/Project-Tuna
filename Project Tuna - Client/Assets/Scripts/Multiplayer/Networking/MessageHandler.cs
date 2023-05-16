using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class MessageHandler : MonoBehaviour
{
    [MessageHandler((ushort)ServerToClientMessageID.updatedUsername)]
    private static void UpdatedUsernameMessageHandler(Message message)
    {
        string username = message.GetString();
        ClientSettings.Instance.username = username;
        ConnectionUIHandler.Instance.OnGameJoined();
    }

    [MessageHandler((ushort)ServerToClientMessageID.joinRejectReason)]
    private static void JoinRejectReasonMessageHandler(Message message)
    {
        string reason = message.GetString();
        DearImGuiWindowHandler.Instance.ShowSimplePopup("Could not connect",$"You were unable to join the lobby because:\n{reason}");
    }
}
