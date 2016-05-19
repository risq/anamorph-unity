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

    private void OnSucessfulRegister()
    {
        Debug.Log("OnSucessfulRegister");
        registered = true;
    }

    private void OnState(SocketIOEvent e)
    {
        Debug.Log("OnState");
        Debug.Log(e.data["auth"]);

        //here we get the urls to connect social networks
        string facebookUrl = e.data["auth"]["facebookUrl"].ToString();
        string twitterUrl = e.data["auth"]["twitterUrl"].ToString();
        string linkedinUrl = e.data["auth"]["linkedinUrl"].ToString();
        string instagramUrl = e.data["auth"]["instagramUrl"].ToString();

        //Application.OpenURL(e.data["auth"]["facebookUrl"].ToString()); //Open facebook url link and set datas in server

        //Valid social connections and retrieve all the datas
        //Application.OpenURL(e.data["auth"]["rootUrl"].ToString().Trim('"') + "/validConnections?clientId=12");
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
