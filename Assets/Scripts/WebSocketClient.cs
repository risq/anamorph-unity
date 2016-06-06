using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour {

    SocketIOComponent socket;
    bool registered = false;
    JSONObject JSONSample1;
    JSONObject JSONSample2;
    JSONObject JSONSample3;
    public SilhouetteController silhouetteController;

    void Start () {
        Debug.Log("Start webSocketClient");    
        socket = GetComponent<SocketIOComponent>();

        socket.On("connect", OnConnect);
        socket.On("state", OnState);

        socket.Connect();
    }

    public void OnConnect(SocketIOEvent e)
    {
        Debug.Log("OnConnect");
        Register();
    }

    public void OnDisconnect(SocketIOEvent e)
    {
        Debug.Log("OnDisconnect");
    }

    public void OnClientRegisterStatus(SocketIOEvent e)
    {
        Debug.Log("OnClientRegisterStatus " + e.data.GetField("err"));
        if (e.data.GetField("err") != null)
        {
            OnSucessfulRegister();
        }
    }

    public void OnClientValidConnection(SocketIOEvent e) {
        Debug.Log("OnClientValidConnection");
        Debug.Log(e.data);
    }

    public void OnSucessfulRegister()
    {
        Debug.Log("OnSucessfulRegister");
        registered = true;
        socket.On("samples", OnSamples);
        socket.Emit("samples:get");
    }

    public void OnState(SocketIOEvent e)
    {
        Debug.Log("OnState");
        Debug.Log(e.data["auth"]);
    }

    public void OnSamples(SocketIOEvent e)
    {
        Debug.Log("OnSamples " + e.data);
        JSONSample1 = e.data.GetField("sample1");
        JSONSample2 = e.data.GetField("sample2");
        JSONSample3 = e.data.GetField("sample3");
    }

    public void OnSocialData(SocketIOEvent e) {
        Debug.Log("OnSocialData");
        Debug.Log(e.data);

    }

    public void Register()
    {
        if (!registered)
        {
            Debug.Log("Registering...");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["id"] = "1";
            socket.On("client:register:status", OnClientRegisterStatus);
            socket.Emit("client:register", new JSONObject(data));
            socket.On("socialData", OnSocialData);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) && JSONSample1)
        {
            LoadData(JSONSample1);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && JSONSample2)
        {
            LoadData(JSONSample2);
        }
        else if (Input.GetKey(KeyCode.Alpha3) && JSONSample3)
        {
            LoadData(JSONSample3);
        }
    }

    void LoadData(JSONObject data)
    {
        silhouetteController.UpdateData(data);
    }
}
