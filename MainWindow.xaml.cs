using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace käynnistin
{
    public partial class MainWindow : Window
    {
        // declare collection
        private readonly ObservableCollection<Script> scripts = [];
        public MainWindow()
        {
            InitializeComponent();

            // initialize collection
            scripts = Storage.Load();

            // bind collection to ItemsControl
            ScriptContainer.ItemsSource = scripts;
        }

        private void AddButton_Click(Object sender, RoutedEventArgs e)
        {
            // Open AddEditScriptDialog as New
            AddEditScriptDialogControl.Show(null, scripts);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Script script)
            {
                void callback()
                {
                    scripts.Remove(script); // Remove from collection
                    Storage.Save(scripts); // Save collection
                }

                ConfirmationDialogControl.Show(callback);
            }
        }

        private void EditButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Script script)
            {

                // Open AddEditScriptDialog as Edit
                AddEditScriptDialogControl.Show(script, scripts);
            }
        }

        private void ScriptCard_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border btn && btn.DataContext is Script script)
            {
                script.Launch();
            }
        }
    }
}