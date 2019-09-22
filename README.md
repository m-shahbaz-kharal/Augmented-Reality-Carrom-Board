# Augmented-Reality-Carrom-Board
“Augmented Reality” sometimes also called “Mixed Reality” is the new way to experience
virtual objects in real world. It’s like seeing virtual things blended with the real world,
through a phone camera and it is so beautiful that you feel like you united both the computer
world and your real world in an almost seamless way.
“Augmented Carrom Board” will use the same technology to make you feel like more closer
to how you played it on real boards. It’ll scan the environment, project a board to it (visible
through a phone screen), set up the game for you, you’ll wear a specialized controller and
there you go place your finger (yes your real hand finger) close to the striker on the board, aim
like the way you do and flick when ready, the game will capture your hand movements, your aim
and game board along with the pieces will react like the real game and that is our imagination
to make the play more real.

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
For making a physical glove controller, here's what you'll need:
* 2 pairs of cloth gloves
* Conductive thread (sew around middle finger, little finger, palm of your hand and connect these according to pin connections) 
* 2 MPU6050 ([this one](https://www.sparkfun.com/products/11028))
* Arduino UNO R3
* ESP8266 Wifi Module ([this one](https://www.sparkfun.com/products/13678))

#### Pin Connections
**Arduino -> ESP8266**
* 12 -> TX
* 13 -> RX
* 3.3v -> Vcc and EN
* GND -> GND

**Arduino -> Player 1 Glove**
* 2 -> Middle Finger of Glove
* 3 -> Little Finger of Glove
