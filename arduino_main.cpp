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

void setup() {
  Serial.begin(9600);
  myServo.attach(3);
  pinMode(8, OUTPUT);
}

// the loop routine runs over and over again forever:
void loop() {

  if(Serial.available() > 0)
  {
    
    digitalWrite(8,HIGH);
    
    uint8_t c = 0;
    Serial.readBytes(&c, 1);

    while(pos != c)
    {
      if(pos > c)
      {
        pos--;
      }
      else
      {
        pos++;
      }
      myServo.write(pos);
      delay(1);
      //delayMicroseconds(500);
    }
    
    digitalWrite(8,LOW);

    Serial.flush();

    while(Serial.available() > 0)
    {
      Serial.readBytes(&c, 1);
    }
    
  }
}
