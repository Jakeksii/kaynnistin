using System.Windows;
using System.Windows.Controls;

namespace käynnistin
{
    public partial class ConfirmationDialog : UserControl
    {
        private Action? _callback;

        // Constructor takes a callback function.
        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        // Invoked when Yes is clicked.
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _callback?.Invoke();
            Hide();
        }

        // Invoked when No is clicked.
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        // Show the overlay by setting its visibility.
        public void Show(Action callback)
        {
            _callback = callback;
            this.Visibility = Visibility.Visible;
        }

        // Hide the overlay.
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
