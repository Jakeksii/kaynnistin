using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace käynnistin
{
    internal static class Storage
    {
        private static readonly string fileName = "save.json";

        public static bool Save(ObservableCollection<Script> scripts)
        {
            try
            {
                // Write data to file
                var json = JsonSerializer.Serialize(scripts);
                File.WriteAllText(fileName, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static ObservableCollection<Script> Load()
        {
            try
            {
                // Load data from file
                var json = File.ReadAllText(fileName);
                var list = JsonSerializer.Deserialize<ObservableCollection<Script>>(json);
                if (list == null)
                {
                    return [];
                }
                return list;
            }
            catch
            {
                return [];
            }
        }
    }
}
