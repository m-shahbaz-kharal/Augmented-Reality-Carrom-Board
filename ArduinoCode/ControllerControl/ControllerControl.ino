#include <SoftwareSerial.h>
//Debug
bool Debug = true;

//Software Serial Vars
SoftwareSerial WifiModule(12,13);

//Player 1 Vars
const int P1MidPin = 2, P1PnkPin = 3;

//Player 2 Vars
const int P2MidPin = 8, P2PnkPin = 9;

//Control Vars
const int Timeout = 10, SignalStepTime = 100;

void setup() {
  //Player 1 Vars
  pinMode(P1MidPin, INPUT_PULLUP);
  pinMode(P1PnkPin, INPUT_PULLUP);

  //Player 2 Vars
  pinMode(P2MidPin, INPUT_PULLUP);
  pinMode(P2PnkPin, INPUT_PULLUP);

  //Debug Purposes
  Serial.begin(115200);
  //while(!Serial){;}
  
  //Software Serial Setup
  WifiModule.begin(115200);
  //while(!WifiModule){;}

  //Initiating Server
  //SendToWifiModule("AT+RST");
  //SendToWifiModule("AT+CWSAP=\"ControllerNetwork\",\"\",1,0");
  SendToWifiModule("AT+CWMODE=2");
  SendToWifiModule("AT+CIFSR");
  SendToWifiModule("AT+CIPMUX=1");
  SendToWifiModule("AT+CIPSERVER=1,80");
  
  if(Debug) Serial.println("Server should be running by now!");
}

void loop() {
  SendData(GetPlayersData());
  delay(SignalStepTime);
}

String GetPlayersData(){
  String Result = "";
  
  if(digitalRead(P1PnkPin) == LOW) Result += "1";
  else Result += "0";
  Result += ":";
  if(digitalRead(P1MidPin) == LOW) Result += "1";
  else Result += "0";

  Result += ":";
  
  if(digitalRead(P2PnkPin) == LOW) Result += "1";
  else Result += "0";
  Result += ":";
  if(digitalRead(P2MidPin) == LOW) Result += "1";
  else Result += "0";

  Result += "#";

  return Result;
}

void SendData(String Data){
  String Len="";
  Len+=Data.length();
  
  SendToWifiModule("AT+CIPSEND=0,"+Len);
  delay(100);
  SendToWifiModule(Data);
  delay(100);
  //SendToWifiModule("AT+CIPCLOSE=0");
  
  SendToWifiModule("AT+CIPSEND=1,"+Len);
  delay(100);
  SendToWifiModule(Data);
  delay(100);
  //SendToWifiModule("AT+CIPCLOSE=1");

  if(Debug){
    Serial.println(Data);
  }
}

String SendToWifiModule(String Command){  
  WifiModule.println(Command);
  String Result = "";
  long int Time = millis();
  while((Time + Timeout) > millis()){
    while(WifiModule.available()){
      char C = WifiModule.read();
      Result += C;
    }
  }

  if(Debug){
    Serial.println(Result);
  }

  return Result;
}

