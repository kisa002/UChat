using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;

    public Text textChat;
    public InputField inputMessage;

    public UserController user;

    void Awake()
    {
        ChatManager.instance = this;
    }
}