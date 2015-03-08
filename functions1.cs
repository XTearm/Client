using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using OwnClient;
public class functions1 : MonoBehaviour {
    string user = "jon@live.com", password = "123456";
	// Use this for initialization
    Client c = new Client();
	void Start () 
    {
        c.Init("jon@live.com", "123456", "127.0.0.1", 3070);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    bool show = false;
    string title = "", content = "", recipient = "",unread="";
    string messages = "", message = "", MessageID = "", RMessageID = "", RContent = "";
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("User name", GUILayout.Width(80)); user = GUILayout.TextArea(user, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Password", GUILayout.Width(80)); password = GUILayout.TextArea(password, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login",GUILayout.Width(140))) {
            c.Init(user, password, "127.0.0.1", 3070);
            if (c.Login()){
                show = true;
            }
        }
        if (GUILayout.Button("Logout", GUILayout.Width(140)))
        {
            if (c.Logout()){
                show = false;
            }
        }
        GUILayout.EndHorizontal();
        if (show)
        {
            
            GUILayout.Label("Publish menu");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Title:");title=  GUILayout.TextArea(title, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Content:"); content = GUILayout.TextArea(content, GUILayout.Width(200),GUILayout.Height(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Recipient:"); recipient = GUILayout.TextArea(recipient, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Send"))
            {
                if (c.Publish(recipient, title, content))
                {
                    Debug.Log("has been sent");
                }
            }

            if (GUILayout.RepeatButton("Get unread count"))
                unread = c.GetUnreadCount()+"";
            GUILayout.BeginHorizontal();
            GUILayout.Label("Unread count:"); unread = GUILayout.TextArea(unread, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Message list");
            GUILayout.BeginHorizontal();
             messages = GUILayout.TextArea(messages, GUILayout.Width(350),GUILayout.Height(100));
             if (GUILayout.Button("Get messages", GUILayout.Width(120)))
            {
                messages = "";
                Queue<TableClass> qt = new Queue<TableClass>();
                c.GetList(ref qt, 0, 10);
                while(qt.Count>0)
                {
                    TableClass tc = qt.Dequeue();
                    messages += "Title:"+tc.GetTitle()+ "\t\tTime:" + tc.GetTime()+"S Or R:\t\t"+tc.GetType_()+"\tMessageID:"+tc.GetID();
                    messages += "\n";
                }

            }
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            GUILayout.Label("Message"); //message = GUILayout.TextArea(message, GUILayout.Width(400), GUILayout.Height(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Get Message", GUILayout.Width(120)))
            {
                message = "";
                TableClass t = new TableClass();
                c.GetMessage(MessageID, ref t);
                message += "MessageID:" + t.GetID() + "\tTitle:" + t.GetTitle() + "\tContent:" + t.GetContent() + "\tTime:" + t.GetTime();
            }
            GUILayout.Label("MessageID:", GUILayout.Width(80)); MessageID = GUILayout.TextArea(MessageID, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            GUILayout.TextField(message,GUILayout.Width(500),GUILayout.Height(200));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Reply");
            GUILayout.BeginHorizontal();
            GUILayout.Label("MessageID"); RMessageID = GUILayout.TextField(RMessageID, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            RContent =  GUILayout.TextArea(RContent, GUILayout.Width(300), GUILayout.Height(200));
            if (GUILayout.Button("Reply"))
            {
                if (c.Reply(RMessageID, RContent))
                {
                    Debug.Log("Reply OK");
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }
    }
}
