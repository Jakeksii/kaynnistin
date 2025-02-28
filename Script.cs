using System.ComponentModel;

namespace käynnistin
{
    internal class Script(string title, string description, string data) : INotifyPropertyChanged
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public string Data { get; private set; } = data;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Update(ScriptData updateInfo)
        {
            Title = updateInfo.Title ?? Title;
            Description = updateInfo.Description ?? Description;
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
            // run powershell script

            // REMOVE THESE LINES
            Title = "launch";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }
    }
    public class ScriptData
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Data { get; set; }
    }

}
