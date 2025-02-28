using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace käynnistin
{
    public partial class ConfirmationDialog : UserControl
    {
        public Action? callback { get; set; }

        // Constructor takes a callback function.
        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        // Invoked when Yes is clicked.
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            callback?.Invoke();
            Hide();
        }

        // Invoked when No is clicked.
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        // Show the overlay by setting its visibility.
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        // Hide the overlay.
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
