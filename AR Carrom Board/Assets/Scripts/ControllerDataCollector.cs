using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text; 
using System.Threading;

public class ControllerDataCollector : MonoBehaviour {
	public bool Debugging = true;

	#region ControllerDataCollection
	public enum ControllerAimStateEnum { Unknown, Idle, PrepareAim, Aim }

	public string ServerIP = "192.168.4.1";
	public int ServerPort = 80;

	public ControllerAimStateEnum Player1ControllerState { get; private set; }
	public ControllerAimStateEnum Player2ControllerState { get; private set; }
	public Int32 Player1AccX, Player1AccY, Player1AccZ;
	public Int32 Player1GyrX, Player1GyrY, Player1GyrZ;
	public Int32 Player2AccX, Player2AccY, Player2AccZ;
	public Int32 Player2GyrX, Player2GyrY, Player2GyrZ;

	private Thread ControllerDataCollectorThread;

	public void RunControllerDataCollectorThread(){
		try { ControllerDataCollectorThread.Abort(); } catch{}
		ControllerDataCollectorThread = new Thread(new ThreadStart(ControllerDataCollectorMethod));
		ControllerDataCollectorThread.Start ();
	}

	private void ControllerDataCollectorMethod(){
		try{
			TcpClient Socket = new TcpClient ();
			Socket.Connect(ServerIP, ServerPort);
			NetworkStream SocketStream = Socket.GetStream();
			while(true){
				byte []Data = new byte[Socket.Available];
				SocketStream.Read(Data, 0, Data.Length);
				string RawData = Encoding.ASCII.GetString(Data);
				try{
					String []Chunks = RawData.Split('#');
					foreach(String Chunk in Chunks){
						String []PData = Chunk.Split('|');
						String []P1Vars = PData[0].Split(':');
						String []P2Vars = PData[1].Split(':');

						if(P1Vars[0].Equals("0") && P1Vars[1].Equals("0")) Player1ControllerState = ControllerAimStateEnum.Idle;
						else if(P1Vars[0].Equals("1") && P1Vars[1].Equals("0")) Player1ControllerState = ControllerAimStateEnum.PrepareAim;
						else if(P1Vars[0].Equals("0") && P1Vars[1].Equals("1")) Player1ControllerState = ControllerAimStateEnum.Aim;
						else Player1ControllerState = ControllerAimStateEnum.Aim;
						Player1AccX = Int32.Parse(P1Vars[2]);
						Player1AccY = Int32.Parse(P1Vars[3]);
						Player1AccZ = Int32.Parse(P1Vars[4]);
						Player1GyrX = Int32.Parse(P1Vars[5]);
						Player1GyrY = Int32.Parse(P1Vars[6]);
						Player1GyrZ = Int32.Parse(P1Vars[7]);

						if(P2Vars[0].Equals("0") && P2Vars[1].Equals("0")) Player2ControllerState = ControllerAimStateEnum.Idle;
						else if(P2Vars[0].Equals("1") && P2Vars[1].Equals("0")) Player2ControllerState = ControllerAimStateEnum.PrepareAim;
						else if(P2Vars[0].Equals("0") && P2Vars[1].Equals("1")) Player2ControllerState = ControllerAimStateEnum.Aim;
						else Player2ControllerState = ControllerAimStateEnum.Aim;
						Player2AccX = Int32.Parse(P2Vars[2]);
						Player2AccY = Int32.Parse(P2Vars[3]);
						Player2AccZ = Int32.Parse(P2Vars[4]);
						Player2GyrX = Int32.Parse(P2Vars[5]);
						Player2GyrY = Int32.Parse(P2Vars[6]);
						Player2GyrZ = Int32.Parse(P2Vars[7]);

						if(Debugging){
							Debug.Log("Player 1 Controller State: "+Player1ControllerState.ToString());
							Debug.Log("Player 2 Controller State: "+Player2ControllerState.ToString());
						}
					}
				}
				catch (Exception e) {
					Debug.Log(e.Message);
				}
			}
		}
		catch (Exception e){
			Debug.Log (e.Message);
			try {
				ControllerDataCollectorThread.Abort (); 
			}
			catch {}
		}
	}
	#endregion

	void Start(){
		RunControllerDataCollectorThread ();
	}

	void OnApplicationQuit(){
		try { ControllerDataCollectorThread.Abort(); } catch{}
	}
}