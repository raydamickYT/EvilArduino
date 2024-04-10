int ledPin = 12;
int sensorPin = 4;
int sensorValue;
int lastTiltState = HIGH; // the previous reading from the tilt sensor
const int butPin1 = 6;
const int butPin2 = 7;
bool canPerformActions = false;

// the following variables are long's because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long lastDebounceTime = 0; // the last time the output pin was toggled
long debounceDelay = 50; // the debounce time; increase if the output flickers

void setup() {
  Serial.begin(9600);

  pinMode(sensorPin, INPUT);
  pinMode(ledPin, OUTPUT);
  pinMode(butPin1, INPUT);

  digitalWrite(sensorPin, HIGH);
  digitalWrite(butPin1, HIGH);
}

void loop() {
  if (digitalRead(butPin1) == LOW) {
    canPerformActions = !canPerformActions;
    Serial.println("can perform");
    Serial.write(1);
  }
  if (canPerformActions) {

    sensorValue = digitalRead(sensorPin);

    // If the switch changed, due to noise or pressing:
    if (sensorValue == lastTiltState) {
      // reset the debouncing timer
      lastDebounceTime = millis();
    }
    if ((millis() - lastDebounceTime) > debounceDelay) {
      // whatever the reading is at, it's been there for longer
      // than the debounce delay, so take it as the actual current state:
      lastTiltState = sensorValue;
      Serial.println(sensorValue);
    }
    if (lastTiltState == 1) {
      Serial.write(3);
      Serial.flush();
    }
    digitalWrite(ledPin, lastTiltState);
  }
  else {
    Serial.println("Actions Are Locked");
  }

  delay(500);
}
