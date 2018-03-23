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

        //CmdJoinMessage(username);
        CmdSendMessage(username, "님이 접속하셨습니다", 0);

        buttonSend = GameObject.Find("ButtonSend").GetComponent<Button>();
        buttonSend.onClick.AddListener(delegate { CmdSendMessage(username, inputMessage.text, 1); });

        //Debug.Log("JOIN : " + username);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            CmdSendMessage(username, inputMessage.text, 1);
    }

    [Command]
    public void CmdSendMessage(string username, string message, int type)
    {
        if (isServer)
            RpcSendMessage(username, message, type);

        if (isLocalPlayer)
            inputMessage.text = "";
    }

    [ClientRpc]
    public void RpcSendMessage(string username, string message, int type)
    {
        GameObject msg = Instantiate(ChatManager.instance.message);
        msg.transform.SetParent(ChatManager.instance.contentMessage.transform);

        int childCount = ChatManager.instance.contentMessage.transform.childCount;

        ChatManager.instance.contentMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatManager.instance.contentMessage.GetComponent<RectTransform>().sizeDelta.x, 40 + (childCount * 110));

        msg.transform.GetComponent<RectTransform>().offsetMin = new Vector2(30, 0);

        if (childCount == 1)
            msg.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -20);
        else
            msg.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -20 - (110 * (childCount - 1)));

        if (type == 0)
        {
            if(isLocalPlayer)
                msg.transform.Find("Text").GetComponent<Text>().text = username + "(ME)" + message;
            else
                msg.transform.Find("Text").GetComponent<Text>().text = username + message;

            msg.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            msg.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            msg.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);

            msg.GetComponent<Image>().color = new Color(250f / 255f, 160f / 255f, 230f / 255f, 1f);
        }
        else if (type == 1)
        {
            msg.transform.Find("Text").GetComponent<Text>().text = "ME : " + message;

            if (isLocalPlayer)
            {
                inputMessage.Select();
                inputMessage.ActivateInputField();
            }

            if (!isLocalPlayer)
            {
                msg.transform.Find("Text").GetComponent<Text>().text = message + " : " + username;

                msg.transform.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
                msg.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                msg.transform.GetComponent<RectTransform>().pivot = new Vector2(1, 1);

                msg.transform.GetComponent<RectTransform>().offsetMax = new Vector2(-30, msg.transform.GetComponent<RectTransform>().offsetMax.y);

                msg.GetComponent<Image>().color = new Color(175f / 255f, 98f / 255f, 218f / 255f, 1f);
                msg.transform.Find("Text").GetComponent<Text>().color = Color.white;
            }
        }

        if(isServer)
            NetworkServer.Spawn(msg);
    }
}