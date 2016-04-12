using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour {

    SocketIOComponent socket;
    bool registered = false;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
       
        socket.On("connect", OnSocketOpen);
        socket.On("state", OnState);
    }

    public void OnSocketOpen(SocketIOEvent e)
    {
        Register();
    }

    public void OnClientRegisterStatus(SocketIOEvent e)
    {
        Debug.Log("OnClientRegisterStatus");
        if (!e.data.GetField("err"))
        {
            OnSucessfulRegister();
        }
    }

    private void OnSucessfulRegister()
    {
        Debug.Log("OnSucessfulRegister");
        registered = true;
    }

    private void OnState(SocketIOEvent e)
    {
        Debug.Log("OnState");
        Debug.Log(e.data);
    }

    private void Register()
    {
        if (!registered)
        {
            Debug.Log("Registering...");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["id"] = "12";
            socket.On("client:register:status", OnClientRegisterStatus);
            socket.Emit("client:register", new JSONObject(data));
        }
    }
 

    // Update is called once per frame
    void Update () {
	
	}
}
