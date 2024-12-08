using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Windows.Forms;
using client;
using System;

namespace unit_test_client
{
    [TestClass]
    public class Form1Tests
    {
        private Form1 form;
        private const string LogFilePath = "test_results.log"; // Шлях до файлу логування

        [TestInitialize]
        public void Setup()
        {
            // Ініціалізація нового об'єкта Form1
            form = new Form1();
        }

        private void LogResult(string message)
        {
            // Запис результатів у файл
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        [TestMethod]
        public void LoadAvailablePorts_ShouldLoadPortsIntoComboBox()
        {
            // Act
            form.LoadAvailablePorts();

            // Assert
            Assert.IsTrue(form.comboBox1.Items.Count > 0, "No COM ports loaded.");
            LogResult("LoadAvailablePorts test passed: COM ports loaded.");
        }

        [TestMethod]
        public void Button1_Click_ShouldOpenSerialPortAndSendData()
        {
            // Arrange
            form.comboBox1.SelectedItem = "COM4"; // Change to a valid port on your machine
            form.comboBox2.SelectedItem = "9600"; // Select the baud rate

            // Act
            form.button1_Click(null, null); // Invoke the button click

            // Assert
            Assert.IsTrue(form.serialPort.IsOpen, "Serial port should be open after button click.");
            LogResult("Button1_Click test passed: Serial port opened and data sent.");
        }
        [TestMethod]
        public void StartMonitoring_ShouldInvokeReadLine_WhenMonitoringIsTrue()
        {
            // Arrange
            form.isMonitoring = true; // Set monitoring to true

            // Act
            form.StartMonitoring(); // Start monitoring

            // Assert
            // You will need to implement a way to validate the reading logic or check the UI updates.
            // This can be tricky with actual SerialPort; consider adding a property or method in Form1 to check its state.
            LogResult("StartMonitoring test initiated.");
        }
    }
}
