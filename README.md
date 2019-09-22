# Augmented-Reality-Carrom-Board
“Augmented Reality” sometimes also called “Mixed Reality” is the new way to experience
virtual objects in the real world. It’s like seeing virtual things blended with the real world,
through a camera and it is so beautiful that you feel like you mixed both the computer
world and your real world in an almost seamless way.
“Augmented Carrom Board” will use the same technology to make you feel closer
to how you played Carrom on real board. It’ll scan the environment, project a board to it (visible
through an Android phone screen), set up the game, you’ll wear a specialized controller and
there you go, place your finger (yes your real hand finger) close to the striker on the board (try to feel it :D), aim
like the way you do and flick when ready, the game will capture your hand movements, your aim
and game board along with the pieces will react like the real game. Weww!!!
## Demo
Here is the [Youtube Link](https://www.youtube.com/watch?v=CXEz-am8Rn8) to see the demo. The demo video shows the glove has an image tracker to track hand, but we updated the project to use an MPU6050 for motion sensing. I'll upload the new video showing the updated setup soon.

## How to run it
There are two folders:
* AR Carrom Board (Unity Project)
* Arduino Code (Physical glove controller code).

### Unity Project
To directly run android apk, just go to **APKs** dir and copy Android Pacakge File (APK) to Android 4.0 or higher.
To make changes according to your need open **AR Carrom Board** in Unity (2017.3.1f1 or Higher). Play with it. Build it. Run it.

### Physical Glove Controller
**Arduino Code** directory contains the code to be uploaded to Arduino UNO.

For making a physical glove controller, here's what you'll need:
* 2 pairs of cloth gloves
* Conductive thread (sew around middle finger, little finger, palm of your hand and connect these according to [Pin Connections](https://github.com/m-shahbaz-kharal/Augmented-Reality-Carrom-Board/blob/master/README.md#pin-connections)) 
* 2 MPU6050 ([this one](https://www.sparkfun.com/products/11028))
* Arduino UNO R3
* ESP8266 Wifi Module ([this one](https://www.sparkfun.com/products/13678))

#### Pin Connections
* **Arduino -> ESP8266**
  * 12 -> TX
  * 13 -> RX
  * 3.3v -> Vcc and EN
  * GND -> GND

* **Arduino -> Player 1 Glove**
  * 2 -> Middle Finger of Glove
  * 3 -> Little Finger of Glove
* **Arduino -> Player 1 MPU6050**
  * 3.3 -> Vcc
  * GND -> GND
  * A4 -> AD0
  * A5 -> SCL

* **Arduino -> Player 2 Glove**
  * 8 -> Middle Finger of Glove
  * 9 -> Little Finger of Glove
* **Arduino -> Player 2 MPU6050**
  * 3.3 -> Vcc
  * GND -> GND
  * A4 -> SDA
  * A5 -> SCL

#### Running it all
After installing the APK to your Android device, uploading Arduino code to Arduino UNO and making these connections between Arduino and your gloves, follow these steps:
1. Power up Arduino.
2. On your Android device you should see a new Wifi network named **ControllerNetwork**. Connect to it.
3. Run the game.
4. Enjoy.

## Developers
**Muhammad Shahbaz Kharal**
**Muhammad Luqman**
**Ahmad Hassan**
