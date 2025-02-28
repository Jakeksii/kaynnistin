using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace käynnistin
{
    internal class Script(string title, string description, string Arguments, bool runAsAdmin, string workingDirectory, string scriptPath, string data) : INotifyPropertyChanged
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public string Arguments { get; private set; } = Arguments;
        public bool RunAsAdmin { get; private set; } = runAsAdmin;
        public string WorkingDirectory { get; private set; } = workingDirectory;
        public string ScriptPath { get; private set; } = scriptPath;
        public string Data { get; private set; } = data;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Update(ScriptData updateInfo)
        {
            Title = updateInfo.Title ?? Title;
            Description = updateInfo.Description ?? Description;
            Arguments = updateInfo.Arguments ?? Arguments;
            RunAsAdmin = updateInfo.RunAsAdmin ?? RunAsAdmin;
            ScriptPath = updateInfo.ScriptPath ?? ScriptPath;
            Data = updateInfo.Data ?? Data;

            // Update UI
            // Loop through every prop in updateInfo
            foreach (var property in typeof(ScriptData).GetProperties())
            {
                var newValue = property.GetValue(updateInfo);
                if (newValue == null) // if prop was not passed then skip
                    continue;

                var targetProp = this.GetType().GetProperty(property.Name);
                if (targetProp != null && targetProp.CanWrite)
                {
                    targetProp.SetValue(this, newValue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));
                }
            }
        }

        public void Launch()
        {
            // Validate input: Either ScriptPath or Data must be provided
            if (string.IsNullOrWhiteSpace(Data) && string.IsNullOrWhiteSpace(ScriptPath))
            {
                MessageBox.Show("Both script path and content are empty.");
                return;
            }

            string path;
            string dir = Environment.CurrentDirectory;

            // Validate ScriptPath if provided
            if (!string.IsNullOrWhiteSpace(ScriptPath))
            {
                if (!File.Exists(ScriptPath))
                {
                    MessageBox.Show("Script Path is provided but is invalid or file does not exist.");
                    return;
                }

                // Use the directory of the script file as the working directory
                path = ScriptPath;
                dir = Path.GetDirectoryName(ScriptPath) ?? Environment.CurrentDirectory;
            }
            else
            {
                // Create a temporary .ps1 file if Data is provided instead of ScriptPath
                path = Path.Combine(Path.GetTempPath(), $"{Id}.ps1");
                try
                {
                    File.WriteAllText(path, Data, new UTF8Encoding(true));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error writing script file: {ex.Message}");
                    return;
                }
            }

            // Validate WorkingDirectory if provided
            if (!string.IsNullOrWhiteSpace(WorkingDirectory))
            {
                if (!Directory.Exists(WorkingDirectory))
                {
                    MessageBox.Show("Working directory is provided but is invalid.");
                    return;
                }
                dir = WorkingDirectory;
            }

            try
            {
                // Prepare the PowerShell process with correct arguments
                ProcessStartInfo psi = new()
                {
                    FileName = "powershell.exe",
                    Arguments = $"{Arguments} -File \"{path}\"",
                    WorkingDirectory = dir,
                    Verb = RunAsAdmin ? "runas" : "", // Request administrative privileges if needed
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                // Start the process
                Process? process = Process.Start(psi);

                if (process == null)
                {
                    MessageBox.Show("Failed to start PowerShell script.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running script: {ex.Message}");
            }
        }
    }
    public class ScriptData
    {
        public string? Title;
        public string? Description;
        public string? Arguments;
        public bool? RunAsAdmin;
        public string? WorkingDirectory;
        public string? ScriptPath;
        public string? Data;
    }

}
