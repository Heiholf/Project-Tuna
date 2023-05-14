using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class MessageHandler : MonoBehaviour
{
    
    [MessageHandler((ushort)ClientToServeMessageID.joinGame)]
    private static void JoinGameMessageHandler(ushort fromClientId, Message message)
    {
        Debug.Log($"Received JoinGameMessage from {message.GetString()}");
    }
}
