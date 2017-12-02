using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatController : NetworkBehaviour {

    ChatManager chatManager;

    string msg;

	// Use this for initialization
	void Start ()
    {
        chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void MessageAdd()
    {
        if (!isLocalPlayer)
            return;

        if (msg != null)
        {
            //textChat.text += msg;

            msg = null;
        }
    }

    public void MessageSend(string msg)
    {
        this.msg = msg;

        MessageAdd();
    }

    public void InputMessageSend()
    {
        //MessageSend(inputMessage.text);
        //inputMessage.text = "";
    }
}