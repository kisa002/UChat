using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatManager : NetworkBehaviour {

    Text textChat;
    public Text inputMessage;

    Button buttonSend;

    [SyncVar]
    public string msg;
    
	void Start ()
    {
        Init();
	}

    void Init()
    {
        textChat = GameObject.Find("TextChat").GetComponent<Text>();
        inputMessage = GameObject.Find("InputMessage").transform.FindChild("Text").GetComponent<Text>();

        buttonSend = GameObject.Find("ButtonSend").GetComponent<Button>();

        //if (isLocalPlayer)
            buttonSend.onClick.AddListener(delegate { MessageSend(); });
    }
	
	void Update ()
    {
        MessageAdd();
    }

    void MessageAdd()
    {
        if (msg != null)
        {
            textChat.text += msg;

            msg = null;
            inputMessage.text = "";
        }
    }

    public void MessageSend()
    {
        CmdMessageSend();
        RpcMessageSend();
    }

    [Command]
    public void CmdMessageSend()
    {
        msg = inputMessage.text + "\n";
    }

    [ClientRpc]
    public void RpcMessageSend()
    {
        msg = inputMessage.text + "\n";
    }
}
