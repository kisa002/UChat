using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public TextMesh textName;

    [SyncVar]
    public int a = 1;

	// Use this for initialization
	void Start () {
        textName = this.transform.FindChild("TextName").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 20f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 20f;

        transform.Translate(x, 0, z);

        if (Input.GetKeyDown(KeyCode.F))
        {
            CmdAddValue();
        }

        textName.text = a.ToString();

        CmdSetText();
        RpcSetText();
	}

    [Command]
    public void CmdAddValue()
    {
        a += 1;

        Debug.Log(a);
    }

    [Command]
    public void CmdSetText()
    {
        textName.text = a.ToString();
    }

    [ClientRpc]
    public void RpcSetText()
    {
        textName.text = a.ToString();
    }
}