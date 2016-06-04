using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour {

    SocketIOComponent socket;
    bool registered = false;

    // Use this for initialization
    void Start () {
        Debug.Log("Start webSocketClient");    
        socket = GetComponent<SocketIOComponent>();
       
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

    public void OnClientValidConnection(SocketIOEvent e) {
        Debug.Log("OnClientValidConnection");
        Debug.Log(e.data);
    }

    private void OnSucessfulRegister()
    {
        Debug.Log("OnSucessfulRegister");
        registered = true;
    }

    private void OnState(SocketIOEvent e)
    {
        Debug.Log("OnState");
        Debug.Log(e.data["auth"]);
    }

    private void OnSocialData(SocketIOEvent e) {
        Debug.Log("OnSocialData");
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
            socket.On("socialData", OnSocialData);
        }
    }
 

    // Update is called once per frame
    void Update () {
	
	}
}
