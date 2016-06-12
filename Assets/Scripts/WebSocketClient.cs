using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour {

    SocketIOComponent socket;
    bool registered = false;
    JSONObject CurrentData;
    JSONObject JSONSample1;
    JSONObject JSONSample2;
    JSONObject JSONSample3;
    public SilhouetteController silhouetteController;

    GUIManager guiManager;

    public GameObject NotConnectedLabel;

    void Start () {
        NotConnectedLabel.SetActive(true);

        guiManager = GameObject.FindObjectOfType<GUIManager>();
        socket = GetComponent<SocketIOComponent>();
        
        socket.On("connect", OnConnect);
        socket.On("state", OnState);
        socket.On("client:register:status", OnClientRegisterStatus);
        socket.On("socialData", OnSocialData);
        socket.On("remoteRegistered", OnRemoteRegistered);

        socket.Connect();
    }

    public void OnConnect(SocketIOEvent e)
    {
        Debug.Log("OnConnect");
        Register();
    }

    public void OnTest(SocketIOEvent e)
    {
        Debug.Log("ONTEST");
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

    public void OnRemoteRegistered(SocketIOEvent e)
    {
        Debug.Log("OnRemoteRegistered");
        guiManager.OnRemoteRegistered();
    }

    public void OnClientValidConnection(SocketIOEvent e) {
        Debug.Log("OnClientValidConnection");
        Debug.Log(e.data);
    }

    public void OnSucessfulRegister()
    {
        Debug.Log("OnSucessfulRegister");
        registered = true;
        NotConnectedLabel.SetActive(false);
        socket.On("samples", OnSamples);
        socket.Emit("samples:get");
    }

    public void OnState(SocketIOEvent e)
    {
        Debug.Log("OnState");
        Debug.Log(e.data);
    }

    public void OnSamples(SocketIOEvent e)
    {
        Debug.Log("OnSamples " + e.data);
        JSONSample1 = e.data.GetField("sample1");
        JSONSample2 = e.data.GetField("sample2");
        JSONSample3 = e.data.GetField("sample3");

        LoadData(JSONSample1);
    }

    public void OnSocialData(SocketIOEvent e) {
        Debug.Log("OnSocialData " + e.data);
        CurrentData = e.data;
        LoadData(CurrentData);
        guiManager.OnDataLoaded();
    }

    public void Register()
    {
        if (!registered)
        {
            Debug.Log("Registering...");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["id"] = "1";
            socket.Emit("client:register", new JSONObject(data));
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0) && CurrentData)
        {
            LoadData(CurrentData);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1) && JSONSample1)
        {
            LoadData(JSONSample1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && JSONSample2)
        {
            LoadData(JSONSample2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && JSONSample3)
        {
            LoadData(JSONSample3);
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            Register();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            LoadData(null);
        }
    }

    void LoadData(JSONObject data)
    {
        silhouetteController.UpdateData(data);
    }
}
