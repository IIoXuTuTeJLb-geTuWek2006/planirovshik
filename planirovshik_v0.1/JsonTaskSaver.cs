using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace planirovshik_v0._1
{
    public class JsonTaskSaver : ISaveList<List<TaskItem>>
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        public List<TaskItem> Load(string path)
        {
            if (!File.Exists(path)) return new List<TaskItem>();

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, _options)
                   ?? new List<TaskItem>();
        }

        public void Save(List<TaskItem> data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }
    }
}
