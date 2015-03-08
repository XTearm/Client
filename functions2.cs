using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OwnClient;
public class functions2 : MonoBehaviour {
    string user = "jon@live.com", password = "123456";
    Client c = new Client();
    //delete message and add friend and get friends
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    bool show = false;
    string DMessageID = "", FriendItems = "", friendID = "";
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        /////1
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("User name", GUILayout.Width(80)); user = GUILayout.TextArea(user, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Password", GUILayout.Width(80)); password = GUILayout.TextArea(password, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login", GUILayout.Width(140)))
        {
            c.Init(user, password, "127.0.0.1", 3070);
            if (c.Login())show = true;
        }
        if (GUILayout.Button("Logout", GUILayout.Width(140)))
        {
            if (c.Logout())show = false;
        }
        GUILayout.EndHorizontal();
        if (show)
        {
            GUILayout.BeginHorizontal();
            DMessageID = GUILayout.TextArea(DMessageID, GUILayout.Width(120));
            if (GUILayout.Button("Delete Message"))
            {
                if (c.Del(DMessageID))
                    Debug.Log("Delete OK");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Get friends",GUILayout.Width(80)))
            {
                FriendItems = "";
                Queue<FriendClass> friends = new Queue<FriendClass>();
                c.GetFriends(ref friends);
                while (friends.Count > 0)
                {
                    FriendClass f = friends.Dequeue();
                    FriendItems+="UserID:"+f.GetUserID()+"\tType:"+f.GetType_()+"\tLevel"+f.GetLevel()+"\n";
                }
            }
            GUILayout.TextArea(FriendItems, GUILayout.Width(200), GUILayout.Height(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            friendID = GUILayout.TextArea(friendID, GUILayout.Width(120));
            if (GUILayout.Button("Add Friend", GUILayout.Width(100)))
            {
                if (c.AddFriend(friendID))
                    Debug.Log("Add Friend OK");

            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        ////1
        ////2
        GUILayout.BeginVertical();

        GUILayout.EndVertical();
        ////2
        GUILayout.EndHorizontal();
    }
}
