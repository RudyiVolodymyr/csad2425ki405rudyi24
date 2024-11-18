void setup() {
  Serial.begin(9600);  // Початок роботи з серійним портом на швидкості 9600 бод
  Serial.println("Connected to Client.");
}

void loop() {
  if (Serial.available() > 0) {
    String received = Serial.readString();  // Читання повідомлення
    Serial.print("Received: " + received);  // Виводимо отримані дані
    // Модифікація повідомлення, наприклад, конвертація до верхнього регістру
    received.toUpperCase();
    Serial.print("Modified and sent back: " + received); // Виводимо модифіковані дані
  }
}
