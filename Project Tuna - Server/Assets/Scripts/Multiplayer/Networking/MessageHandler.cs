using Riptide;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{

    [MessageHandler((ushort)ClientToServeMessageID.joinGame)]
    private static void JoinGameMessageHandler(ushort fromClientId, Message message)
    {

        if (!GameStateManager.Instance.isGameJoinable())
        {
            Debug.Log($"Rejected JoinGameMessage");
            SendJoinBlockReason(fromClientId);
            return;
        }

        #region Username

        string username = message.GetString();

        string updatedUsername = ClientManager.Instance.AddClient(new ClientData(username, fromClientId));

        SendUpdatedUsernameMessage(fromClientId, updatedUsername);

        #endregion


        Debug.Log($"Received JoinGameMessage from {username}");
    }

    private static void SendUpdatedUsernameMessage(ushort toClient, string updatedUsername)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageID.updatedUsername);

        message.AddString(updatedUsername);

        NetworkManager.Instance.Server.Send(message, toClient);
    }

    private static void SendJoinBlockReason(ushort toClient)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageID.joinRejectReason);

        message.AddString(GameStateManager.Instance.GetGameJoinabilityReason());

        NetworkManager.Instance.Server.Send(message, toClient);
    }
}
