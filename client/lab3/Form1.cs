using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO.Ports;
using System.IO;
/// <summary>
/// Namespace for the game client.
/// This namespace contains the core logic of the application, handling user interface, game mode selection, 
/// and interactions with external hardware (if necessary).
/// </summary>

namespace game_client
{
    /// <summary>
     /// Main form for the game client.
     /// Responsible for initializing the game, updating the interface, and handling user interactions with the application.
     /// </summary>
    public partial class Form1 : Form
    {
        public bool IsExitCalled { get; set; } = false;
        public bool StartMenuCalled { get; private set; } = false;
        public bool Player1Called { get; private set; } = false;
        public bool ServerControlCalled { get; private set; } = false;
        public SoundPlayer _player = new SoundPlayer();
        public int select1User;
        public int select2User;
        public bool winStrategy;
        public bool randomMode;
        public bool musicOn;
        public string mode;
        public bool onLoad;
        public int score1;
        public int score2;
        public SerialPort serialPort = new SerialPort();
        public List<int> player1Moves = new List<int>();
        public Timer clickTimer;
        public Random random = new Random();

        /// <summary>
        /// Constructor for the <see cref="Form1"/> class.
        /// Initializes the main game form, sets up window properties, attaches event handlers for custom painting,
        /// and calls the method to load the start menu.
        /// </summary>
        /// <remarks>
        /// This constructor performs the following key tasks:
        /// 1. Initializes the form components through the <see cref="InitializeComponent"/> method.
        /// 2. Loads initial settings and displays the start menu using the <see cref="StartMenu"/> method.
        /// 3. Ensures the form is static, preventing resizing by the user.
        /// 4. Attaches custom paint event handlers for drawing borders on panels.
        /// </remarks>
        /// <code>
        /// public Form1()
        /// {
        ///     // *** 1. Initialize form components ***
        ///     // The InitializeComponent method creates all the visual elements and adds them to the form.
        ///     InitializeComponent();
        ///
        ///     // *** 2. Load the start menu ***
        ///     // The StartMenu method sets up initial interface settings and displays the start menu.
        ///     StartMenu();
        ///
        ///     // *** 3. Configure window properties ***
        ///     // Sets the form style to be non-resizable.
        ///     this.FormBorderStyle = FormBorderStyle.FixedSingle; // Alternative style: FormBorderStyle.Fixed3D.
        ///     
        ///     // Disables the maximize button.
        ///     this.MaximizeBox = false;
        ///
        ///     // *** 4. Attach event handlers for custom panel border painting ***
        ///     // The DrawCustomBorder method handles painting custom borders on panels.
        ///     // Each call adds this method to the Paint event of the corresponding panel.
        ///     panel13.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel14.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel16.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel8.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel12.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel9.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel2.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel3.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel5.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel7.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel17.Paint += new PaintEventHandler(DrawCustomBorder);
        /// }
        /// </code>
        public Form1()
        {
            InitializeComponent();
            StartMenu();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            panel13.Paint += new PaintEventHandler(DrawCustomBorder);
            panel14.Paint += new PaintEventHandler(DrawCustomBorder);
            panel16.Paint += new PaintEventHandler(DrawCustomBorder);
            panel8.Paint += new PaintEventHandler(DrawCustomBorder);
            panel12.Paint += new PaintEventHandler(DrawCustomBorder);
            panel9.Paint += new PaintEventHandler(DrawCustomBorder);
            panel2.Paint += new PaintEventHandler(DrawCustomBorder);
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            panel3.Paint += new PaintEventHandler(DrawCustomBorder);
            panel5.Paint += new PaintEventHandler(DrawCustomBorder);
            panel7.Paint += new PaintEventHandler(DrawCustomBorder);
            panel17.Paint += new PaintEventHandler(DrawCustomBorder);
        }

        /// <summary>
        /// Handles the click event for the "New Game" button.
        /// Opens the mode selection menu and resets the game loading state.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "New Game" button.</param>
        /// <param name="e">Event arguments associated with the button click.</param>
        /// <remarks>
        /// This method performs the following actions:
        /// 1. Calls the <see cref="ModMenu"/> method to open the mode selection menu.
        /// 2. Resets the <see cref="onLoad"/> flag to ensure the game is not in a loading state.
        /// </remarks>
        /// <code>
        /// public void button1_Click(object sender, EventArgs e)
        /// {
        ///     // Opens the mode selection menu.
        ///     ModMenu();
        ///     
        ///     // Resets the loading state.
        ///     onLoad = false;
        /// }
        /// </code>
        public void button1_Click(object sender, EventArgs e) // кнопка New Game
        {
            ModMenu();
            onLoad = false;
        }
        /// <summary>
        /// Loads the menu for selecting game modes.
        /// Adjusts the visibility of buttons, panels, and labels to display the mode selection interface.
        /// </summary>
        /// <remarks>
        /// This method modifies the visibility of several UI elements to prepare the game mode selection menu.
        /// - Hides elements that are not relevant during mode selection.
        /// - Displays elements required for the user to choose a mode.
        /// 
        /// **Key Actions:**
        /// 1. Hides buttons such as `button1`, `button22`, and `button21`.
        /// 2. Hides unnecessary UI components like `panel17` and `pictureBox15`.
        /// 3. Makes essential components like `label2`, `panel1`, and `panel2` visible.
        /// 4. Activates mode selection buttons (`button2`, `button3`, `button4`).
        /// </remarks>
        /// <code>
        /// public void ModMenu()
        /// {
        ///     // Hide irrelevant buttons and UI components.
        ///     button1.Visible = false;
        ///     button22.Visible = false;
        ///     button21.Visible = false;
        ///     panel17.Visible = false;
        ///     pictureBox15.Visible = false;
        ///
        ///     // Display the mode selection interface.
        ///     label2.Visible = true;
        ///     panel1.Visible = true;
        ///     panel2.Visible = true;
        ///     button2.Visible = true;
        ///     button3.Visible = true;
        ///     button4.Visible = true;
        /// }
        /// </code>
        public void ModMenu()
        {
            button1.Visible = false;
            button22.Visible = false;
            button21.Visible = false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            label2.Visible = true;
            panel1.Visible = true;
            panel2.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            
        }

        /// <summary>
        /// Loads the main menu and initializes settings from the configuration file.
        /// Configures the initial state of the game based on saved settings and manages background music.
        /// </summary>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Reads settings from an INI configuration file (`config.ini`) to determine:
        ///    - Background music state.
        ///    - Game-winning strategy mode.
        ///    - Random mode state.
        /// 2. Initializes the background music if enabled.
        /// 3. Hides unnecessary UI elements to display the main menu.
        /// 4. Handles errors during INI file reading gracefully by showing a message box.
        /// 
        /// **Key Elements:**
        /// - Uses the `IniFile` class to manage INI file reading.
        /// - Handles boolean parsing using `bool.TryParse`.
        /// - Utilizes the `SoundPlayer` class for background music playback.
        /// - Updates UI visibility based on the current state.
        /// </remarks>
        /// <code>
        /// public void StartMenu()
        /// {
        ///     // Specify the path to the INI file.
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     try
        ///     {
        ///         // Read values from the INI file as strings.
        ///         string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
        ///         string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
        ///         string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");
        ///
        ///         // Convert string values to booleans using TryParse.
        ///         bool.TryParse(checkBox1Value, out musicOn);
        ///         bool.TryParse(checkBox2Value, out winStrategy);
        ///         bool.TryParse(checkBox3Value, out randomMode);
        ///
        ///         if (musicOn == true)
        ///         {
        ///             _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
        ///             _player.LoadAsync();
        ///             _player.PlayLooping();
        ///         }
        ///         else
        ///         {
        ///             _player?.Stop();
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        ///
        ///     // Hide unnecessary UI elements to display the main menu.
        ///     label2.Visible = false;
        ///     panel1.Visible = false;
        ///     panel2.Visible = false;
        ///     button2.Visible = false;
        ///     button3.Visible = false;
        ///     button4.Visible = false;
        ///     panel3.Visible = false;
        ///     panel8.Visible = false;
        ///     panel13.Visible = false;
        /// }
        /// </code>
        public void StartMenu()
        {
            ServerControlCalled = true;
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {

                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1","");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2","");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3","");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out  winStrategy);
                bool.TryParse(checkBox3Value, out  randomMode);
                if(musicOn==true)
                {
                    
                        _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                        _player.LoadAsync();
                        _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading INI file: " + ex.Message); //  MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);або інше повідомлення для тестів
            }
            label2.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            panel3.Visible = false;
            panel8.Visible = false;
            panel13.Visible = false;
            StartMenuCalled = true;
        }

        /// <summary>
        /// Draws a custom border around a panel.
        /// </summary>
        /// <remarks>
        /// This method customizes the appearance of a `Panel` control by drawing a border around it.
        /// The border is styled using a specific color and thickness defined in the method.
        /// 
        /// **Steps Performed:**
        /// 1. Casts the `sender` object to a `Panel`.
        /// 2. Uses the `Graphics` object from the `PaintEventArgs` to draw the border.
        /// 3. Configures a `Pen` with a predefined color (`Color.Aqua`) and width (`3`).
        /// 4. Draws a rectangle that fits within the panel's dimensions, leaving a one-pixel margin.
        /// 
        /// **Example Usage:**
        /// Attach this method to the `Paint` event of any `Panel`:
        /// ```csharp
        /// panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        /// ```
        /// </remarks>
        /// <param name="sender">The source of the event, expected to be a `Panel` object.</param>
        /// <param name="e">Provides data for the `Paint` event, including the `Graphics` object.</param>
        /// <code>
        /// private void DrawCustomBorder(object sender, PaintEventArgs e)
        /// {
        ///     Panel panel = sender as Panel;
        ///     Graphics g = e.Graphics;
        ///
        ///     // Specify the color and thickness of the border pen.
        ///     Pen pen = new Pen(Color.Aqua, 3);
        ///
        ///     // Draw a rectangular border around the panel.
        ///     g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        /// }
        /// </code>
        public void DrawCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;

            // Задання кольору і товщини пензля для рамки
            Pen pen = new Pen(Color.Gold, 3);

            // Малюємо прямокутну рамку навколо панелі
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }

        /// <summary>
        /// Handles the "Man VS Man" button click event.
        /// </summary>
        /// <remarks>
        /// This method is triggered when the user clicks the "Man VS Man" button.
        /// It sets up the game mode for a two-player experience ("Man VS Man") and initiates Player 1's turn.
        /// 
        /// **Steps Performed:**
        /// 1. Calls the `Player1` method to start the game sequence for Player 1.
        /// 2. Updates the `mode` variable to indicate the current game mode is "Man VS Man".
        /// 
        /// **Example Usage:**
        /// Attach this method to the `Click` event of the "Man VS Man" button:
        /// ```csharp
        /// button3.Click += new EventHandler(button3_Click);
        /// ```
        /// </remarks>
        /// <param name="sender">The source of the event, typically the "Man VS Man" button.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <code>
        /// public void button3_Click(object sender, EventArgs e)
        /// {
        ///     Player1();
        ///     mode = "Man VS Man";
        /// }
        /// </code>
        public void button3_Click(object sender, EventArgs e) // кнопка Man Vs Man
        {
            Player1();
            mode = "Man VS Man";
        }

        /// <summary>
        /// Handles the transition and setup for Player 1's turn in the game.
        /// </summary>
        /// <remarks>
        /// This method manages the visibility of the UI panels and sets up specific behavior depending on the game mode.
        /// If the game mode is "AI VS AI", it initializes a timer to simulate a delayed random AI move.
        /// 
        /// **Steps Performed:**
        /// 1. Hides `panel1` and `panel2`, which are likely for Player 1's initial view.
        /// 2. Displays `panel3`, which is likely for Player 1's actual game view.
        /// 3. If the mode is "AI VS AI", it initializes a timer that triggers a random AI move after 1000 ms:
        ///    - A random number (1 to 3) is generated and a corresponding button (button5, button6, or button7) is clicked programmatically.
        /// 
        /// **Example Usage:**
        /// This method can be called when Player 1's turn begins, especially in a scenario like "AI VS AI".
        /// ```csharp
        /// Player1();
        /// ```
        /// </remarks>
        /// <code>
        /// public virtual void Player1()
        /// {
        ///     panel1.Visible = false;
        ///     panel2.Visible = false;
        ///     panel3.Visible = true;
        ///     if (mode == "AI VS AI")
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             // Generate a random number between 1 and 3
        ///             int buttonNumber = random.Next(1, 4); // Generates 1, 2, or 3
        /// 
        ///             // Simulate a button press based on the random number
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button5.PerformClick(); // Simulate click on button5
        ///                     break;
        ///                 case 2:
        ///                     button6.PerformClick(); // Simulate click on button6
        ///                     break;
        ///                 case 3:
        ///                     button7.PerformClick(); // Simulate click on button7
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// }
        /// </code>
        public virtual void Player1()
        {
            Player1Called = true;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;

            // Отримати кореневу директорію проекту
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            // Побудувати шляхи до зображень відносно кореня проекту
            string imagePath = Path.Combine(projectRoot, "img", "rock.png");
            string imagePath1 = Path.Combine(projectRoot, "img", "paper.png");
            string imagePath2 = Path.Combine(projectRoot, "img", "scissors.png");

            LoadImage(imagePath, pictureBox2);
            LoadImage(imagePath1, pictureBox3);
            LoadImage(imagePath2, pictureBox1);

            if (mode == "AI VS AI")
            {
                // Ініціалізуємо таймер
                clickTimer = new Timer();
                clickTimer.Interval = 1000; // Затримка 500 мс
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop(); // Зупиняємо таймер

                    // Генеруємо випадкове число 1, 2 або 3
                    int buttonNumber = random.Next(1, 4); // 1, 2 або 3

                    // Вибираємо кнопку для натискання
                    switch (buttonNumber)
                    {
                        case 1:
                            button5.PerformClick();
                            break;
                        case 2:
                            button6.PerformClick();
                            break;
                        case 3:
                            button7.PerformClick();
                            break;
                    }
                };
                clickTimer.Start(); // Запускаємо таймер
            }

        }

        /// <summary>
        /// Handles the transition and setup for Player 2's turn in the game.
        /// </summary>
        /// <remarks>
        /// This method manages the behavior of Player 2's turn depending on the game mode:
        /// - If the game mode is "Man VS AI" or "AI VS AI" with random mode enabled, it simulates a random AI move after a 1-second delay.
        /// - If the game mode is "Man VS AI" or "AI VS AI" with a strategy mode enabled, it either calculates a strategic counter-move (if sufficient data is available) or defaults to a random move.
        ///
        /// **Steps Performed:**
        /// 1. Displays `panel8`, which is likely representing Player 2's game view.
        /// 2. Initializes a timer that triggers after 1 second to simulate Player 2's choice:
        ///    - If the mode is `"AI VS AI"` or `"Man VS AI"` with random mode, it generates a random choice for Player 2 (Rock, Paper, or Scissors).
        ///    - If the mode is `"AI VS AI"` or `"Man VS AI"` with strategy mode enabled, it calculates a counter move based on Player 1's previous moves (if sufficient data is available) or defaults to a random move.
        /// 
        /// **Example Usage:**
        /// This method can be called when Player 2's turn begins, especially in a scenario like "Man VS AI" or "AI VS AI".
        /// ```csharp
        /// Player2();
        /// ```
        /// </remarks>
        /// <code>
        /// public void Player2()
        /// {
        ///     panel8.Visible = true; // Display Player 2's game panel
        /// 
        ///     // If the mode is "Man VS AI" or "AI VS AI" with random mode, simulate a random choice
        ///     if (mode == "Man VS AI" || mode == "AI VS AI" && randomMode == true)
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             // Generate a random number between 1 and 3
        ///             int buttonNumber = random.Next(1, 4); // Generates 1 (Rock), 2 (Paper), or 3 (Scissors)
        /// 
        ///             // Simulate a button press based on the random number
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button11.PerformClick(); // Simulate click on button for Rock
        ///                     break;
        ///                 case 2:
        ///                     button12.PerformClick(); // Simulate click on button for Paper
        ///                     break;
        ///                 case 3:
        ///                     button13.PerformClick(); // Simulate click on button for Scissors
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// 
        ///     // If the mode is "Man VS AI" or "AI VS AI" with strategy mode, calculate counter move or random choice
        ///     else if ((mode == "Man VS AI" || mode == "AI VS AI") && winStrategy == true)
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             int buttonNumber;
        /// 
        ///             // If there is enough data, calculate a counter move
        ///             if (winStrategy == true && player1Moves.Count >= 5)
        ///             {
        ///                 buttonNumber = GetCounterMove(); // Get counter move based on Player 1's previous moves
        ///             }
        ///             else
        ///             {
        ///                 // Default to a random choice if not enough data is available
        ///                 buttonNumber = random.Next(1, 4);
        ///             }
        /// 
        ///             // Simulate a button press based on the calculated or random move
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button11.PerformClick(); // Simulate click on button for Rock
        ///                     break;
        ///                 case 2:
        ///                     button12.PerformClick(); // Simulate click on button for Paper
        ///                     break;
        ///                 case 3:
        ///                     button13.PerformClick(); // Simulate click on button for Scissors
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// }
        /// </code>
        public void Player2()
        {
            panel8.Visible = true;

            // Створюємо функції для кожного режиму
            Action randomChoiceAction = () =>
            {
                clickTimer = new Timer();
                clickTimer.Interval = 1000;
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop();
                    int buttonNumber = random.Next(1, 4);
                    SimulateButtonClick(buttonNumber);
                };
                clickTimer.Start();
            };

            Action strategicChoiceAction = () =>
            {
                clickTimer = new Timer();
                clickTimer.Interval = 1000;
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop();
                    int buttonNumber = (player1Moves.Count >= 5) ? GetCounterMove() : random.Next(1, 4);
                    SimulateButtonClick(buttonNumber);
                };
                clickTimer.Start();
            };

            // Словник для зберігання режимів
            var actionDict = new Dictionary<string, Action>
    {
        { "Man VS AI", randomChoiceAction },
        { "AI VS AI", randomChoiceAction },
        { "Man VS AI Strategy", strategicChoiceAction },
        { "AI VS AI Strategy", strategicChoiceAction }
    };

            // Перевірка, чи існує відповідна дія для вибраного режиму
            string actionKey = (mode == "Man VS AI" || mode == "AI VS AI")
                ? (randomMode ? "Man VS AI" : "Man VS AI Strategy")
                : null;

            // Виконати дію, якщо вона визначена
            if (actionKey != null && actionDict.ContainsKey(actionKey))
            {
                actionDict[actionKey].Invoke();
            }
        }

        /// <summary>
        /// Calculates the counter move based on Player 1's previous moves.
        /// Analyzes the last 5 moves and predicts the most likely move to counter it.
        /// </summary>
        /// <returns>
        /// The counter move as an integer:
        /// 1 - Rock,
        /// 2 - Paper,
        /// 3 - Scissors.
        /// </returns>
        /// 
        /// <remarks>
        /// This method calculates the counter move for Player 2 by analyzing the last 5 moves of Player 1.
        /// Each move is assigned a weighted score, with more recent moves receiving higher weights.
        /// The function also introduces a small chance of randomness to make the AI less predictable.
        /// Based on the most likely move of Player 1, the AI will choose a counter move:
        /// - Rock is countered by Paper (2),
        /// - Paper is countered by Scissors (3),
        /// - Scissors is countered by Rock (1).
        /// If the randomness condition is met, the AI will choose a move randomly.
        /// </remarks>
        /// 
        /// @code
        /// public int GetCounterMove()
        /// {
        ///     // Frequency weights for each move
        ///     Dictionary<int, double> moveWeights = new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 } };
        /// 
        ///     // Assign weights to Player 1's moves
        ///     double weight = 1.0;
        ///     double weightIncrement = 0.15; // Increment for weighting moves
        ///     foreach (int move in player1Moves)
        ///     {
        ///         moveWeights[move] += weight;
        ///         weight += weightIncrement;
        ///     }
        /// 
        ///     // Determine the most likely move
        ///     int mostLikelyMove = moveWeights.OrderByDescending(m => m.Value).First().Key;
        /// 
        ///     // Introduce randomness in AI's choice
        ///     double randomness = 0.15; // 15% chance of random move
        ///     if (random.NextDouble() < randomness)
        ///     {
        ///         return random.Next(1, 4); 
        ///     }
        /// 
        ///     // Return the counter move
        ///     switch (mostLikelyMove)
        ///     {
        ///         case 1: // Rock -> Paper
        ///             return 1; 
        ///         case 2: // Paper -> Scissors
        ///             return 2; 
        ///         case 3: // Scissors -> Rock
        ///             return 3; 
        ///         default:
        ///             return random.Next(1, 4); // Default random move
        ///     }
        /// }
        /// @endcode
        public int GetCounterMove()
        {
            // Підрахунок частот ходів із вагами
            Dictionary<int, double> moveWeights = new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 } };

            // Ваговий коефіцієнт - останні ходи отримують більшу вагу
            double weight = 1.0;
            double weightIncrement = 0.15; // Зменшуємо крок, щоб вага зростала більш плавно на 8 ходах
            foreach (int move in player1Moves)
            {
                moveWeights[move] += weight;
                weight += weightIncrement;
            }

            // Знаходимо найбільш ймовірний хід гравця
            int mostLikelyMove = moveWeights.OrderByDescending(m => m.Value).First().Key;

            // Імовірність випадкового ходу
            double randomness = 0.15; // 15% шанс вибрати випадковий хід
            if (random.NextDouble() < randomness)
            {
                return random.Next(1, 4); // Випадковий хід
            }

            // Контрхід: камінь (1) -> папір (2), папір (2) -> ножиці (3), ножиці (3) -> камінь (1)
            switch (mostLikelyMove)
            {
                case 1: // Гравець найчастіше вибирає камінь
                    return 1; // AI вибирає папір
                case 2: // Гравець найчастіше вибирає папір
                    return 2; // AI вибирає ножиці
                case 3: // Гравець найчастіше вибирає ножиці
                    return 3; // AI вибирає камінь
                default:
                    return random.Next(1, 4); // Випадковий вибір на випадок непередбаченої ситуації
            }
        }

        private void SimulateButtonClick(int buttonNumber)
        {
            switch (buttonNumber)
            {
                case 1:
                    button11.PerformClick(); // Rock
                    break;
                case 2:
                    button12.PerformClick(); // Paper
                    break;
                case 3:
                    button13.PerformClick(); // Scissors
                    break;
            }
        }
        /// <summary>
        /// Handles the "Rock" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Rock" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Rock" (represented by the value 1).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button5_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 1; // Set Player 1's move to "Rock"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        public void button5_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 1;
            TrackPlayer1Move(select1User);
        }

        /// <summary>
        /// Handles the "Paper" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Paper" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Paper" (represented by the value 2).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button7_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 2; // Set Player 1's move to "Paper"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        public void button7_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 2;
            TrackPlayer1Move(select1User);
        }

        /// <summary>
        /// Handles the "Scissors" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Scissors" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Scissors" (represented by the value 3).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button6_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 3; // Set Player 1's move to "Scissors"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        public void button6_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 3;
            TrackPlayer1Move(select1User);
        }

        /// <summary>
        /// Tracks the last 5 moves made by Player 1.
        /// Keeps a rolling list of the most recent moves.
        /// </summary>
        /// <param name="move">The move made by Player 1 (1: Rock, 2: Paper, 3: Scissors).</param>
        /// 
        /// <remarks>
        /// This function performs the following tasks:
        /// 1. Adds the new move to the <c>player1Moves</c> list.
        /// 2. If the list exceeds 5 moves, the oldest move is removed to maintain a maximum of 5 recent moves.
        /// This allows the game to track Player 1's most recent moves and potentially use that information for AI decision-making.
        /// </remarks>
        /// 
        /// @code
        /// public void TrackPlayer1Move(int move)
        /// {
        ///     // Add the new move to the list
        ///     player1Moves.Add(move);
        ///     // Remove the oldest move if the list exceeds 5 moves
        ///     if (player1Moves.Count > 5)
        ///     {
        ///         player1Moves.RemoveAt(0); // Remove the first (oldest) move
        ///     }
        /// }
        /// @endcode
        public void TrackPlayer1Move(int move)
        {
            player1Moves.Add(move); // Додаємо новий хід гравця 1
            if (player1Moves.Count > 5)
            {
                player1Moves.RemoveAt(0); // Видаляємо найстаріший хід, щоб зберігати тільки останні 5
            }
        }

        /// <summary>
        /// Handles the "Rock" button click event for Player 2.
        /// Sets the move for Player 2 as "Rock" and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Rock" button is clicked, the following actions are performed:
        /// 1. The move for Player 2 is set to "Rock" (represented by the value 4).
        /// 2. The <c>serverControl()</c> method is called to send Player 2's move along with Player 1's move to the server for further processing.
        /// </remarks>
        /// 
        /// @code
        /// public void button13_Click(object sender, EventArgs e)
        /// {
        ///     // Player 2 chooses "Rock"
        ///     select2User = 4; // Set Player 2's choice to "Rock"
        ///     serverControl(); // Send the player choices to the server and handle the response
        /// }
        /// @endcode
        public void button13_Click(object sender, EventArgs e)
        {
            select2User = 4;
            serverControl();
        }

        /// <summary>
        /// Manages the communication with the server via the serial port.
        /// This method handles the process of sending user choices to the server, receiving responses, and processing those responses.
        /// It retrieves the port name from a configuration file (`config.ini`), opens the serial port, and exchanges data with the server.
        /// If there is an error (such as a timeout or serial port issue), it displays an error message and redirects the user to the main menu.
        /// </summary>
        /// <remarks>
        /// **Steps Performed:**
        /// 1. Reads the serial port name from the configuration file (`config.ini`).
        /// 2. Tries to open the serial port with the specified port name and sets the read timeout to 3 seconds.
        /// 3. Sends a message containing the selected user choices to the server.
        /// 4. Attempts to read the server's responses. If successful, it processes those responses.
        /// 5. If an error occurs (e.g., a timeout), it catches the exception and displays an error message. The user is redirected to the main menu.
        /// 
        /// **Error Handling:**
        /// - A `TimeoutException` is caught if no response is received from the server within the timeout period.
        /// - A general exception is caught if there is an issue opening the serial port or sending/receiving data.
        /// </remarks>
        /// <code>
        /// public void serverControl()
        /// {
        ///     // Initialize the configuration file for reading the port name
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Read port name from config file
        /// 
        ///     try
        ///     {
        ///         // Open the serial port with the specified port name and 9600 baud rate
        ///         using (SerialPort serialPort = new SerialPort(portName, 9600))
        ///         {
        ///             serialPort.ReadTimeout = 3000; // Set 3-second timeout for reading data
        ///             serialPort.Open(); // Open the serial port
        /// 
        ///             // Send user choices to the server
        ///             string message = select1User.ToString() + " " + select2User.ToString();
        ///             serialPort.WriteLine(message); // Send the message
        /// 
        ///             try
        ///             {
        ///                 // Receive responses from the server
        ///                 string response = serialPort.ReadLine(); // First response
        ///                 string counter1 = serialPort.ReadLine(); // Second response
        ///                 string counter2 = serialPort.ReadLine(); // Third response
        /// 
        ///                 // Process the responses
        ///                 FinalAction(response, counter1, counter2);
        ///             }
        ///             catch (TimeoutException)
        ///             {
        ///                 // Handle timeout if no response from the server
        ///                 MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///                 StartMenu(); // Redirect to main menu
        ///                 button1.Visible = true;
        ///                 button21.Visible = true;
        ///                 button22.Visible = true;
        ///                 panel17.Visible = true;
        ///                 pictureBox15.Visible = true;
        ///                 return;
        ///             }
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle general errors (e.g., issues opening the serial port)
        ///         MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///         StartMenu(); // Redirect to main menu
        ///         button1.Visible = true;
        ///         button21.Visible = true;
        ///         button22.Visible = true;
        ///         panel17.Visible = true;
        ///         pictureBox15.Visible = true;
        ///         return;
        ///     }
        /// }
        /// </code>
        public void serverControl()
        {
            ServerControlCalled = true;
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", "");
            try
            {
                using (SerialPort serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // Встановлюємо таймаут читання на 3 секунди
                    serialPort.Open();

                    string message = select1User.ToString() + " " + select2User.ToString();
                    serialPort.WriteLine(message);

                    try
                    {
                        string response = serialPort.ReadLine();
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        FinalAction(response, counter1, counter2);
                    }
                    catch (TimeoutException)
                    {
                        Console.WriteLine("The serial port is inactive or not responding."); // або інше повідомлення для тестів  MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StartMenu(); // Перенаправлення на головне меню
                        button1.Visible = true;
                        button21.Visible = true;
                        button22.Visible = true;
                        panel17.Visible = true;
                        pictureBox15.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message, "Serial Port Error"); // або інше повідомлення для тестів MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartMenu(); // Перенаправлення на головне меню
                button1.Visible = true;
                button21.Visible = true;
                button22.Visible = true;
                panel17.Visible = true;
                pictureBox15.Visible = true;
                return;
            }
        }

        /// <summary>
        /// This method processes the results of the game between Player 1 and Player 2 based on their selections. 
        /// It compares the response from the server with predefined results for ties and wins and displays the corresponding outcome.
        /// The method also updates the game score in the UI if needed.
        /// </summary>
        /// <param name="response">The response received from the server that contains the outcome of the game.</param>
        /// <param name="counter1">The server's response related to Player 1's score.</param>
        /// <param name="counter2">The server's response related to Player 2's score.</param>
        /// <remarks>
        /// **Steps Performed:**
        /// 1. The method compares the `response` with predefined strings to determine if it's a tie or if one player wins.
        /// 2. Based on the outcome, it displays images corresponding to the players' choices in two `PictureBox` controls.
        /// 3. It also displays the result message ("It's a tie!" or "Player X win!") in a `Label` control.
        /// 4. Updates the scores for Player 1 and Player 2 in `textBox1` and `textBox2` respectively.
        /// </remarks>
        /// <code>
        /// public void FinalAction(string response, string counter1, string counter2)
        /// {
        ///     // Define the tie and win conditions
        ///     string tieRock1 = "It's a tie. Player1 and Player2 select rock";
        ///     string tieRock2 = "It's a tie. Player1 and Player2 select paper";
        ///     string tieRock3 = "It's a tie. Player1 and Player2 select scissors";
        /// 
        ///     string WinPlayer1f = "Player1 Win!. Player1 select rock and Player2 select scissors";
        ///     string WinPlayer1s = "Player1 Win!. Player1 select scissors and Player2 select paper";
        ///     string WinPlayer1t = "Player1 Win!. Player1 select paper and Player2 select rock";
        /// 
        ///     string WinPlayer2f = "Player2 Win!. Player1 select scissors and Player2 select rock";
        ///     string WinPlayer2s = "Player2 Win!. Player1 select paper and Player2 select scissors";
        ///     string WinPlayer2t = "Player2 Win!. Player1 select rock and Player2 select paper";
        /// 
        ///     // Define image paths
        ///     string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
        ///     string imagePath = Path.Combine(projectRoot, "img", "rock.png");
        ///     string imagePath1 = Path.Combine(projectRoot, "img", "paper.png");
        ///     string imagePath2 = Path.Combine(projectRoot, "img", "scissors.png");
        /// 
        ///     // Check for tie scenarios
        ///     if (tieRock1 == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     else if (tieRock2 == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     else if (tieRock3 == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     // Check for Player 1 win scenarios
        ///     else if (WinPlayer1f == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer1s == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer1t == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     // Check for Player 2 win scenarios
        ///     else if (WinPlayer2f == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer2s == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer2t == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        /// 
        ///     // Update scores
        ///     if (onLoad == false)
        ///     {
        ///         textBox1.Text = counter1.Substring(14).ToString();
        ///         textBox2.Text = counter2.Substring(14).ToString();
        ///     }
        ///     else
        ///     {
        ///         int line1 = int.Parse(counter1.Substring(14));
        ///         int line2 = int.Parse(counter2.Substring(14));
        ///         textBox1.Text = (line1 + score1).ToString();
        ///         textBox2.Text = (line2 + score2).ToString();
        ///     }
        /// }
        /// </code>
        public void FinalAction(string response, string counter1, string counter2)
        {
            // нічийні варіанті
           
            string tieRock1 = "It's a tie. Player1 and Player2 select rock";
            string tieRock2 = "It's a tie. Player1 and Player2 select paper";
            string tieRock3 = "It's a tie. Player1 and Player2 select scissors";

            // перемога першого гравця програш другого 
            string WinPlayer1f = "Player1 Win!. Player1 select rock and Player2 select scissors";
            string WinPlayer1s = "Player1 Win!. Player1 select scissors and Player2 select paper";
            string WinPlayer1t = "Player1 Win!. Player1 select paper and Player2 select rock";

            // перемога другого гравця програш другого 
            string WinPlayer2f = "Player2 Win!. Player1 select scissors and Player2 select rock";
            string WinPlayer2s = "Player2 Win!. Player1 select paper and Player2 select scissors";
            string WinPlayer2t = "Player2 Win!. Player1 select rock and Player2 select paper";
            
            // Отримати кореневу директорію проекту
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            // Побудувати шляхи до зображень відносно кореня проекту
            string imagePath = Path.Combine(projectRoot, "img", "rock.png");
            string imagePath1 = Path.Combine(projectRoot, "img", "paper.png");
            string imagePath2 = Path.Combine(projectRoot, "img", "scissors.png");

            // нічия якщо два гравці вибрали камінь
            if (tieRock1 == response.Trim())
            {
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали папір
            else if (tieRock2 == response.Trim())
            {
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали ножниці
            else if (tieRock3 == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим каменю, вибір другим ножниць)
            else if (WinPlayer1f == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим ножниці, вибір другим папір)
            else if (WinPlayer1s == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим папір, вибір другим камінь)
            else if (WinPlayer1t == response.Trim())
            { 
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим ножниць, вибір другим каменю)
            else if (WinPlayer2f == response.Trim())
            { 
                // Завантажити зображення у PictureBox
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим папір , вибір другим ножниці)
            else if (WinPlayer2s == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим камінь, вибір другим папір)
            else if (WinPlayer2t == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            if(onLoad == false)
            {
                textBox1.Text = counter1.Substring(14).ToString();
                textBox2.Text = counter2.Substring(14).ToString();
            }
            else
            {
                int line1 = int.Parse(counter1.Substring(14));
                int line2 = int.Parse(counter2.Substring(14));
                textBox1.Text = (line1 + score1).ToString();
                textBox2.Text = (line2 + score2).ToString();
            }
        }

        /// <summary>
        /// Loads an image into a specified PictureBox from a given file path.
        /// </summary>
        /// <remarks>
        /// This function checks if the PictureBox already contains an image. 
        /// If it does, the previous image's resources are released to avoid memory leaks.
        /// Then, the new image is loaded from the specified file path and displayed in the PictureBox.
        /// </remarks>
        /// <param name="imagePath">The file path to the image that needs to be loaded.</param>
        /// <param name="pictureBox">The PictureBox where the image will be displayed.</param>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Thrown if the specified file does not exist.
        /// </exception>
        /// @code
        /// private void LoadImage(string imagePath, PictureBox pictureBox)
        /// {
        ///     // Check if the PictureBox already has an image
        ///     if (pictureBox.Image != null)
        ///     {
        ///         // Release resources of the existing image
        ///         pictureBox.Image.Dispose();
        ///     }
        /// 
        ///     // Load and display the new image
        ///     pictureBox.Image = Image.FromFile(imagePath);
        /// }
        /// @endcode
        public void LoadImage(string imagePath, PictureBox pictureBox)
        {
            // Перевірка на існування зображення
            if (pictureBox.Image != null)
            {
                // Звільняємо ресурси старого зображення
                pictureBox.Image.Dispose();
            }

            // Завантажуємо нове зображення
            pictureBox.Image = Image.FromFile(imagePath);
        }

        /// <summary>
        /// Handles the `FormClosing` event for the main form.
        /// Ensures proper cleanup of resources by closing the serial port if it is open.
        /// </summary>
        /// <param name="sender">The source of the event, typically the form being closed.</param>
        /// <param name="e">Provides data for the `FormClosing` event, including the ability to cancel the closure.</param>
        /// <remarks>
        /// This method prevents potential resource leaks by checking if the serial port is open and closing it 
        /// before the application exits.
        /// </remarks>
        /// @code
        /// public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        /// {
        ///     // Close the serial port if it is open
        ///     if (serialPort.IsOpen)
        ///     {
        ///         serialPort.Close();
        ///     }
        /// }
        /// @endcode
        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закриваємо серійний порт, якщо він відкритий
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {/*
            // Тут можна закрити серійний порт, якщо він відкритий
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            base.OnFormClosing(e);*/
        }

        /// <summary>
        /// Handles the click event for the button corresponding to the "Paper" choice for User 2.
        /// Sets the `select2User` variable to indicate the "Paper" option and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button being clicked.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <remarks>
        /// This method is triggered when the user selects the "Paper" option. 
        /// It assigns a value of `5` to the `select2User` variable, which represents "Paper," 
        /// and calls the `serverControl` method to process the choice and communicate with the server.
        /// </remarks>
        /// @code
        /// private void button11_Click(object sender, EventArgs e)
        /// {
        ///     select2User = 5; // Set the choice for User 2 to "Paper"
        ///     serverControl(); // Call serverControl to handle communication
        /// }
        /// @endcode
        public void button11_Click(object sender, EventArgs e)
        {
            select2User = 5;
            serverControl();
        }

        /// <summary>
        /// Handles the click event for the button corresponding to the "Scissors" choice for User 2.
        /// Sets the `select2User` variable to indicate the "Scissors" option and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button being clicked.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <remarks>
        /// This method is triggered when the user selects the "Scissors" option. 
        /// It assigns a value of `6` to the `select2User` variable, which represents "Scissors," 
        /// and calls the `serverControl` method to process the choice and communicate with the server.
        /// </remarks>
        /// @code
        /// private void button12_Click(object sender, EventArgs e)
        /// {
        ///     select2User = 6; // Set the choice for User 2 to "Scissors"
        ///     serverControl(); // Call serverControl to handle communication
        /// }
        /// @endcode
        public void button12_Click(object sender, EventArgs e)
        {
            select2User = 6;
            serverControl();
        }

        /// <summary>
        /// Event handler for the "Play Again" button. This function hides the game result panels and starts a new game for Player 1 and Player 2.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game result panels, <c>panel13</c> and <c>panel8</c>, are hidden.
        /// 2. The <c>Player1()</c> function is called to start a new game for Player 1 and Player 2.
        /// </remarks>
        /// 
        /// @code
        /// private void button17_Click_1(object sender, EventArgs e)
        /// {
        ///     // Hide the game result panels
        ///     panel13.Visible = false;
        ///     panel8.Visible = false;
        ///
        ///     // Start a new game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        public void button17_Click_1(object sender, EventArgs e)
        {
            panel13.Visible = false;
            panel8.Visible = false;
            Player1();
        }

        /// <summary>
        /// Event handler for the button that redirects to the main menu. This function makes several UI elements visible, including buttons and panels, to display the main menu.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The <c>StartMenu()</c> function is called to navigate to the main menu.
        /// 2. Several UI elements are made visible to display the main menu, including:
        ///    - <c>button1</c>
        ///    - <c>button21</c>
        ///    - <c>button22</c>
        ///    - <c>panel17</c>
        ///    - <c>pictureBox15</c>
        /// </remarks>
        /// 
        /// @code
        /// private void button19_Click(object sender, EventArgs e)
        /// {
        ///     // Navigate to the main menu
        ///     StartMenu();
        ///
        ///     // Make UI elements visible for the main menu
        ///     button1.Visible = true;
        ///     button21.Visible = true;
        ///     button22.Visible = true;
        ///     panel17.Visible = true;
        ///     pictureBox15.Visible = true;
        /// }
        /// @endcode
        public void button19_Click(object sender, EventArgs e)
        {
            StartMenu();
            button1.Visible = true;
            button21.Visible = true;
            button22.Visible = true;
            panel17.Visible = true;
            pictureBox15.Visible = true;
        }

        /// <summary>
        /// Resets the score by communicating with the serial port. This function reads the score from the connected device and updates the UI with the received values.
        /// </summary>
        /// <remarks>
        /// The function performs the following steps:
        /// 1. Reads the serial port name from a configuration file (config.ini).
        /// 2. Opens a serial port with the specified port name and sends a reset command ("0") to the connected device.
        /// 3. Reads the response from the serial port, extracting the score values.
        /// 4. If successful, the score values are displayed in <c>textBox1</c> and <c>textBox2</c>.
        /// 5. If an error occurs (e.g., timeout or other serial port issues), an appropriate error message is shown.
        /// </remarks>
        /// 
        /// @code
        /// private void ResetScore()
        /// {
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Default value: empty string
        ///     try
        ///     {
        ///         using (serialPort = new SerialPort(portName, 9600))
        ///         {
        ///             serialPort.ReadTimeout = 3000; // Set read timeout to 3 seconds
        ///             serialPort.Open(); // Open the serial port
        ///
        ///             serialPort.WriteLine("0"); // Send reset command
        ///
        ///             try
        ///             {
        ///                 // Read responses from the port
        ///                 string counter1 = serialPort.ReadLine();
        ///                 string counter2 = serialPort.ReadLine();
        ///
        ///                 // Process the received data
        ///                 textBox1.Text = counter1.Substring(14);
        ///                 textBox2.Text = counter2.Substring(14);
        ///             }
        ///             catch (TimeoutException)
        ///             {
        ///                 // Handle timeout error if the port doesn't respond
        ///                 MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///             }
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle other exceptions related to the serial port
        ///         MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        /// }
        /// @endcode
        public void ResetScore()
        {
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Значення за замовчуванням: порожнє
            try
            {
                using (serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // Встановлюємо таймаут читання у 3 секунди
                    serialPort.Open(); // Відкриваємо порт

                    serialPort.WriteLine("0"); // Відправляємо команду для скидання

                    try
                    {
                        // Читаємо відповіді від порту
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        // Обробка отриманих даних
                        textBox1.Text = counter1.Substring(14);
                        textBox2.Text = counter2.Substring(14);
                    }
                    catch (TimeoutException)
                    {
                        // Якщо порт не відповідає протягом часу таймауту, показуємо повідомлення і повертаємося до головного меню
                        Console.WriteLine("The serial port is inactive or not responding."); // або інше повідомлення для тестів  MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка інших помилок під час роботи з портом
                Console.WriteLine("Error: " + ex.Message); //MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// Event handler for the button that triggers the score reset. This function calls the <c>ResetScore()</c> method to reset the score.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the <c>ResetScore()</c> method is called, which communicates with the serial port to reset the score and update the UI accordingly.
        /// </remarks>
        /// 
        /// @code
        /// private void button20_Click_1(object sender, EventArgs e)
        /// {
        ///     // Call ResetScore method to reset the score
        ///     ResetScore();
        /// }
        /// @endcode
        public void button20_Click_1(object sender, EventArgs e)
        {
            ResetScore();
        }

        /// <summary>
        /// Event handler for the button that starts a "Man vs AI" game mode. This function sets the game mode to "Man VS AI" and initiates the game by calling the <c>Player1()</c> method.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game mode is set to "Man VS AI" by assigning the string "Man VS AI" to the <c>mode</c> variable.
        /// 2. The <c>Player1()</c> method is called to start the game for Player 1 in the selected mode.
        /// </remarks>
        /// 
        /// @code
        /// private void button2_Click(object sender, EventArgs e)
        /// {
        ///     // Set the game mode to "Man VS AI"
        ///     mode = "Man VS AI";
        ///
        ///     // Start the game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        public void button2_Click(object sender, EventArgs e)
        {
            mode = "Man VS AI";
            Player1();
        }

        /// <summary>
        /// Event handler for the button that starts an "AI vs AI" game mode. This function sets the game mode to "AI VS AI" and initiates the game by calling the <c>Player1()</c> method.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game mode is set to "AI VS AI" by assigning the string "AI VS AI" to the <c>mode</c> variable.
        /// 2. The <c>Player1()</c> method is called to start the game for Player 1 in the selected mode, which involves AI-controlled gameplay.
        /// </remarks>
        /// 
        /// @code
        /// private void button4_Click(object sender, EventArgs e)
        /// {
        ///     // Set the game mode to "AI VS AI"
        ///     mode = "AI VS AI";
        ///
        ///     // Start the game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        public void button4_Click(object sender, EventArgs e)
        {
            mode = "AI VS AI";
            Player1();
        }

        /// <summary>
        /// Event handler for the "Exit" button. This function closes the application when the button is clicked.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the <c>Application.Exit()</c> method is called, which terminates the application and closes all windows.
        /// </remarks>
        /// 
        /// @code
        /// private void button22_Click(object sender, EventArgs e)
        /// {
        ///     // Exit the application
        ///     Application.Exit();
        /// }
        /// @endcode
        public void button22_Click(object sender, EventArgs e)
        {
            IsExitCalled = true;
            Application.Exit();
        }

        /// <summary>
        /// Event handler for the button that opens the game settings form. This function opens the <c>SettingsForm</c> as a modal window and updates game settings based on the values from a configuration file.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The <c>SettingsForm</c> is opened as a modal window, allowing the user to adjust the game settings.
        /// 2. After the settings form is closed, the function reads values from the <c>config.ini</c> file to configure various game settings, such as:
        ///    - <c>CheckBox1</c>: Whether music is enabled.
        ///    - <c>CheckBox2</c>: The player's win strategy.
        ///    - <c>CheckBox3</c>: Whether the game is in random mode.
        /// 3. If music is enabled, the sound file is played; otherwise, the music stops.
        /// </remarks>
        /// 
        /// @code
        /// private void button23_Click(object sender, EventArgs e)
        /// {
        ///     // Open the SettingsForm as a modal window
        ///     SettingsForm settingsForm = new SettingsForm();
        ///     settingsForm.ShowDialog(); // Show the settings form as a dialog (modal)
        ///
        ///     // Specify the path to the ini file
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     try
        ///     {
        ///         // Read values from the ini file as strings
        ///         string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
        ///         string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
        ///         string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");
        ///
        ///         // Convert the string values to boolean using TryParse
        ///         bool.TryParse(checkBox1Value, out musicOn);
        ///         bool.TryParse(checkBox2Value, out winStrategy);
        ///         bool.TryParse(checkBox3Value, out randomMode);
        ///
        ///         // If music is enabled, play the sound; otherwise, stop the music
        ///         if (musicOn == true)
        ///         {
        ///             _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
        ///             _player.LoadAsync();
        ///             _player.PlayLooping();
        ///         }
        ///         else
        ///         {
        ///             _player?.Stop();
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle any errors while reading from the ini file
        ///         MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        /// }
        /// @endcode
        public void button23_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show(); // Відкриваємо як модальне вікно
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {
                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out winStrategy);
                bool.TryParse(checkBox3Value, out randomMode);
                if (musicOn == true)
                {
                    _player.SoundLocation = @"C:\Users\Admin\Downloads\music.wav";
                    _player.LoadAsync();
                    _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            settingsForm.Close();
        }

        /// <summary>
        /// Event handler for the "Save Game" button. This function formats the current game score and opens a <c>SaveMenu</c> to allow the user to save the game.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The current score from <c>textBox1</c> and <c>textBox2</c> is formatted into a string in the format "Player1Score : Player2Score".
        /// 2. A new <c>SaveMenu</c> form is instantiated with the current game mode and score.
        /// 3. The <c>SaveMenu</c> form is displayed to the user, allowing them to save the game.
        /// </remarks>
        /// 
        /// @code
        /// public void button18_Click(object sender, EventArgs e)
        /// {
        ///     // Format the current game score
        ///     string mainScore = textBox1.Text.Replace("\n", "").Replace("\r", "") + " : " +
        ///            textBox2.Text.Replace("\n", "").Replace("\r", "");
        ///
        ///     // Create and show the SaveMenu form with the game mode and score
        ///     SaveMenu saveMenu = new SaveMenu(mode, mainScore);
        ///     saveMenu.Show();
        /// }
        /// @endcode
        public void button18_Click(object sender, EventArgs e)
        {
            string mainScore = textBox1.Text.Replace("\n", "").Replace("\r", "") + " : " +
                   textBox2.Text.Replace("\n", "").Replace("\r", "");

            SaveMenu saveMenu = new SaveMenu(mode, mainScore);
            saveMenu.Show();
        }

        /// <summary>
        /// Event handler for the "Load Game" button. This function opens the <c>LoadForm</c> to allow the user to load a previously saved game.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. A new <c>LoadForm</c> is instantiated, passing the current instance of the form to it.
        /// 2. The <c>LoadForm</c> is shown to the user, allowing them to select and load a saved game.
        /// </remarks>
        /// 
        /// @code
        /// private void button21_Click(object sender, EventArgs e)
        /// {
        ///     // Create and show the LoadForm with the current form instance passed to it
        ///     LoadForm loadForm = new LoadForm(this);
        ///     loadForm.Show();
        /// }
        /// @endcode
        public void button21_Click(object sender, EventArgs e)
        {
            LoadForm loadForm = new LoadForm(this);
            loadForm.Show();
        }

        /// <summary>
        /// Sets the game data by processing the game mode and score, and prepares the game for loading the saved state.
        /// </summary>
        /// <param name="gameMode">The mode of the game (e.g., "Man VS AI", "AI VS AI").</param>
        /// <param name="gameScore">A string representing the game score in the format "Player1Score : Player2Score".</param>
        /// 
        /// <remarks>
        /// This function performs the following tasks:
        /// 1. It splits the provided game score string into two parts, representing the scores of Player 1 and Player 2.
        /// 2. The game mode is stored in the <c>mode</c> variable, and various UI elements are hidden.
        /// 3. If the score format is valid (i.e., contains exactly two parts), the scores are parsed into integers and used for updating the game state.
        /// 4. The <c>ResetScore()</c> function is called to reset the score display, and the <c>Player1()</c> function is called to start the game.
        /// 5. If the score format is invalid, an error message is shown to the user.
        /// </remarks>
        /// 
        /// @code
        /// public void SetGameData(string gameMode, string gameScore)
        /// {
        ///     // Split the score string into two parts
        ///     string[] scores = gameScore.Split(':');
        ///     mode = gameMode;
        ///     button21.Visible = false;
        ///     button22.Visible = false;
        ///     panel17.Visible = false;
        ///     pictureBox15.Visible = false;
        ///     if (scores.Length == 2) // Ensure there are two values
        ///     {
        ///         score1 = int.Parse(scores[0].Trim()); // First score as integer
        ///         score2 = int.Parse(scores[1].Trim()); // Second score as integer
        ///         onLoad = true;
        ///     }
        ///     else
        ///     {
        ///         MessageBox.Show("Invalid score format", "Error");
        ///     }
        ///     ResetScore();
        ///     Player1();
        /// }
        /// @endcode
        public void SetGameData(string gameMode, string gameScore)
        {
            // Розділяємо рядок рахунку на дві частини
            string[] scores = gameScore.Split(':');
            mode = gameMode;
            button21.Visible = false;
            button22.Visible =false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            if (scores.Length == 2) // Переконуємось, що є два значення
            {
                score1 = int.Parse(scores[0].Trim()); // Перше значення рахунку як int
                score2 = int.Parse(scores[1].Trim()); // Друге значення рахунку як int
                onLoad = true;
            }
            else
            {
                Console.WriteLine("Error: Invalid score format"); // закоментовано для збільшення покриття в тестах або інше повідомлення для тестів  MessageBox.Show("Invalid score format", "Error");
                return;
            }
            ResetScore();
            Player1();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public static class TestEnvironment
    {
        public static bool IsTestMode = false; // За замовчуванням false для реального режиму
    }
}
