/*
  ReadAnalogVoltage
  Reads an analog input on pin 0, converts it to voltage, and prints the result to the serial monitor.
  Graphical representation is available using serial plotter (Tools > Serial Plotter menu)
  Attach the center pin of a potentiometer to pin A0, and the outside pins to +5V and ground.

  This example code is in the public domain.
*/

#include <Servo.h>

Servo myServo;
int pos = 0;
int scale = 1024 / 180;

void setup() {
  Serial.begin(9600);
  myServo.attach(9);
}

// the loop routine runs over and over again forever:
void loop() {
  int sensorValue = analogRead(A0);
  float voltage = sensorValue * (5.0 / 1023.0);
  Serial.println(voltage);
  
  myServo.write(sensorValue / scale);
  delay(5);
  
  //for (pos = 0; pos <= 180; pos += 1) {
  //}
  //for (pos = 180; pos >= 0; pos -= 1) {
  //  myServo.write(pos);
  //  delay(5);
  //}
}
