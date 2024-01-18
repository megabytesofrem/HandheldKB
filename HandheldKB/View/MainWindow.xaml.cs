using System.Windows;
using System.Windows.Controls;
using SharpDX.XInput;

namespace HandheldKB.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller? _controller;
        private Grid? _keyboardGrid;

        // The index of the currently selected key (-1, if none selected)
        private int _selectedKeyIndex = -1;

        public Controller Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new Controller(UserIndex.One);
                }

                return _controller;
            }

            set => _controller = value;
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _keyboardGrid = FindName("KeyboardGrid") as Grid;
        }

        private Grid? FindKeyboardRow(int row)
        {
            if (_keyboardGrid == null)
                return null;

            // Find the row that matches the row number
            for (int i = 0; i < _keyboardGrid.Children.Count; i++)
            {
                var child = _keyboardGrid.Children[i];
                if (child is Grid grid)
                {
                    if (Grid.GetRow(grid) == row)
                        return grid;
                }
            }

            return null;
        }


        // Controller polling code

        private async void OnTimerTick(object sender, EventArgs e)
        {
            // Asynchronously poll controller input
            await PollControllerInput();
        }

        private async Task PollControllerInput()
        {
            if (!Controller.IsConnected)
            {
                return;
            }

            var state = Controller.GetState().Gamepad;
            switch (state.Buttons)
            {
                case GamepadButtonFlags.DPadLeft:
                    break;
                case GamepadButtonFlags.DPadRight:
                    break;
            }
        }
    }
}
