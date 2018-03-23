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

        if (isLocalPlayer)
            inputMessage.text = "";
    }

    [ClientRpc]
    public void RpcSendMessage(string message)
    {
        // ChatManager.instance.textChat.text += message + "\n";

        GameObject msg = Instantiate(ChatManager.instance.message);
        msg.transform.Find("Text").GetComponent<Text>().text = message;
        msg.transform.SetParent(ChatManager.instance.contentMessage.transform);

        int childCount = ChatManager.instance.contentMessage.transform.childCount;

        if(childCount > 3)
            ChatManager.instance.contentMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatManager.instance.contentMessage.GetComponent<RectTransform>().rect.width, (98 * childCount - 1) - (98 * 3));
        
        if(childCount == 1)
            msg.transform.localPosition = new Vector2(150, -75);
        else
            msg.transform.localPosition = new Vector2(150, (ChatManager.instance.contentMessage.transform.GetChild(childCount - 1).position.y) - 65 * (childCount - 1));

        if(!isLocalPlayer)
        {
            msg.transform.localPosition = new Vector2(800f, msg.transform.localPosition.y);

            msg.GetComponent<Image>().color = new Color(175f / 255f, 98f / 255f, 218f / 255f, 1f);
            msg.transform.Find("Text").GetComponent<Text>().color = Color.white;
        }

        if(isServer)
            NetworkServer.Spawn(msg);
    }
}