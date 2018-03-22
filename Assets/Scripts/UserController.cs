using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UserController : NetworkBehaviour
{
    public string username;

    private Text textChat;
    private Text textUsername;
    private InputField inputMessage;

    private Button buttonSend;

    void Start ()
    {
        if(!isLocalPlayer)
        {
            enabled = false;

            return;
        }

        ChatManager.instance.user = this;
        username = "USER-" + Random.Range(100, 1000);

        textChat = GameObject.Find("TextChat").GetComponent<Text>();
        textUsername = GameObject.Find("TextUsername").GetComponent<Text>();
        inputMessage = GameObject.Find("InputMessage").GetComponent<InputField>();

        textUsername.text = username;

        CmdJoinMessage(username);

        buttonSend = GameObject.Find("ButtonSend").GetComponent<Button>();
        buttonSend.onClick.AddListener(delegate { CmdSendMessage(username + " : " + inputMessage.text); });

        //Debug.Log("JOIN : " + username);
    }

    [Command]
    void CmdJoinMessage(string username)
    {
        RpcJoinMessage(username);
    }

    [ClientRpc]
    void RpcJoinMessage(string username)
    {
        ChatManager.instance.textChat.text += username + "님이 접속하셨습니다.\n";
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        if (isServer)
            RpcSendMessage(message);
    }

    [ClientRpc]
    public void RpcSendMessage(string message)
    {
        if (isLocalPlayer)
            inputMessage.text = "";

        ChatManager.instance.textChat.text += message + "\n";
    }
}