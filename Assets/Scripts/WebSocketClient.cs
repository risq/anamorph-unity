using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour {

    SocketIOComponent socket;
    bool registered = false;
    JSONObject JSONSample;
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

        //here we get the urls to connect social networks
        //string facebookUrl = e.data["auth"]["facebookUrl"].ToString();
        //string twitterUrl = e.data["auth"]["twitterUrl"].ToString();
        //string linkedinUrl = e.data["auth"]["linkedinUrl"].ToString();
        //string instagramUrl = e.data["auth"]["instagramUrl"].ToString();

        //Application.OpenURL(e.data["auth"]["facebookUrl"].ToString()); //Open facebook url link and set datas in server

        //Valid social connections and retrieve all the datas
        //Application.OpenURL(e.data["auth"]["rootUrl"].ToString().Trim('"') + "/validConnections?clientId=12");
    }

    public void OnSamples(SocketIOEvent e)
    {
        Debug.Log("OnSamples");
        JSONSample = e.data;
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


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 300, 150, 50), "Load sample JSON") && JSONSample)
        {
            LoadData(JSONSample);
        }
    }

    void LoadData(JSONObject data)
    {
        silhouetteController.UpdateData(data);
    }
}
