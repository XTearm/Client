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
namespace OwnClient{
    public class TableClass
    {
        private string id, sour, dest, title, content, reply, time;
        private int type, state;
        public void SetID(string s){
	        id = s;
        }
        public void SetSour(string s){
	        sour = s;
        }
        public void SetDest(string s){
	        dest = s;
        }
        public void SetTitle(string s){
	        title = s;
        }
        public void SetContent(string s){
	        content = s;
        }
        public void SetReply(string s){
            reply = s;
        }
        public void SetType(string s){
            type = int.Parse(s);
        }
        public void SetState(string s){
            state = int.Parse(s);
        }
        public void SetTime(string s){
	        time = s;
        }
        public void SetType(int i){
	        type = i;
        }
        public void SetState(int i){
	        state = i;
        }

        public string GetID(){
	        return id;
        }
        public string GetSour()
        {
	        return sour;
        }
        public string GetDest()
        {
	        return dest;
        }
        public string GetTitle()
        {
	        return title;
        }
        public string GetContent()
        {
	        return content;
        }
        public string GetReply()
        {
	        return reply;
        }
        public int GetType_()
        {
	        return type;
        }
        public int GetState()
        {
	        return state;
        }
        public string GetTime()
        {
            return time;
        }

    }
    public class FriendClass
    {
        private string userID;
        private int type,level;

        public string GetUserID()
        {
            return userID;
        }
        public int GetType_()
        {
            return type;
        }
        public int GetLevel()
        {
            return level;
        }
        public void SetUserID(string u)
        {
            userID = u;
        }
        public void SetType(int t)
        {
            type = t;
        }
        public void SetLevel(int l)
        {
            level = l;
        }
    }
	public class Client
	{
		private string user,password;//保存用户名与密码
		IPEndPoint ipEp;//保存服务端地址与端口
		public Client()
		{
		}
        public void Init(string user_, string password_, string ip, int port)
        {
            user = user_;
            password = password_;
            IPAddress ipAdr = IPAddress.Parse(ip);
            ipEp = new IPEndPoint(ipAdr, port);
        }
		public bool Login()
		{
			string str = "<Su>"+user+"</Su><Ob></Ob><Ve>Login</Ve><Ad>"+password+"</Ad>";//发往服务器的请求，登陆请求

			Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建套接字
			client.Connect(ipEp);//连接服务器
			client.Send (Encoding.Unicode.GetBytes (str));//发送请求到服务器
			string result = "";//保存服务器发回的结果
			byte [] sizebuff = new byte[12];//服务器发回的第一个数据包时int类型，表示结果的数据长度
            client.Receive(sizebuff);//接受第一个数据包，保存在sizebuff
            int size = BitConverter.ToInt32(sizebuff, 0);//把sizebuff转换为int类型

            byte[] strbuff = new byte[size * 2];//用于接收第二个数据包的缓冲，容量必须是第一个数据包受到的容量的两倍
            client.Receive(strbuff, strbuff.Length, SocketFlags.None);//开始收取数据包
            result = Encoding.Unicode.GetString(strbuff);//转换为string类型
            client.Shutdown(SocketShutdown.Both);//关闭套接字
            client.Close();
			if (result.Equals("OK"))//服务器返回OK说明登陆成功
					return true;
			return false;
		}
        public bool Logout()
        {
            string str = "<Su>" + user + "</Su><Ob></Ob><Ve>Logout</Ve><Ad>" + password + "</Ad>#";//登出请求字符串
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff, strbuff.Length, SocketFlags.None);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            if (result.Equals("OK"))
                return true;
            return false;
        }
        public int GetUnreadCount()//获取未读消息量
        {
            string str = "<Su>" + user + "</Su><Ob></Ob><Ve>GetUnreadCount</Ve><Ad> </Ad>";//请求字符串

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size*2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            int conut = int.Parse(result);
            return conut;
        }
        public int GetList(ref Queue <TableClass> qt,int num,int lines)
        {
            string str = "<Su>" + user + "</Su><Ob>" + num + "</Ob><Ve>GetList</Ve><Ad>" + lines + "</Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();

            //解析数据
            Regex r1 = new Regex("<Message>(.*?)</Message>");
            Match m1 = r1.Match(result);
            while (m1.Success)
            {
                Regex r2 = new Regex("<ID>(.*?)<Title>(.*?)<Type>(.*?)<Time>(.*?)</>");
                Match m2 = r2.Match(m1.Groups[1].Value);
                TableClass tc = new TableClass();
                tc.SetID(m2.Groups[1].Value);
                tc.SetTitle(m2.Groups[2].Value);
                tc.SetType(m2.Groups[3].Value);
                tc.SetTime(m2.Groups[4].Value);
                qt.Enqueue(tc);
                m1 = m1.NextMatch();
            }
            return qt.Count;
        }
        public bool GetMessage(string MessageID,ref TableClass tc)
        {
            string str = "<Su>" + user + "</Su><Ob>" + MessageID + "</Ob><Ve>GetMessage</Ve><Ad> </Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            if (result == "No such message")
                return false;
            Regex r1 = new Regex("<Message>MessageID=(.*?)Title=(.*?)Content=([^.]+)Type=(.*?)Time=(.*?)<Message>");
            Match m1 = r1.Match(result);
            Debug.Log(result);
            tc.SetID(m1.Groups[1].Value);
            tc.SetTitle(m1.Groups[2].Value);
            tc.SetContent(m1.Groups[3].Value);
            
            tc.SetType(int.Parse(m1.Groups[4].Value));
            tc.SetTime(m1.Groups[5].Value);
            return true;
        }
        public bool Publish(string recipient,string title,string content)
        {
            string str = "<Su>" + user + "</Su><Ob>" + recipient + "*</Ob><Ve>Publish</Ve><Ad><Title>" + title + "</Title><Content>" + content + "</Content></Ad>";
            string xxs = "asdf;fdsa;asdf;fdsa;";
            Regex rr = new Regex("(.*?);");
            Match mm = rr.Match(xxs);
            mm = mm.NextMatch();

            Debug.Log(mm.Groups[1].Value);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            if (result.Equals("OK"))
                return true;
            Debug.Log(result);
            return false;
        }
        public bool Reply(string MessageID,string content)
        {
            string str = "<Su>" + user + "</Su><Ob>" + MessageID + "</Ob><Ve>Reply</Ve><Ad>="+content+"</Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Debug.Log(result);
            if (result.Equals("OK"))
                return true;
            return false;
        }
        public bool Del(string MessageID)
        {
            string str = "<Su>" + user + "</Su><Ob>" + MessageID + "</Ob><Ve>Del</Ve><Ad> </Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Debug.Log(result);
            if (result.Equals("OK"))
                return true;
            return false;
        }
        public bool AddFriend(string friendID)
        {
            string str = "<Su>" + user + "</Su><Ob>" + friendID + "</Ob><Ve>AddFriend</Ve><Ad>= </Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Debug.Log(result);
            if (result.Equals("OK"))
                return true;
            return false;
        }

        public int GetFriends(ref Queue<FriendClass> friends)
        {
            string str = "<Su>" + user + "</Su><Ob> </Ob><Ve>GetFriends</Ve><Ad> </Ad>";
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEp);
            client.Send(Encoding.Unicode.GetBytes(str));
            string result = "";
            byte[] sizebuff = new byte[12];
            client.Receive(sizebuff);
            int size = BitConverter.ToInt32(sizebuff, 0);

            byte[] strbuff = new byte[size * 2];
            client.Receive(strbuff);
            result = Encoding.Unicode.GetString(strbuff);
            
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Regex r1 = new Regex("<Friends>(.*?)</Friends>");
            Match m1 = r1.Match(result);
            
            while (m1.Success)
            {
                Regex r2 = new Regex("<UserID>(.*?)<type>(.*?)<level>(.*?)</>");
                Match m2 = r2.Match(m1.Groups[1].Value);
                FriendClass tc = new FriendClass();
                tc.SetUserID(m2.Groups[1].Value);
                tc.SetType(int.Parse(m2.Groups[2].Value));
                tc.SetLevel(int.Parse(m2.Groups[3].Value));
                friends.Enqueue(tc);
                m1 = m1.NextMatch();
            }
            return friends.Count;
        }
	}
}

/* 
*/