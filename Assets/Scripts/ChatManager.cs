using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;

    public UserController user;
    
    public Text textChat;
    public InputField inputMessage;

    public GameObject contentMessage;
    public GameObject messageGroup;
    public GameObject message;


    void Awake()
    {
        ChatManager.instance = this;
    }
}