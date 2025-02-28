using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
            Script script1 = new("id", "title", "description", "data");
            scripts.Add(script1);
            // scripts = Storage.Load();
            // bind collection to ItemsControl
            ScriptContainer.ItemsSource = scripts;
        }

        private void AddButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                // REPLACE THIS WITH edit / new modal
                Script script1 = new("id", "title", "description", "data");
                scripts.Add(script1);
                Storage.Save(scripts); // Save collection
            }
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

                ShowConfirmationDialog(callback);
            }
        }

        private void EditButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Script script)
            {

                // OPEN EDIT / NEW MODAL AS EDIT

                // REMOVE THESE TEST LINES
                var updateInfo = new ScriptUpdateInfo
                {
                    Id = "newId",
                    Title = "New Title",
                    Description = "Updated description",
                    Data = "New powershell command"
                };
                Console.WriteLine(updateInfo);

                script.Update(updateInfo);
            }
        }

        private void ScriptCard_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border btn && btn.DataContext is Script script)
            {
                script.Launch();
            }
        }

        private void ShowConfirmationDialog(Action callback)
        {
            ConfirmationDialogControl.callback = callback;
            ConfirmationDialogControl.Show();
        } 
    }
}