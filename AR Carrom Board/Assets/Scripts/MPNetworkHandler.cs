using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Threading;
using Vuforia;
using System.Net;

public class MPNetworkHandler : MonoBehaviour {
	public string ServerIP;
	public int ServerPort;

	public Transform LocalPlayerTranform;
	private string LocalPosX;
	private string LocalRotY;

	public int PingIntervalMillis = 50;
	public bool IsConnected { get; private set; }

	private PlayerStateHolder.PlayerHandEnum opponentPlayerHand = PlayerStateHolder.PlayerHandEnum.NoData;
	public PlayerStateHolder.PlayerHandEnum OpponentPlayerHand {
		get { return opponentPlayerHand; }
		set { opponentPlayerHand = value; }
	}
	public string OpponentPlayerName { get; private set; }

	public float OpponentPosX { get; private set; }
	public float OpponentRotY { get; private set;}
	//TODO: Make Pieces (White to Player 1 and Black to Player 2)

	private TcpListener Server = null;
	private TcpClient Client = null;
	private NetworkStream SockStream;

	private const string ERR_INVALID = "ERR_INVALID";

	private Thread ReceiverThread;

	void Connect(){
		switch (PlayerStateHolder.PlayerType) {
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			if (Server != null) {
				Server.Stop ();
			}
			Server = new TcpListener (IPAddress.Parse (ServerIP), ServerPort);
			Server.Start ();
			while (!Server.Pending ()) {
				Thread.Sleep (1000);
			}
			Client = Server.AcceptTcpClient ();
			SockStream = Client.GetStream ();
			IsConnected = true;
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			if (Client != null) {
				try{
					Client.Close();
				}
				catch (Exception e){
					Debug.Log (e.Message);
				}
			}
			Client = new TcpClient ();
			Client.Connect (ServerIP, ServerPort);
			SockStream = Client.GetStream ();
			IsConnected = true;
			break;
		}
	}

	void Send(string str) {
		if (!IsConnected) {
			return;
		}
		try{
			byte []Data = Encoding.ASCII.GetBytes(str);
			SockStream.Write(Data, 0, Data.Length);
			SockStream.Flush();
		}
		catch (Exception e) {
			Debug.Log (e.Message);
			IsConnected = false;
		}
	}

	string Recv(){
		if (!IsConnected) {
			return ERR_INVALID;
		}
		try {
			byte []Data = new byte[Client.Available];
			SockStream.Read(Data, 0, Data.Length);
			return Encoding.ASCII.GetString (Data);
		}
		catch (Exception e) {
			Debug.Log (e.Message);
			IsConnected = false;
			return ERR_INVALID;
		}
	}

	void SendData(){
		string Response = "";

		//Player Profile Data
		Response += (PlayerStateHolder.PlayerHand == PlayerStateHolder.PlayerHandEnum.LeftHand ? "1" : "2");
		Response += ":";
		Response += PlayerStateHolder.PlayerName;

		Response += ":";

		//Player Positional and Rotational Data
		Response += LocalPosX;
		Response += ":";
		Response += LocalRotY;

		Send(Response);
	}

	void ReceiveData(){
		string Data = Recv ();
		if (Data.Equals (ERR_INVALID)) {
			return;
		}
		try {
			string[] Parts = Data.Split (':');
			switch (Parts [0]) {
			case "1":
				OpponentPlayerHand = PlayerStateHolder.PlayerHandEnum.LeftHand;
				break;
			case "2":
				OpponentPlayerHand = PlayerStateHolder.PlayerHandEnum.RightHand;
				break;
			}
			OpponentPlayerName = Parts [1];
			OpponentPosX = float.Parse (Parts [2]);
			OpponentRotY = float.Parse (Parts [3]);
		} catch (Exception e) {
			Debug.Log (e.Message);
		}
	}

	void Start(){
//		GetComponent<CustomDebug> ().Start ();

		LocalPosX = LocalPlayerTranform.localPosition.x.ToString();
		LocalRotY = LocalPlayerTranform.localRotation.eulerAngles.y.ToString();

		ReceiverThread = new Thread (new ThreadStart (ReceiverThreadFunc));
		ReceiverThread.Start ();
	}


	void Update(){
		LocalPosX = LocalPlayerTranform.localPosition.x.ToString();
		LocalRotY = LocalPlayerTranform.localRotation.eulerAngles.y.ToString();
	}

	void OnApplicationQuit(){
		if (ReceiverThread != null) {
			ReceiverThread.Abort ();
		}
	}

	void ReceiverThreadFunc(){
		while (true) {
			Connect ();
			while (IsConnected) {
				SendData ();
				Thread.Sleep (PingIntervalMillis);
				ReceiveData ();
				Thread.Sleep (PingIntervalMillis);
			}
			Thread.Sleep (PingIntervalMillis * 10);
		}
	}
}
