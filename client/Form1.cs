using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;

namespace client
{
    public partial class Form1 : Form
    {
        public bool isMonitoring;
        public SerialPort serialPort;
        private System.Timers.Timer portCheckTimer;

        public Form1()
        {
            InitializeComponent();
            LoadAvailablePorts();
            Exchangerate();
            textBox1.Text = "Hello Arduino";

            // Ініціалізуємо таймер
            portCheckTimer = new System.Timers.Timer(2000); // Перевірка кожні 2 секунди
            portCheckTimer.Elapsed += PortCheckTimer_Elapsed;
            portCheckTimer.AutoReset = true;
            portCheckTimer.Start();
        }

        // Метод для перевірки змін у списку портів
        private void PortCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Отримуємо актуальний список портів
            string[] currentPorts = SerialPort.GetPortNames();

            // Виконуємо оновлення UI у головному потоці
            this.Invoke((MethodInvoker)delegate
            {
                // Якщо порти змінилися, оновлюємо ComboBox
                if (!currentPorts.SequenceEqual(comboBox1.Items.Cast<string>()))
                {
                    LoadAvailablePorts();
                }
            });
        }

        // Метод для завантаження доступних COM-портів у ComboBox
        public void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();

            if (ports.Length > 0)
            {
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.Items.Add("no ports");
                comboBox1.SelectedIndex = 0;
            }
        }

        public void Exchangerate()
        {
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("14400");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("115200");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    MessageBox.Show("Already connected to Arduino.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; // Якщо порт вже відкритий, не робимо нічого
                }

                string selectedCom = comboBox1.SelectedItem?.ToString();
                string selectedSpeed = comboBox2.SelectedItem?.ToString();
                int speed = Convert.ToInt32(selectedSpeed);

                serialPort = new SerialPort(selectedCom, speed);
                serialPort.Open();
                richTextBox1.AppendText(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " - Connected to Arduino.\n");

                // Читання даних
                isMonitoring = true; // Починаємо моніторинг
                StartMonitoring(); // Запускаємо моніторинг в окремому потоці
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied to the selected COM port. It might be in use by another application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public async void StartMonitoring()
        {
            await Task.Run(() =>
            {
                while (isMonitoring)
                {
                    try
                    {
                        // Читання даних з порту
                        string data = serialPort.ReadLine();

                        // Оновлення ListBox з основного потоку UI
                        this.Invoke((MethodInvoker)delegate
                        {
                            richTextBox1.AppendText($"{DateTime.Now:dd.MM.yyyy HH:mm:ss -} {data}\n");
                        });
                    }
                    catch (TimeoutException) { /* Ігноруємо тайм-аут, якщо дані не прийшли */ }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        return;
                    }
                }
            });
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {

        }

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Зупиняємо моніторинг і закриваємо порт
            isMonitoring = false;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                MessageBox.Show("No active connection to close.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Зупиняємо моніторинг
            isMonitoring = false;

            // Очікуємо деякий час, щоб завершити всі операції читання перед закриттям
            await Task.Delay(500);

            try
            {
                serialPort.Close();
                richTextBox1.AppendText($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Connection closed.\n"); // Додаємо повідомлення до richTextBox
                MessageBox.Show("Connection closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to close the port: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    richTextBox1.AppendText($"\n");
                    richTextBox1.AppendText($"@@@@@@@@@@@@@@");
                    // Відправка даних
                    string line = textBox1.Text;
                    serialPort.WriteLine(line);
                    richTextBox1.AppendText($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Send: {line}\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending data: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No active connection to send data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
