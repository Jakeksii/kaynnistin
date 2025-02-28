using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace käynnistin
{
    public partial class AddEditScriptDialog : UserControl
    {
        public ScriptViewModel ViewModel { get; set; }
        private ObservableCollection<Script>? _scripts;
        private Script? _script;

        public AddEditScriptDialog()
        {
            InitializeComponent();
            ViewModel = new ScriptViewModel();
            this.DataContext = ViewModel;
        }

        internal void Show(Script? script, ObservableCollection<Script> scripts)
        {
            if (script == null) 
            { 
                ViewModel.Title = "";
                ViewModel.Description = "";
                ViewModel.Arguments = "-ExecutionPolicy Bypass -NoExit -NoProfile";
                ViewModel.RunAsAdmin = false;
                ViewModel.WorkingDirectory = "";
                ViewModel.ScriptPath = "";
                ViewModel.Script = "";
                ViewModel.Mode = "New";
            } else
            {
                ViewModel.Title = script.Title;
                ViewModel.Description = script.Description;
                ViewModel.Arguments = script.Arguments;
                ViewModel.RunAsAdmin = script.RunAsAdmin;
                ViewModel.WorkingDirectory = script.WorkingDirectory;
                ViewModel.ScriptPath = script.ScriptPath;
                ViewModel.Script = script.Data;
                ViewModel.Mode = "Edit";
            }
            _scripts = scripts;
            _script = script;
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_scripts == null) return;

            // Create or update a script based on mode
            if (ViewModel.Mode == "New")
            {
                var newScript = new Script(ViewModel.Title, ViewModel.Description, ViewModel.Arguments, ViewModel.RunAsAdmin, ViewModel.WorkingDirectory, ViewModel.ScriptPath, ViewModel.Script);
                _scripts.Add(newScript);
            }
            else if (ViewModel.Mode == "Edit")
            {
                _script?.Update(new ScriptData
                {
                    Title = ViewModel.Title,
                    Description = ViewModel.Description,
                    Arguments = ViewModel.Arguments,
                    RunAsAdmin = ViewModel.RunAsAdmin,
                    WorkingDirectory = ViewModel.WorkingDirectory,
                    ScriptPath = ViewModel.ScriptPath,
                    Data = ViewModel.Script
                });
            }

            Storage.Save(_scripts);

            Hide();
        }
    }

    public class ScriptViewModel : INotifyPropertyChanged
    {
        private string _title = "";
        private string _description = "";
        private string _arguments = "-ExecutionPolicy Bypass -NoExit -NoProfile";
        private bool _runAsAdmin = false;
        private string _scriptPath = "";
        private string _workingDirectory = "";
        private string _script = "";
        private string _mode = "New";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Arguments
        {
            get => _arguments;
            set
            {
                _arguments = value;
                OnPropertyChanged(nameof(Arguments));
            }
        }

        public bool RunAsAdmin
        {
            get => _runAsAdmin;
            set
            {
                _runAsAdmin = value;
                OnPropertyChanged(nameof(RunAsAdmin));
            }
        }

        public string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                _workingDirectory = value;
                OnPropertyChanged(nameof(WorkingDirectory));
            }
        }

        public string ScriptPath
        {
            get => _scriptPath;
            set
            {
                _scriptPath = value;
                OnPropertyChanged(nameof(ScriptPath));
            }

        }

        public string Script
        {
            get => _script;
            set
            {
                _script = value;
                OnPropertyChanged(nameof(Script));
            }
        }

        public string Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
